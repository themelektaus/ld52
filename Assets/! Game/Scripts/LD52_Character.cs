using System;
using UnityEngine;
using UnityEngine.AI;

namespace Prototype
{
	public class LD52_Character : MonoBehaviour
	{
        public NavMeshAgent agent;
        public Vector3 agentPosition => agent.transform.position;

        public ObjectQuery playerQuery;

        public new Collider collider;
        public Animator animator;

        public Func<Vector2> getMoveDirection;
        public Func<LD52_Global.CharacterSettings> getCharacterSettings;

        public event Action onUpdate;
        public event Action<Vector3> onUpdateDirection;
        public event Action onDie;
        public event Action onHarvest;

        float health;
        float harvestLife;

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
            health = getCharacterSettings().maxHealth;
            harvestLife = getCharacterSettings().maxHarvestLife;
        }

        void Update()
        {
            if (LD52_GameStateMachine.instance.IsIngamePaused())
                return;

            var direction = getMoveDirection().ToX0Z().normalized;
            
            if (!Utils.Approximately(direction, new()))
            {
                agent.transform.rotation = Quaternion.LookRotation(direction);
                onUpdateDirection?.Invoke(direction);
            }

            onUpdate?.Invoke();

            direction *= getCharacterSettings().speed;
            direction *= Time.deltaTime;

            agent.Move(direction);
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(0, health - damage);
            if (health == 0)
            {
                animator.SetTrigger("Die");
                onDie?.Invoke();
            }
        }

        public void Harvest(float value)
        {
            harvestLife = Mathf.Max(0, harvestLife - value);
            if (harvestLife == 0)
            {
                //animator.SetTrigger("Harvest");
                onHarvest?.Invoke();
            }
        }
    }
}