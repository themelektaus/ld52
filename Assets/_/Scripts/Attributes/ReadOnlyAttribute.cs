using UnityEngine;

namespace Prototype
{
	public class ReadOnlyAttribute : PropertyAttribute
	{
		public bool duringEditMode = true;
		public bool duringPlayMode = true;
	}
}