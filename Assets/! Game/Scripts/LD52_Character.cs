using System;
using UnityEngine;
using UnityEngine.AI;

namespace Prototype
{
	public class LD52_Character : MonoBehaviour
	{
        public NavMeshAgent agent;
        public Vector3 agentPosition => agent.transform.position;

        public new Collider collider;
        public Animator animator;

        public Func<Vector2> getMoveDirection;
        public Func<float> getMoveSpeed;
        public Func<LD52_Global.CharacterSettings> getCharacterSettings;

        public event Action onUpdate;
        public event Action<Vector3> onUpdateDirection;
        public event Action onDie;
        public event Action onHarvest;
        public event Action onDigOut;

        public float health;
        public float harvestLife;
        public float buried;

        LD52_Global.CharacterSettings _settings;
        public LD52_Global.CharacterSettings settings
            => _settings ??= getCharacterSettings?.Invoke() ?? new();

        readonly SmoothQuaternion rotation = new(new(), .1f);

        void OnEnable()
        {
            agent.enabled = true;
        }

        void OnDisable()
        {
            agent.enabled = false;
        }

        void Start()
        {
            health = settings.maxHealth;
            harvestLife = settings.maxHarvestLife;
            buried = settings.maxBuried;
        }

        void Update()
        {
            if (LD52_GameStateMachine.instance.IsIngamePaused())
                return;

            var direction = getMoveDirection?.Invoke().ToX0Z().normalized ?? new();
            
            if (Utils.Approximately(direction, new()))
            {
                animator.SetFloat("Move", 0);
            }
            else
            {
                animator.SetFloat("Move", 1);
                rotation.target = Quaternion.LookRotation(direction);
                onUpdateDirection?.Invoke(direction);
            }

            onUpdate?.Invoke();

            direction *= getMoveSpeed?.Invoke() ?? 1;
            direction *= Time.deltaTime;

            if (agent.enabled)
                agent.Move(direction);

            rotation.Update();
            agent.transform.rotation = rotation;
        }

        public void TakeDamage(float damage)
        {
            if (TryGetComponent(out LD52_HitObject hitObject))
                hitObject.hitWeight = 1;

            health = Mathf.Max(0, health - damage);
            if (health == 0)
            {
                LD52_Global.instance.PlayDeathSound();
                animator.SetTrigger("Die");
                onDie?.Invoke();
            }
            else
            {
                LD52_Global.instance.PlayHitSound();
            }
        }

        public void Harvest(float value)
        {
            harvestLife = Mathf.Max(0, harvestLife - value);
            if (harvestLife == 0)
            {
                LD52_Global.instance.PlayHarvestSound();
                //animator.SetTrigger("Harvest");
                onHarvest?.Invoke();
            }
        }

        public void DigOut(float value)
        {
            buried = Mathf.Max(0, buried - value);
            if (buried == 0)
            {
                //animator.SetTrigger("Dig Out");
                onDigOut?.Invoke();
            }
        }
    }
}