namespace Prototype
{
    [UnityEngine.CreateAssetMenu]
	public class LD52_Global : UnityEngine.ScriptableObject
	{
		public void GameStateMachineTrigger(string name)
        {
			LD52_GameStateMachine.instance.Trigger(name);
        }

		public void PlaySound(SoundEffectCollection soundEffect)
        {
			soundEffect.PlayRandomClip();
        }
	}
}