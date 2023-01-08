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

        public float radius { get; private set; }
        float strength;

        [SerializeField] Transform model;

        [SerializeField] GameObject circle;
        [SerializeField] ParticleSystem particleEffect;

        [SerializeField] float maxDistanceToPlayer = 1;

        void Awake()
        {
            radius = LD52_Global.instance.upgrades.harvestRadius.GetCurrent();
            strength = LD52_Global.instance.upgrades.harvestStrength.GetCurrent();
        }

        void Update()
        {
            model.localScale = new(radius * 2, model.localScale.y, radius * 2);

            if (LD52_GameStateMachine.instance.IsIngamePaused())
                return;

            if (!LD52_Global.GetInputHarvest())
            {
                circle.SetActive(false);
                var emission2 = particleEffect.emission;
                emission2.enabled = false;
                return;
            }

            circle.SetActive(true);
            var emission = particleEffect.emission;
            emission.enabled = true;

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

            harvestSubject.Notify(new() { sender = this, position = position, strength = strength });

            transform.position = position.ToX0Z();
        }
    }
}