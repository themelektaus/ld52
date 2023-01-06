using UnityEngine;

namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		public static LD52_GameStateMachine instance;

		public static class States
        {
			public const string Intro = "Intro";
			public const string Title = "Title";
			public const string MainMenu = "Main Menu";
			public const string Settings = "Settings";
			public const string Ingame = "Ingame";
			public const string Pause = "Pause";
			public const string GameOver = "Game Over";
			public const string Credits = "Credits";
			public const string Exit = "Exit";
		}

		public static class Tiggers
        {
			public const string Next = "Next";
			public const string Back = "Back";
			public const string Settings = "Settings";
			public const string Ingame = "Ingame";
			public const string Pause = "Pause";
			public const string GameOver = "Game Over";
			public const string Credits = "Credits";
			public const string Exit = "Exit";
		}

		public bool IsIngame() => animator.GetBool("Is Ingame");
		public void EnableIngame() => animator.SetBool("Is Ingame", true);
		public void DisableIngame() => animator.SetBool("Is Ingame", false);

		public RectTransform mainCanvas;
		public BlurOverride blurOverride;

		[SerializeField] GameObject intro;
		[SerializeField] GameObject introUI;
		[SerializeField] GameObject title;
		[SerializeField] GameObject titleUI;
		[SerializeField] GameObject mainMenu;
		[SerializeField] GameObject mainMenuUI;
		[SerializeField] GameObject settings;
		[SerializeField] GameObject settingsUI;
		[SerializeField] GameObject ingame;
		[SerializeField] GameObject ingameUI;
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

		protected override void OnAwake()
        {
			instance = this;
			gameStateInstances = new(this);

			LD52_GlobalAlpha.SetValue(0, 1);
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