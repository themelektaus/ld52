using UnityEngine;

namespace Prototype
{
	public class LD52_Enemy : MonoBehaviour,
        IObserver<LD52_Projectile.HitMessage>,
        IObserver<LD52_Harvest.HarvestMessage>
    {
        public LD52_Character character;

        [SerializeField] LD52_Global.CharacterSettings characterSettings;

        [SerializeField] float minDistanceToPlayer = 10;

        [SerializeField] LD52_EnemyItem item;

        Vector2 randomMoveDirection;
        float randomMoveDirectionTimer;

        void Awake()
        {
            character.getMoveDirection = GetMoveDirection;
            character.getCharacterSettings = () => characterSettings;
            character.onDie += OnDie;
            character.onHarvest += OnHarvest;
        }

        void OnEnable()
        {
            LD52_Projectile.hitSubject.Register(this, x => character.enabled && x.reciever == character.collider);
            LD52_Harvest.harvestSubject.Register(this, x => !character.enabled);
        }

        void OnDisable()
        {
            LD52_Projectile.hitSubject.Unregister(this);
            LD52_Harvest.harvestSubject.Unregister(this);
        }

        public void ReceiveNotification(LD52_Projectile.HitMessage message)
        {
            message.sender.gameObject.Destroy();
            character.TakeDamage(message.damage);
        }

        public void ReceiveNotification(LD52_Harvest.HarvestMessage message)
        {
            var distance = (character.agentPosition.ToXZ() - message.position).magnitude;
            if (distance > message.sender.radius)
                return;

            character.Harvest(message.strength * Time.deltaTime);
        }

        void OnDie()
        {
            character.enabled = false;

        }

        void OnHarvest()
        {
            var player = character.playerQuery.FindComponent<LD52_Player>();
            if (player && player.Add(item))
                gameObject.Destroy();
        }

        Vector2 GetMoveDirection()
        {
            var player = character.playerQuery.FindComponent<LD52_Player>();
            if (player)
            {
                var direction = character.agentPosition - player.character.agentPosition;
                var distance = direction.magnitude;
                if (distance < minDistanceToPlayer)
                {
                    randomMoveDirectionTimer = 0;
                    return direction.ToXZ();
                }
            }

            randomMoveDirectionTimer -= Time.deltaTime;
            if (randomMoveDirectionTimer <= 0)
            {
                randomMoveDirectionTimer = Random.value + 1;
                randomMoveDirection = Random.insideUnitCircle;
            }

            return randomMoveDirection;
        }
    }
}