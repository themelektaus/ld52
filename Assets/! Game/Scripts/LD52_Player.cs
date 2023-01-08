using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class LD52_Player : MonoBehaviour, IObserver<LD52_Altar.WantItemMessage>
	{
        public LD52_Character character;

        [SerializeField] Transform cameraTarget;

        public List<LD52_EnemyItem> items;

        Vector3 cameraTargetOffset;
        SmoothVector3 cameraTargetPosition;

        void Awake()
        {
            LD52_Global.instance.playerQuery.ClearCache();

            character.getMoveDirection = () => LD52_Global.GetInputAxis();
            character.getMoveSpeed = () => LD52_Global.instance.upgrades.moveSpeed.GetCurrent();
            
            character.onUpdate += Character_onUpdate;
            character.onUpdateDirection += Character_onUpdateDirection;

            cameraTargetOffset = cameraTarget.localPosition;
            cameraTargetPosition = new(new(), .2f);
        }

        void OnEnable()
        {
            LD52_Altar.wantItemSubject.Register(this, x => x.reciever == character.collider);
        }

        void OnDisable()
        {
            LD52_Altar.wantItemSubject.Unregister(this);
        }

        public void ReceiveNotification(LD52_Altar.WantItemMessage message)
        {
            if (items.Count == 0)
                return;

            message.sender.Feed();

            var item = items[0];
            items.RemoveAt(0);
            LD52_Global.instance.altarItems.Add(item);
        }

        void Character_onUpdate()
        {
            cameraTarget.position = character.agentPosition + cameraTargetOffset + cameraTargetPosition;
        }

        void Character_onUpdateDirection(Vector3 direction)
        {
            cameraTargetPosition.target = direction * 1.5f;
            cameraTargetPosition.Update();
        }

        public bool Add(LD52_EnemyItem item)
        {
            if (items.Count >= LD52_Global.instance.upgrades.carryingCapacity.GetCurrent())
                return false;

            items.Add(item);
            return true;
        }
    }
}