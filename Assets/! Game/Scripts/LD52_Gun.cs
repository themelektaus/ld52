using UnityEngine;

namespace Prototype
{
    public class LD52_Gun : MonoBehaviour
    {
        [SerializeField] GameObject projectile;

        [SerializeField] float shootInterval = .2f;
        
        float shootTimer;

        void Update()
        {
            if (LD52_GameStateMachine.instance.IsIngamePaused())
                return;

            var hit = LD52_Global.GetMouseGroundHit();
            if (hit.HasValue)
                transform.position = hit.Value.point;

            shootTimer = Mathf.Max(0, shootTimer - Time.deltaTime);
            if (shootTimer > 0)
                return;

            if (!LD52_Global.GetInputShoot())
                return;

            var player = LD52_Global.instance.GetPlayer();
            if (!player)
                return;

            projectile
                .Instantiate(position: player.character.agentPosition + Vector3.up)
                .GetComponent<LD52_Projectile>()
                .direction = (transform.position - player.character.agentPosition).ToXZ();

            shootTimer = shootInterval;
        }
    }
}