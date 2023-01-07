using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class LD52_Player : MonoBehaviour
	{
        public LD52_Character character;

        [SerializeField] Transform cameraTarget;

        [SerializeField] List<LD52_EnemyItem> items;

        Vector3 cameraTargetOffset;
        SmoothVector3 cameraTargetPosition;

        void Awake()
        {
            character.playerQuery.ClearCache();

            character.getMoveDirection = () => LD52_Global.GetInputAxis();
            character.getCharacterSettings = () => LD52_Global.instance.playerSettings;

            character.onUpdate += Character_onUpdate;
            character.onUpdateDirection += Character_onUpdateDirection;

            cameraTargetOffset = cameraTarget.localPosition.ToXZ().ToX0Z();
            cameraTargetPosition = new(new(), .2f);
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
            if (items.Count >= LD52_Global.instance.playerSettings.itemCapacity)
                return false;

            items.Add(item);
            return true;
        }
    }
}