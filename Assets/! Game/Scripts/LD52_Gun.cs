using UnityEngine;

namespace Prototype
{
    public class LD52_Gun : MonoBehaviour
    {
        [SerializeField] GameObject projectile;

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

            LD52_Global.instance.sounds.bullet.PlayRandomClip();

            var projectile = this.projectile
                .Instantiate(position: player.character.agentPosition + Vector3.up)
                .GetComponent<LD52_Projectile>();

            var shootSpeed = LD52_Global.instance.GetAbility(AbilityType.ShootSpeed).GetValue();
            projectile.speed = 5 + shootSpeed / 5;
            projectile.damage = LD52_Global.instance.GetAbility(AbilityType.ShootDamage).GetValue();
            projectile.direction = (transform.position - player.character.agentPosition).ToXZ();

            shootTimer = 10 / shootSpeed;
        }
    }
}