using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Prototype
{
    public static class ExtensionMethods
    {
        public static T GetComponent<T>(this Object @this)
        {
            if (@this is T t)
                return t;

            if (@this is Component component)
                return component.GetComponent<T>();

            if (@this is GameObject gameObject)
                return gameObject.GetComponent<T>();

            return default;
        }

        public static T[] GetComponents<T>(this Object @this)
        {
            if (@this is T t)
                return new[] { t };

            if (@this is Component component)
                return component.GetComponents<T>();

            if (@this is GameObject gameObject)
                return gameObject.GetComponents<T>();

            return new T[0];
        }

        public static T AddComponent<T>(this GameObject @this, Action<T> onAdd) where T : Component
        {
            bool activeSelf = @this.activeSelf;
            if (activeSelf)
                @this.SetActive(false);
            var component = @this.AddComponent<T>();
            onAdd(component);
            if (activeSelf)
                @this.SetActive(true);
            return component;
        }

        public static T GetOrAddComponent<T>(this GameObject @this) where T : Component
        {
            var component = @this.GetComponent<T>();
            if (!component)
                component = @this.AddComponent<T>();
            return component;
        }

        public static T GetOrAddComponent<T>(this GameObject @this, Action<T> onAdd) where T : Component
        {
            var component = @this.GetComponent<T>();
            if (!component)
                component = @this.AddComponent(onAdd);
            return component;
        }

        public static void RemoveComponent<T>(this GameObject @this) where T : Component
        {
            var component = @this.GetComponent<T>();
            Object.Destroy(component);
        }

        public static GameObject Instantiate(this GameObject @this)
        {
            return @this.InstantiateInternal(new());
        }

        public static GameObject Instantiate(this GameObject @this, Action<GameObject> onInstantiate)
        {
            return @this.InstantiateInternal(new() { onInstantiate = onInstantiate });
        }

        public static GameObject Instantiate(this GameObject @this, Transform parent)
        {
            return @this.InstantiateInternal(new() { parent = parent });
        }

        public static GameObject Instantiate(this GameObject @this, Transform parent, Vector3 position)
        {
            return @this.InstantiateInternal(new() { parent = parent, position = position });
        }

        public static GameObject Instantiate(this GameObject @this, Vector3 position)
        {
            return @this.InstantiateInternal(new() { position = position });
        }

        public static GameObject Instantiate(this GameObject @this, Vector3 position, Quaternion rotation)
        {
            return @this.InstantiateInternal(new() { position = position, rotation = rotation });
        }

        public static GameObject Instantiate(this GameObject @this, Vector3 position, float scale)
        {
            return @this.InstantiateInternal(new() { position = position, scale = Vector3.one * scale });
        }

        public static GameObject Instantiate(this GameObject @this, Vector3 position, Vector3 scale)
        {
            return @this.InstantiateInternal(new() { position = position, scale = scale });
        }

        public static GameObject Instantiate(this GameObject @this, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            return @this.InstantiateInternal(new() { position = position, rotation = rotation, scale = scale });
        }

        struct InstantiationOptions
        {
            public Transform parent;
            public Vector3? position;
            public Quaternion? rotation;
            public Vector3? scale;
            public Action<GameObject> onInstantiate;
        }

        static GameObject InstantiateInternal(this GameObject @this, InstantiationOptions options)
        {
            GameObject gameObject;

            if (@this.TryGetComponent<PoolObject>(out _))
                gameObject = Pool.Instantiate(@this, options.parent);
            else
                gameObject = Object.Instantiate(@this, options.parent);

            bool activeSelf = @this.activeSelf;

            if (options.onInstantiate is not null)
            {
                if (activeSelf)
                    gameObject.SetActive(false);

                options.onInstantiate(gameObject);
            }

            var transform = gameObject.transform;

            if (options.position.HasValue)
                transform.position = options.position.Value;

            if (options.rotation.HasValue)
                transform.rotation = options.rotation.Value;

            if (options.scale.HasValue)
                transform.localScale = options.scale.Value;

            if (options.onInstantiate is not null && activeSelf)
                gameObject.SetActive(true);

            return gameObject;
        }

        public static void Destroy(this GameObject @this)
        {
            if (@this.TryGetComponent<IDestroyable>(out var destroyable))
            {
                destroyable.Destroy();
                return;
            }

            @this.Kill();
        }

        public static List<GameObject> GetChildren(this GameObject @this)
        {
            var gameObjects = new List<GameObject>();

            foreach (Transform transform in @this.transform)
                gameObjects.Add(transform.gameObject);

            return gameObjects;
        }

        public static void DestroyChildren(this GameObject @this)
        {
            foreach (var gameObject in @this.GetChildren())
                gameObject.Destroy();
        }

        public static void DestroyChildrenImmediate(this GameObject @this)
        {
            foreach (var gameObject in @this.GetChildren())
                Object.DestroyImmediate(gameObject);
        }

        public static void Kill(this GameObject @this)
        {
            if (@this.TryGetComponent<PoolObject>(out _))
            {
                Pool.Destroy(@this);
                return;
            }

            Object.Destroy(@this);
        }

        public static void KillChildren(this GameObject @this)
        {
            foreach (var gameObject in @this.GetChildren())
                gameObject.Kill();
        }

        public static T[] Filter<T>(this IEnumerable<object> @this) where T : class
        {
            return @this.Select(x => x as T)
                        .Where(x => x is Object ? (x as Object) : x is not null)
                        .ToArray();
        }

        public static string ToFormattedString(this float @this, FloatFormat format)
        {
            var @string = format switch
            {
                FloatFormat.Raw => @this.ToString(),
                FloatFormat.OneDecimal => $"{@this:0.0}",
                FloatFormat.TwoDecimals => $"{@this:0.00}",
                FloatFormat.ThreeDecimals => $"{@this:0.000}",
                FloatFormat.Percent => $"{Mathf.RoundToInt(@this * 100)}%",
                _ => "",
            };

            return @string.Replace(',', '.');
        }

        public static AnimatorControllerParameter GetParameter(this Animator @this, string name)
        {
            var parameters = @this.GetParameters();

            if (parameters.ContainsKey(name))
                return parameters[name];

            return null;
        }

        static readonly Dictionary<Animator, Dictionary<string, AnimatorControllerParameter>> animatorParameters = new();

        static Dictionary<string, AnimatorControllerParameter> GetParameters(this Animator @this)
        {
            if (animatorParameters.ContainsKey(@this))
                return animatorParameters[@this];

            var parameters = new Dictionary<string, AnimatorControllerParameter>();

            foreach (var parameter in @this.parameters)
                parameters[parameter.name] = parameter;

            animatorParameters.Add(@this, parameters);

            return parameters;
        }

        public static string[] GetParameterNames(this Animator @this)
        {
            return @this.GetParameters().Select(x => x.Key).ToArray();
        }

        public static object GetParameterValue(this Animator @this, string name)
        {
            if (!Application.isPlaying)
                return null;

            var parameter = @this.GetParameter(name);
            if (parameter is null)
                return null;

            return parameter.type switch
            {
                AnimatorControllerParameterType.Float => @this.GetFloat(name),
                AnimatorControllerParameterType.Int => @this.GetInteger(name),
                AnimatorControllerParameterType.Bool => @this.GetBool(name),
                _ => null,
            };
        }

        public static bool SetParameterValue(this Animator @this, string name, object value)
        {
            var parameter = @this.GetParameter(name);
            if (parameter is null)
                return false;

            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Float:
                    if (value is float floatValue)
                    {
                        @this.SetFloat(name, floatValue);
                        return true;
                    }
                    break;

                case AnimatorControllerParameterType.Int:
                    if (value is int intValue)
                    {
                        @this.SetInteger(name, intValue);
                        return true;
                    }
                    break;

                case AnimatorControllerParameterType.Bool:
                    if (value is bool boolValue)
                    {
                        @this.SetBool(name, boolValue);
                        return true;
                    }
                    break;
            }

            return false;
        }

        struct ShaderPropertyField
        {
            public bool x;
            public bool y;
            public bool z;
            public bool w;

            public bool r;
            public bool g;
            public bool b;
            public bool a;

            public static ShaderPropertyField From(string fields)
            {
                return new ShaderPropertyField
                {
                    x = fields.Contains('x'),
                    y = fields.Contains('y'),
                    z = fields.Contains('z'),
                    w = fields.Contains('w'),

                    r = fields.Contains('r'),
                    g = fields.Contains('g'),
                    b = fields.Contains('b'),
                    a = fields.Contains('a'),
                };
            }

            public bool IsVector() => x || y || z || w;
            public bool IsColor() => r || g || b || a;
        }

        public static float GetValue(this Material @this, string name)
        {
            if (!Application.isPlaying)
                return 0;

            if (name.Contains('.'))
            {
                var parts = name.Split('.');

                name = parts[0];

                var field = ShaderPropertyField.From(parts[1]);

                if (field.IsVector())
                {
                    var vector = @this.GetVector(name);

                    if (field.x) return vector.x;
                    if (field.y) return vector.y;
                    if (field.z) return vector.z;
                    if (field.w) return vector.w;
                }

                if (field.IsColor())
                {
                    var color = @this.GetColor(name);

                    if (field.r) return color.r;
                    if (field.g) return color.g;
                    if (field.b) return color.b;
                    if (field.a) return color.a;
                }
            }

            return @this.GetFloat(name);
        }

        public static void SetValue(this Material @this, string name, float value)
        {
            if (name.Contains('.'))
            {
                var parts = name.Split('.');

                name = parts[0];

                var field = ShaderPropertyField.From(parts[1]);

                if (field.IsVector())
                {
                    var vector = @this.GetVector(name);

                    if (field.x)
                        vector.x = value;

                    else if (field.y)
                        vector.y = value;

                    else if (field.z)
                        vector.z = value;

                    else if (field.w)
                        vector.w = value;

                    @this.SetVector(name, vector);

                    return;
                }

                if (field.IsColor())
                {
                    var color = @this.GetColor(name);

                    if (field.r)
                        color.r = value;

                    else if (field.g)
                        color.g = value;

                    else if (field.b)
                        color.b = value;

                    else if (field.a)
                        color.a = value;

                    @this.SetColor(name, color);

                    return;
                }
            }

            @this.SetFloat(name, value);
        }

        public static GameObject GetGameObject(this Object @this)
        {
            if (@this is GameObject gameObject)
                return gameObject;

            if (@this is Component component)
                return component.gameObject;

            return null;
        }

        public static Transform GetTransform(this Object @this)
        {
            if (@this is Transform transform)
                return transform;

            if (@this is GameObject gameObject)
                return gameObject.transform;

            if (@this is Component component)
                return component.transform;

            return null;
        }

        public static Vector2 ToXZ(this Vector3 @this)
        {
            return new Vector2(@this.x, @this.z);
        }

        public static Vector3 ToX0Z(this Vector2 @this)
        {
            return new Vector3(@this.x, 0, @this.y);
        }

        public static Vector3 ToXYZ(this Vector2 @this, float y)
        {
            return new Vector3(@this.x, y, @this.y);
        }

        public static void SubscribeTo<T>(this IObserver<T> @this, IObservable<T> observable, Predicate<T> validation = null)
        {
            observable.Register(@this, validation);
        }

        public static void UnsubscribeFrom<T>(this IObserver<T> @this, IObservable<T> observable)
        {
            observable.Unregister(@this);
        }

        public static Collider[] CheckCollisions(this Collider @this)
        {
            var transform = @this.transform;

            if (@this is BoxCollider boxCollider)
            {
                var center = transform.TransformPoint(boxCollider.center);
                var halfExtents = Vector3.Scale(boxCollider.size / 2, transform.lossyScale);
                var orientation = transform.rotation;
                return Physics.OverlapBox(center, halfExtents, orientation);
            }

            if (@this is SphereCollider sphereCollider)
            {
                var center = transform.TransformPoint(sphereCollider.center);
                var radius = sphereCollider.radius * transform.lossyScale.x;
                return Physics.OverlapSphere(center, radius);
            }

            Debug.LogWarning($"{@this.GetType()} is not supported");

            return Array.Empty<Collider>();
        }

        public static Quaternion? GetLookRotation(this Transform @this, Vector3 target)
        {
            var dir = (target - @this.position).normalized;

            if (Utils.Approximately(dir, Vector3.zero))
                return null;

            return Quaternion.LookRotation(dir);
        }

        public static Texture2D AsBase64ToTexture(this string @this)
        {
            var texture = new Texture2D(1, 1);
            texture.LoadImage(Convert.FromBase64String(@this));
            return texture;
        }

        public static string ToBase64String(this Texture2D @this)
        {
            byte[] a = @this.EncodeToJPG(80);
            return Convert.ToBase64String(a);
        }

        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> @this, Func<TKey, TValue, bool> match)
        {
            var keys = @this.Keys.ToArray();
            foreach (var key in keys.Where(key => match(key, @this[key])))
                @this.Remove(key);
        }

        public static Sequence CreateSequence(this MonoBehaviour @this)
            => new(@this);

        public static Sequence CreateSequence(this MonoBehaviour @this, Action action)
            => @this.CreateSequence().Then(action);

        public static Sequence Wait(this MonoBehaviour @this, float seconds)
            => @this.CreateSequence().Wait(seconds);

        public static Sequence While(this MonoBehaviour @this, Func<bool> condition)
            => @this.CreateSequence().While(condition);

        public static Sequence.Instance Start(this MonoBehaviour @this, System.Collections.IEnumerator enumerator)
            => @this.CreateSequence().Then(enumerator).Start();

        public static void Enable(this Object @this) => @this.Enable(true);
        public static void Disable(this Object @this) => @this.Enable(false);
        static void Enable(this Object @this, bool enabled)
        {
            if (@this is Behaviour behaviour)
            {
                behaviour.enabled = enabled;
                return;
            }

            if (@this is Renderer renderer)
            {
                renderer.enabled = enabled;
                return;
            }

            if (@this is Collider collider)
            {
                collider.enabled = enabled;
                return;
            }

            @this.LogWarning(enabled ? "Enable" : "Disable");
        }

        public static bool IsEnabled(this Object @this)
        {
            if (@this is Behaviour behaviour)
                return behaviour.enabled;

            if (@this is Renderer renderer)
                return renderer.enabled;

            if (@this is Collider collider)
                return collider.enabled;

            @this.LogWarning("Unsupported");
            return false;
        }

        public static int Pow(this int @this, int p)
        {
            if (p < 0) throw new Exception("p less than zero is not supported");
            if (p == 0) return 0;
            int result = @this;
            for (int i = 1; i < p; i++)
                result *= @this;
            return result;
        }

        public static float RoundTo(this float @this, int decimals)
        {
            int power = 10.Pow(decimals);
            return Mathf.Round(@this * power) / power;
        }

        public static Vector2 RoundTo(this Vector2 @this, int decimals)
        {
            var power = 10.Pow(decimals);
            @this.x = Mathf.Round(@this.x * power) / power;
            @this.y = Mathf.Round(@this.y * power) / power;
            return @this;
        }

        public static Vector3 RoundTo(this Vector3 @this, int decimals)
        {
            var power = 10.Pow(decimals);
            @this.x = Mathf.Round(@this.x * power) / power;
            @this.y = Mathf.Round(@this.y * power) / power;
            @this.z = Mathf.Round(@this.z * power) / power;
            return @this;
        }

        public static Vector4 RoundTo(this Vector4 @this, int decimals)
        {
            var power = 10.Pow(decimals);
            @this.x = Mathf.Round(@this.x * power) / power;
            @this.y = Mathf.Round(@this.y * power) / power;
            @this.z = Mathf.Round(@this.z * power) / power;
            @this.w = Mathf.Round(@this.w * power) / power;
            return @this;
        }

        public static void RoundTo(this Transform @this, int decimals)
        {
            @this.localPosition = @this.localPosition.RoundTo(decimals);
            @this.localEulerAngles = @this.localEulerAngles.RoundTo(decimals);
            @this.localScale = @this.localScale.RoundTo(decimals);
        }
    }
}