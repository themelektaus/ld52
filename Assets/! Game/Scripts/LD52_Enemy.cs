using UnityEngine;
using UnityEngine.AI;

namespace Prototype
{
    public abstract class LD52_Enemy : MonoBehaviour,
        IObserver<LD52_Projectile.HitMessage>,
        IObserver<LD52_Harvest.HarvestMessage>
    {
        public LD52_Character character;

        [SerializeField] LD52_Global.CharacterSettings characterSettings;

        [SerializeField] float minDistanceToPlayer = 10;

        public LD52_EnemyItem item;

        [SerializeField] Transform mesh;
        [SerializeField] bool buried;

        Vector2 randomMoveDirection;
        float randomMoveDirectionTimer;

        float moveAwayTimer;

        Vector2 moveAwayFromWallTarget;
        float moveAwayFromWallTimer;

        void Awake()
        {
            character.getMoveDirection = GetMoveDirection;
            character.getMoveSpeed = GetMoveSpeed;
            character.getCharacterSettings = () => characterSettings;
            character.onDie += OnDie;
            character.onHarvest += OnHarvest;
            character.onDigOut += OnDigOut;
        }

        void OnEnable()
        {
            LD52_Projectile.hitSubject.Register(this, x => character.enabled && x.reciever == character.collider);
            LD52_Harvest.harvestSubject.Register(this, x => buried || !character.enabled);
        }

        void OnDisable()
        {
            LD52_Projectile.hitSubject.Unregister(this);
            LD52_Harvest.harvestSubject.Unregister(this);
        }

        void Update()
        {
            character.agent.enabled = !buried && character.enabled;
        }

        public void ReceiveNotification(LD52_Projectile.HitMessage message)
        {
            if (buried)
                return;

            message.sender.gameObject.Destroy();
            character.TakeDamage(message.damage);
        }

        public void ReceiveNotification(LD52_Harvest.HarvestMessage message)
        {
            var distance = (character.collider.transform.position.ToXZ() - message.position).magnitude;
            if (distance > message.sender.radius)
                return;

            var value = message.strength * Time.deltaTime;

            if (buried)
            {
                character.DigOut(value);
                return;
            }

            character.Harvest(value);
        }

        void OnDie()
        {
            character.enabled = false;
        }

        void OnHarvest()
        {
            var player = LD52_Global.instance.GetPlayer();
            if (player && player.Add(item))
                gameObject.Destroy();

            LD52_Global.instance.sounds.harvest.PlayRandomClip();
        }

        void OnDigOut()
        {
            buried = false;
            character.agent.enabled = true;
            
            var position = mesh.localPosition;
            position.y = -.1f;
            mesh.localPosition = position;

            LD52_Global.instance.sounds.digOut.PlayRandomClip();
        }

        Vector3 GetDirectionToPlayer()
        {
            var player = LD52_Global.instance.GetPlayer();
            if (player)
                return player.character.agentPosition - character.agentPosition;

            return new();
        }

        bool IsNearPlayer()
        {
            var player = LD52_Global.instance.GetPlayer();
            if (player)
            {
                var direction = GetDirectionToPlayer();
                var distance = direction.magnitude;
                return distance < minDistanceToPlayer;
            }
            return false;
        }

        Vector2 GetMoveDirection()
        {
            if (buried)
                return new();

            if (IsNearPlayer())
            {
                randomMoveDirectionTimer = 0;
                moveAwayTimer = 1;
            }

            var direction = character.agentPosition.normalized;

            if (moveAwayFromWallTimer > 0)
            {
                moveAwayFromWallTimer -= Time.deltaTime;
                return moveAwayFromWallTarget;
            }

            if (NavMesh.Raycast(character.agentPosition, character.agentPosition + direction, out var hit, NavMesh.AllAreas))
            {
                var alongTheWall = Vector2.Perpendicular(hit.normal.ToXZ());
                if (Vector2.Dot(alongTheWall, GetDirectionToPlayer().ToXZ()) > 0)
                    alongTheWall *= -1;

                var target = hit.position.ToXZ() - direction.ToXZ() * 4 + alongTheWall * 4;
                moveAwayFromWallTarget = target - character.agentPosition.ToXZ();
                moveAwayFromWallTimer = Random.Range(.5f, 1);
            }

            if (moveAwayTimer > 0)
            {
                moveAwayTimer -= Time.deltaTime;
                return -GetDirectionToPlayer().ToXZ();
            }

            randomMoveDirectionTimer -= Time.deltaTime;
            if (randomMoveDirectionTimer <= 0)
            {
                randomMoveDirectionTimer = Random.value + 1;
                randomMoveDirection = Random.insideUnitCircle;
            }

            return randomMoveDirection;
        }

        float GetMoveSpeed()
        {
            if (buried)
                return 0;

            if (IsNearPlayer())
                return characterSettings.speed.y;

            return characterSettings.speed.x;
        }
    }
}