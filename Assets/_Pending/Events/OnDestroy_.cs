using UnityEngine;

namespace Prototype.Pending
{
	[AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Destroy")]
	public class OnDestroy_ : On
	{
		protected override bool offloadCoroutine => true;

		void OnDestroy()
		{
			Invoke();
		}
	}
}