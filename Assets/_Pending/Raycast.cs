using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(Raycast))]
    public class Raycast : MonoBehaviour
    {
        [SerializeField] ObjectQuery objectQuery;
        
        public readonly Observable<RaycastHit> subject = new();

        public enum RayType
        {
            None,
            MousePosition
        }

        [SerializeField] RayType rayType;

        Ray ray;
        RaycastHit[] raycastHits;

        void Awake()
        {
            objectQuery.ClearCache();
        }

        void Update()
        {
            switch (rayType)
            {
                case RayType.MousePosition:

                    ray = Utils.mainCamera.ScreenPointToRay(Input.mousePosition);
                    raycastHits = Physics.RaycastAll(ray, Mathf.Infinity);

                    foreach (var hit in raycastHits)
                        subject.Notify(hit);

                    break;
            }
        }
    }
}