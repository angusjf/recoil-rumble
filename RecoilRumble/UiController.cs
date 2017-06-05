using System;
using Microsoft.Xna.Framework;
using Ui;

namespace RecoilRumble
{
	public class UiController : Updatable
	{
		public MenuScene CurrentMenu { get; private set; }

		public UiController ()
		{
		}

		public void LoadMainMenu() {

			MenuScene mainMenu = null, optionsMenu = null, creditsMenu = null, levelSelectMenu = null;

			mainMenu = new MenuScene (
				new Image (
					new Vector2 (200, 40), Engine.Instance.GetTexture ("Title-Image")
				),
				new Button (
					new Vector2 (280, 150), Engine.Instance.GetTexture ("Play-Text"), () => {
						LoadMenu (levelSelectMenu);
					}
				),
				new Button (
					new Vector2 (280, 200), Engine.Instance.GetTexture ("Options-Text"), () => {
						LoadMenu (optionsMenu);
					}
				),
				new Button (
					new Vector2 (280, 250), Engine.Instance.GetTexture ("Credits-Text"), () => {
						LoadMenu (creditsMenu);
					}
				),
				new Button (
					new Vector2 (280, 300), Engine.Instance.GetTexture ("Quit-Text"), () => { Engine.Instance.Exit (); }
				)
			);

			optionsMenu = new MenuScene (
				new Image (new Vector2 (280, 2), Engine.Instance.GetTexture ("Options-Image")),
				new Button (
					new Vector2 (280, -3), Engine.Instance.GetTexture ("Back-Text"), () => {
						LoadMenu (mainMenu);
					}
				)
			);

			creditsMenu = new MenuScene (
				new Image (new Vector2 (280, 2), Engine.Instance.GetTexture ("Credits-Image")),
				new Image (new Vector2 (280, -0), Engine.Instance.GetTexture ("Findlang-Twitter-Image")),
				new Image (new Vector2 (280, -1), Engine.Instance.GetTexture ("Rhys-Twitter-Image")),
				new Button (
					new Vector2 (280, -2), Engine.Instance.GetTexture ("Website-Text"), () => {
						//OpenURL("http://findlang.github.io");
					}
				),
				new Button (
					new Vector2 (280, -3), Engine.Instance.GetTexture ("Back-Text"), () => {
						LoadMenu (mainMenu);
					}
				)
			);

			levelSelectMenu = new MenuScene (
				new Image (new Vector2 (180, 6), Engine.Instance.GetTexture ("Choose-Level-Image")),
				new Button (
					new Vector2 (280, 80), Engine.Instance.GetTexture ("Map-1-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						tempStartGame(0);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (280, 160), Engine.Instance.GetTexture ("Map-2-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						tempStartGame(1);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (280, 240), Engine.Instance.GetTexture ("Map-3-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						tempStartGame(2);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (280, 320), Engine.Instance.GetTexture ("Map-4-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						tempStartGame(3);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (280, 400), Engine.Instance.GetTexture ("Back-Text"), () => {
						LoadMenu (mainMenu);
					}
				)
			);

			LoadMenu (mainMenu);
		}

		private void tempStartGame (int i)
		{
			Engine.Instance.GameManager.NewRound(i);
		}

		private void LoadMenu (MenuScene menu)
		{
			if (CurrentMenu != null) CurrentMenu.Hide ();
			CurrentMenu = menu;
			CurrentMenu.Show ();
		}

		private void HideMenu ()
		{
			CurrentMenu.Hide ();
		}

		public void Update ()
		{
			if (Engine.Instance.InputManager.GetKeyDown (Microsoft.Xna.Framework.Input.Keys.Down)) {
				CurrentMenu.NextElement ();
			}
			if (Engine.Instance.InputManager.GetKeyDown (Microsoft.Xna.Framework.Input.Keys.Up)) {
				CurrentMenu.PreviousElement ();
			}
			if (Engine.Instance.InputManager.GetKeyDown (Microsoft.Xna.Framework.Input.Keys.Enter)) {
				CurrentMenu.Press ();
			}
		}
	}
}
