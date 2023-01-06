using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Prototype
{
    public static class Utils
    {
        #region Assemblies
        static readonly string[] assemblyInclusions = new[] {
            "UnityEngine",
            "UnityEngine.CoreModule",
            "UnityEngine.PhysicsModule",
            "UnityEditor",
            "UnityEditor.CoreModule"
        };

        static readonly string[] assemblyExclusions = new[] {
            "0Harmony",
            "Bee",
            "Cinemachine",
            "clipper_library",
            "com.unity.cinemachine.editor",
            "ExCSS",
            "HarmonySharedState",
            "HBAO",
            "Mono",
            "Microsoft.CSharp",
            "mscorlib",
            "netstandard",
            "nunit.framework",
            "PlayerBuildProgramLibrary",
            "PsdPlugin",
            "ScriptCompilationBuildProgram",
            "System",
            "Unity",
            "WebGLPlayerBuildProgram"
        };
        #endregion

        static HashSet<Assembly> _assemblies;
        static HashSet<Assembly> assemblies
        {
            get
            {
                if (_assemblies is null)
                    _assemblies = new();

                if (_assemblies.Count == 0)
                {
                    foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (assemblyInclusions.Any(x => assembly.FullName.Split(',')[0] == x))
                            _assemblies.Add(assembly);

                        else if (!assemblyExclusions.Any(x => assembly.FullName.StartsWith(x)))
                            _assemblies.Add(assembly);

                        continue;
                    }
                }

                var contents = new List<string>();
                foreach (var assembly in _assemblies)
                    contents.Add(assembly.FullName.Split(',')[0]);

#if UNITY_EDITOR
                System.IO.Directory.CreateDirectory("Temp");
                System.IO.File.WriteAllLines(@"Temp\Assemblies.txt", contents);
#endif

                return _assemblies;
            }
        }

        static HashSet<System.Type> _types;
        public static HashSet<System.Type> types
        {
            get
            {
                if (_types is null)
                    _types = new();

                if (_types.Count == 0)
                    foreach (var assembly in assemblies)
                        foreach (var type in assembly.GetTypes())
                            _types.Add(type);

                return _types;
            }
        }

        public static void DestroyImmediateInEditor(Object obj)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(obj);
                return;
            }

            Object.DestroyImmediate(obj);
        }

        public static bool Approximately(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }

        public static bool Approximately(float a, float b, float threshold)
        {
            return Mathf.Abs(a - b) < threshold;
        }

        public static bool Approximately(Vector2 a, Vector2 b)
        {
            return Mathf.Approximately(a.x, b.x)
                && Mathf.Approximately(a.y, b.y);
        }

        public static bool Approximately(Vector2 a, Vector2 b, float threshold)
        {
            return Approximately(a.x, b.x, threshold)
                && Approximately(a.y, b.y, threshold);
        }

        public static bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x)
                && Mathf.Approximately(a.y, b.y)
                && Mathf.Approximately(a.z, b.z);
        }

        public static bool Approximately(Vector3 a, Vector3 b, float threshold)
        {
            return Approximately(a.x, b.x, threshold)
                && Approximately(a.y, b.y, threshold)
                && Approximately(a.z, b.z, threshold);
        }

        public static bool Approximately(Quaternion a, Quaternion b)
        {
            return Approximately(a.x, b.x)
                && Approximately(a.y, b.y)
                && Approximately(a.z, b.z)
                && Approximately(a.w, b.w);
        }

        public static bool Approximately(Quaternion a, Quaternion b, float threshold)
        {
            return Approximately(a.x, b.x, threshold)
                && Approximately(a.y, b.y, threshold)
                && Approximately(a.z, b.z, threshold)
                && Approximately(a.w, b.w, threshold);
        }

        public static float Clamp(float value, Vector2 range)
        {
            return Mathf.Clamp(value, range.x, range.y);
        }

        public static T RandomPick<T>(IList<T> collection)
        {
            int index = Random.Range(0, collection.Count);
            return collection[index];
        }

        public static float RandomRange(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }

        public static Vector2 RandomRange(Vector2 min, Vector2 max)
        {
            return new(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );
        }

        public static Vector3 RandomRange(Vector3 min, Vector3 max)
        {
            return new(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
            );
        }

        public static Vector3 RandomPointInsideCollider(Collider collider)
        {
            var bounds = collider.bounds;
            var point = RandomRange(bounds.min, bounds.max);

            if (Approximately(point, collider.ClosestPoint(point)))
                return point;

            return RandomPointInsideCollider(collider);
        }

        public static Vector2 RandomPointInsideCollider(Collider2D collider)
        {
            var bounds = collider.bounds;
            var point = RandomRange((Vector2) bounds.min, (Vector2) collider.bounds.max);

            if (Approximately(point, collider.ClosestPoint(point)))
                return point;

            return RandomPointInsideCollider(collider);
        }

        public static T Invoke<T>(System.Func<T> func, T @default = default)
        {
            if (func is null)
                return @default;
            return func.Invoke();
        }

#if UNITY_EDITOR
        public static Texture2D Screenshot(Rect rect)
        {
            var position = new Vector2Int((int) rect.x, (int) rect.y);
            var size = new Vector2Int((int) rect.width, (int) rect.height);
            var pixels = UnityEditorInternal.InternalEditorUtility.ReadScreenPixel(position, size.x, size.y);
            var texture = new Texture2D(size.x, size.y);
            texture.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
#endif



        static readonly FrameTimeVariable<List<RaycastResult>> pointerRaycastResults = new(x =>
        {
            x.Clear();
            var e = EventSystem.current;
            if (e) e.RaycastAll(new(e) { position = Input.mousePosition }, x);
            return x;
        });

        public static List<RaycastResult> GetPointerRaycastResults()
            => pointerRaycastResults;

        public static bool IsMouseButton_ButNotUI(int button)
        {
            if (!Input.GetMouseButton(button))
                return false;

            if (IsPointerOverUI())
                return false;

            return true;
        }

        static readonly int uiLayer = LayerMask.NameToLayer("UI");

        public static bool IsPointerOverUI()
        {
            var results = GetPointerRaycastResults();
            return results.Any(x => x.gameObject.layer == uiLayer);
        }

        public static bool IsPointerOver(GameObject gameObject)
        {
            foreach (var result in GetPointerRaycastResults())
                if (result.gameObject == gameObject)
                    return true;
            return false;
        }

        public static bool IsMouseButtonDown_ButNotUI(int button)
        {
            if (!Input.GetMouseButtonDown(button))
                return false;

            if (IsPointerOverUI())
                return false;

            return true;
        }

        public static bool IsMouseButtonUp_ButNotUI(int button)
        {
            if (!Input.GetMouseButtonUp(button))
                return false;

            if (IsPointerOverUI())
                return false;

            return true;
        }



        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int X;
            public int Y;

            public static explicit operator Vector2Int(POINT point)
            {
                return new(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        public static Vector2Int GetMousePosition()
        {
            GetCursorPos(out var point);
            return (Vector2Int) point;
        }

        public static void SetMousePosition(Vector2Int position)
        {
            SetCursorPos(position.x, position.y);
        }

        public static Camera mainCamera
        {
            get
            {
                var camera = Camera.main;
                if (!camera)
                {
                    GameObject cameraObject = new("Main Camera") { tag = "MainCamera" };
                    camera = cameraObject.AddComponent<Camera>();
                    camera.clearFlags = CameraClearFlags.SolidColor;
                    camera.backgroundColor = new(.135f, .135f, .142f);
                    cameraObject.AddComponent<AudioListener>();
                }
                return camera;
            }
        }
    }
}