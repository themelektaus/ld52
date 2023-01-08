using UnityEngine;

namespace Prototype
{
    public class LD52_Harvest : MonoBehaviour
    {
        public static Observable<HarvestMessage> harvestSubject = new();

        public struct HarvestMessage
        {
            public LD52_Harvest sender;
            public Vector2 position;
            public float strength;
        }

        public float radius = 5;

        [SerializeField] Transform model;
        [SerializeField] float maxDistanceToPlayer = 1;

        void Update()
        {
            model.localScale = new(radius * 2, model.localScale.y, radius * 2);

            if (LD52_GameStateMachine.instance.IsIngamePaused())
                return;

            if (!LD52_Global.GetInputHarvest())
            {
                model.gameObject.SetActive(false);
                return;
            }

            model.gameObject.SetActive(true);

            var player = LD52_Global.instance.GetPlayer();
            if (!player)
                return;

            var hit = LD52_Global.GetMouseGroundHit();
            if (!hit.HasValue)
                return;

            var cursorPosition = hit.Value.point.ToXZ();
            var playerPosition = player.character.agentPosition.ToXZ();

            var direction = cursorPosition - playerPosition;
            var distance = direction.magnitude;

            Vector2 position;
            if (distance > maxDistanceToPlayer)
                position = playerPosition + direction.normalized * maxDistanceToPlayer;
            else
                position = cursorPosition;

            harvestSubject.Notify(new() { sender = this, position = position, strength = 1 });

            transform.position = position.ToX0Z();
        }
    }
}