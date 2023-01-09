using UnityEngine;

namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		public static LD52_GameStateMachine instance;

		public static class States
        {
			public const string Intro = "Intro";
			public const string MainMenu = "Main Menu";
			public const string Settings = "Settings";
			public const string Ingame = "Ingame";
			public const string EndOfWave = "End of Wave";
			public const string UpgradeMenu = "Upgrade Menu";
			public const string Pause = "Pause";
			public const string GameOver = "Game Over";
			public const string Credits = "Credits";
			public const string Exit = "Exit";
		}

		public static class Triggers
        {
			public const string Next = "Next";
			public const string Back = "Back";
			public const string Settings = "Settings";
			public const string Ingame = "Ingame";
			public const string EndOfWave = "End of Wave";
			public const string Pause = "Pause";
			public const string GameOver = "Game Over";
			public const string Credits = "Credits";
			public const string Exit = "Exit";
		}

		public bool IsIngame() => animator.GetBool("Is Ingame");
		public bool IsIngamePaused() => !HasState(States.Ingame);
		public void EnableIngame() => animator.SetBool("Is Ingame", true);
		public void DisableIngame() => animator.SetBool("Is Ingame", false);

		public RectTransform mainCanvas;
		public BlurOverride blurOverride;

		[SerializeField] GameObject intro;
		[SerializeField] GameObject introUI;
		[SerializeField] GameObject mainMenu;
		[SerializeField] GameObject mainMenuUI;
		[SerializeField] GameObject settings;
		[SerializeField] GameObject settingsUI;
		[SerializeField] GameObject ingame;
		[SerializeField] GameObject ingameUI;
		[SerializeField] GameObject endOfWave;
		[SerializeField] GameObject endOfWaveUI;
		[SerializeField] GameObject upgradeMenu;
		[SerializeField] GameObject upgradeMenuUI;
		[SerializeField] GameObject pause;
		[SerializeField] GameObject pauseUI;
		[SerializeField] GameObject gameOver;
		[SerializeField] GameObject gameOverUI;
		[SerializeField] GameObject credits;
		[SerializeField] GameObject creditsUI;
		[SerializeField] GameObject exit;
		[SerializeField] GameObject exitUI;

		LD52_GameStateInstances gameStateInstances;

		SoundEffectInstance music;

		LD52_Global global => LD52_Global.instance;
		LD52_Global.Wave wave => global.wave;

		protected override void OnAwake()
        {
			instance = this;
			gameStateInstances = new(this);
		}

		void ChangeMusic(SoundEffectCollection music, float volume)
		{
			if (!this.music && !music)
				return;

			if (this.music && music && this.music.clip == music.GetClip())
				return;

			if (this.music)
				this.music.Destroy();

			this.music = music ? music.PlayClip(new()
			{
				volume = volume,
				loop = true,
				fade = new(2, 2)
			}) : null;
		}
	}
}