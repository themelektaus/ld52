using UnityEngine;

namespace Prototype.Pending
{
    public interface ICameraTarget
    {
        public Vector3 GetLookAtPosition();
        public Vector3 GetFollowPosition();
    }
}