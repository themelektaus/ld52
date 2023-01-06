using System.Reflection;

using UnityEngine;

using Image = UnityEngine.UI.Image;

namespace Prototype
{
    [System.Serializable]
    public class Reference
    {
        public const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public enum Type
        {
            Member,
            AnimatorParameter,
            ShaderProperty
        }

        public Type type = Type.Member;
        public Object @object;
        public string path;

        [ReadOnly] public string serializedData;

        object obj;
        object info;
        string sub;

        int hashCode;
        
        void Update()
        {
            var hashCode = System.HashCode.Combine(@object, path);

            if (this.hashCode == hashCode)
                return;

            this.hashCode = hashCode;

            obj = null;
            info = null;
            sub = null;

            if (!@object || path is null)
                return;

            path = path.Trim();
            if (path == string.Empty)
                return;

            obj = @object;

            if (type == Type.Member)
            {
                if (path.EndsWith("()"))
                {
                    var methodInfo = obj.GetType().GetMethod(path[..^2]);
                    if (methodInfo is null)
                        return;

                    info = methodInfo;
                    return;
                }

                string[] parts = path.Split('.');
                for (int i = 0; i < parts.Length; i++)
                {
                    var fieldInfo = obj.GetType().GetField(parts[i], FLAGS);
                    if (fieldInfo is null)
                    {
                        var propertyInfo = obj.GetType().GetProperty(parts[i], FLAGS);
                        if (propertyInfo is null)
                            return;

                        if (FinalizeUpdate(propertyInfo, parts, i))
                            return;

                        continue;
                    }

                    if (FinalizeUpdate(fieldInfo, parts, i))
                        return;
                }
                Debug.Log($"Can not find member \"{path}\"");
                return;
            }

            if (type == Type.AnimatorParameter)
            {
                if (@object is Animator animator)
                    info = animator.GetParameter(path);
                return;
            }

            if (type == Type.ShaderProperty)
            {
                if (@object is Renderer renderer)
                    info = renderer.material;

                if (@object is Image image)
                    info = image.material;

                return;
            }
        }

        bool FinalizeUpdate(object info, string[] parts, int i)
        {
            if (i < parts.Length - 1)
            {
                System.Type type;

                if (info is PropertyInfo propertyInfo)
                    type = propertyInfo.PropertyType;

                else if (info is FieldInfo fieldInfo)
                    type = fieldInfo.FieldType;

                else
                {
                    Debug.LogError("info has no type");
                    return true;
                }

                if (type.IsValueType && !type.IsEnum)
                {
                    this.info = info;
                    sub = type.GetField(parts[i + 1], FLAGS).Name;
                    return true;
                }

                obj = GetValue(obj, info);

                return false;
            }

            this.info = info;

            return true;
        }

        public System.Type GetFieldType()
        {
            Update();
            if (info is not null)
                return GetType(info);
            return null;
        }

        public Reference Of(Object memberOwner)
        {
            @object = memberOwner;
            return this;
        }

        public object Get()
        {
            Update();
            if (info is null)
                return null;

            if (sub is not null)
            {
                var fieldValue = GetValue(obj, info);
                return fieldValue.GetType().GetField(sub, FLAGS).GetValue(fieldValue);
            }

            if (type == Type.AnimatorParameter)
                return (obj as Animator).GetParameterValue(path);

            if (type == Type.ShaderProperty)
            {
                if (info is Material material)
                    return material.GetValue(path);

                if (info is Image image)
                    return image.material.GetValue(path);
            }

            return GetValue(obj, info);
        }

        public T Get<T>(T defaultValue = default)
        {
            var result = Get();

            if (result is null)
                return defaultValue;

            if (result is SmoothValue<T> smoothValue)
                return smoothValue.current;

            return (T) result;
        }

        public void Set(object value) => Set(value, .5f);

        public void Set(object value, float boolThreshold)
        {
            Update();

            if (info is null)
                return;

            if (sub is not null)
            {
                var fieldValue = GetValue(obj, info);
                fieldValue.GetType().GetField(sub, FLAGS).SetValue(fieldValue, value);
                SetValue(obj, info, fieldValue, boolThreshold);
                return;
            }

            if (type == Type.AnimatorParameter)
            {
                (obj as Animator).SetParameterValue(path, value);
                return;
            }

            if (type == Type.ShaderProperty)
            {
                (info as Material).SetValue(path, (float) value);
                return;
            }

            SetValue(obj, info, value, boolThreshold);
        }

        static System.Type GetType(object info)
        {
            if (info is FieldInfo fieldInfo)
                return fieldInfo.FieldType;

            if (info is PropertyInfo propertyInfo)
                return propertyInfo.PropertyType;

            return null;
        }

        static object GetValue(object obj, object info)
        {
            if (info is FieldInfo fieldInfo)
                return fieldInfo.GetValue(obj);

            if (info is PropertyInfo propertyInfo)
                return propertyInfo.GetValue(obj);

            return null;
        }

        static void SetValue(object obj, object info, object value, float boolThreshold)
        {
            if (info is FieldInfo fieldInfo)
            {
                SetValue(fieldInfo.FieldType, value, x => fieldInfo.SetValue(obj, x), boolThreshold);
                return;
            }
            
            if (info is PropertyInfo propertyInfo)
            {
                SetValue(propertyInfo.PropertyType, value, x => propertyInfo.SetValue(obj, x), boolThreshold);
                return;
            }

            if (info is MethodInfo methodInfo)
            {
                if (value is float floatValue)
                {
                    var parameters = methodInfo.GetParameters();
                    if (parameters.Length == 1)
                    {
                        if (parameters[0].ParameterType == typeof(bool))
                        {
                            methodInfo.Invoke(obj, new object[] { Mathf.Abs(floatValue) > boolThreshold });
                            return;
                        }

                        methodInfo.Invoke(obj, new object[] { floatValue });
                        return;
                    }

                    methodInfo.Invoke(obj, new object[0]);
                    return;
                }
            }
        }

        static void SetValue(System.Type type, object value, System.Action<object> setValue, float boolThreshold)
        {
            if (value is float floatValue)
            {
                if (type == typeof(bool))
                {
                    setValue(Mathf.Abs(floatValue) > boolThreshold);
                    return;
                }

                if (type == typeof(Vector2))
                {
                    setValue(Vector2.one * floatValue);
                    return;
                }

                if (type == typeof(Vector3))
                {
                    setValue(Vector3.one * floatValue);
                    return;
                }
            }

            setValue(value);
        }
    }
}