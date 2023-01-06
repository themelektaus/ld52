using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Pending
{
    public class ToRenderTexture : MonoBehaviour
    {
        public event System.Action<RenderTexture> onRender;

        [SerializeField] Shader shader;
        [SerializeField] Transform shaderLightDirection;
        [SerializeField] float shaderLightPower = 1;
        [SerializeField] float shaderCeilSteps = 1;

        [SerializeField] Transform cameraPosition;
        [SerializeField, ReadOnly] RenderTexture renderTexture;
        [SerializeField] List<Renderer> renderers;

        GameObject _tempGameObject;
        Camera _camera;

        [Button, SerializeField] bool _Render;
        void Render()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorWindow.FocusWindowIfItsOpen<UnityEditor.SceneView>();
                if (UnityEditor.EditorWindow.focusedWindow is not UnityEditor.SceneView)
                    return;
            }
#endif

            Camera.onPostRender -= Camera_OnPostRender1;
            Camera.onPostRender -= Camera_OnPostRender2;

            if (renderTexture)
                DestroyInternal(renderTexture);

            if (_tempGameObject)
                DestroyInternal(_tempGameObject);

            if (renderers.Count == 0)
                return;

            renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 4
            };
            renderTexture.Create();

            _tempGameObject = new();
            _tempGameObject.transform.SetParent(transform, false);

            GameObject cameraGameObject;

            cameraGameObject = new GameObject();
            cameraGameObject.transform.SetParent(cameraPosition, false);

            _camera = cameraGameObject.AddComponent<Camera>();
            _camera.clearFlags = CameraClearFlags.Depth;
            _camera.cullingMask = LayerMask.GetMask("For Render Texture");
            _camera.fieldOfView = 30;
            _camera.renderingPath = RenderingPath.Forward;
            _camera.targetTexture = renderTexture;
            _camera.allowHDR = false;
            _camera.allowMSAA = false;

            cameraGameObject.transform.SetParent(_tempGameObject.transform);

            foreach (var renderer in renderers)
            {
                var child = renderer.gameObject.Instantiate(_tempGameObject.transform);
                child.layer = LayerMask.NameToLayer("For Render Texture");
                if (shader && child.TryGetComponent(out Renderer x))
                {
                    x.material.shader = shader;

                    if (shaderLightDirection)
                    {
                        Vector4 lightDirection = -shaderLightDirection.forward;
                        lightDirection.w = 1;
                        x.material.SetVector("_LightDirection", lightDirection);
                    }

                    x.material.SetFloat("_LightPower", shaderLightPower);
                    x.material.SetFloat("_CeilSteps", shaderCeilSteps);
                }
            }

            Camera.onPostRender += Camera_OnPostRender1;
        }

        void Camera_OnPostRender1(Camera camera)
        {
            if (_camera != camera)
                return;

            Camera.onPostRender -= Camera_OnPostRender1;
            _camera.targetTexture = null;
            _camera.enabled = false;

            Camera.onPostRender += Camera_OnPostRender2;
        }

        void Camera_OnPostRender2(Camera camera)
        {
            Camera.onPostRender -= Camera_OnPostRender2;

            onRender?.Invoke(renderTexture);

            if (_tempGameObject)
                DestroyInternal(_tempGameObject);
        }

        void OnEnable()
        {
            Render();
        }

        void DestroyInternal(Object @object)
        {
            if (Application.isPlaying)
            {
                Destroy(@object);
                return;
            }
            DestroyImmediate(@object);
        }
    }
}