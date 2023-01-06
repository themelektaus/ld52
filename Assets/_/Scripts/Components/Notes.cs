using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Notes))]
    public class Notes : MonoBehaviour
	{
        [SerializeField] string text;
    }
}