using System;
using Microsoft.Xna.Framework;
using Ui;

namespace RecoilRumble
{
	public class UiController : Updatable
	{
		private MenuScene currentMenu;

		public UiController ()
		{
			LoadMainMenu ();
			Engine.Instance.UpdateableGameObjects.Add (this);
		}

		private void LoadMainMenu() {

			MenuScene mainMenu = null, optionsMenu = null, creditsMenu = null, levelSelectMenu = null;

			mainMenu = new MenuScene (
				new Image (
					new Vector2 (0, 0), Engine.Instance.GetTexture ("Title-Image")
				),
				new Button (
					new Vector2 (0, 50), Engine.Instance.GetTexture ("Play-Text"), () => {
						LoadMenu (levelSelectMenu);
					}
				),
				new Button (
					new Vector2 (0, 65), Engine.Instance.GetTexture ("Options-Text"), () => {
						LoadMenu (optionsMenu);
					}
				),
				new Button (
					new Vector2 (0, 80), Engine.Instance.GetTexture ("Credits-Text"), () => {
						LoadMenu (creditsMenu);
					}
				),
				new Button (
					new Vector2 (0, 95), Engine.Instance.GetTexture ("Quit-Text"), () => { Engine.Instance.Exit (); }
				)
			);

			optionsMenu = new MenuScene (
				new Image (new Vector2 (0, 2), Engine.Instance.GetTexture ("Options-Image")),
				new Button (
					new Vector2 (0, -3), Engine.Instance.GetTexture ("Back-Text"), () => {
						LoadMenu (mainMenu);
					}
				)
			);

			creditsMenu = new MenuScene (
				new Image (new Vector2 (0, 2), Engine.Instance.GetTexture ("Credits-Image")),
				new Image (new Vector2 (0, -0), Engine.Instance.GetTexture ("Findlang-Twitter-Image")),
				new Image (new Vector2 (0, -1), Engine.Instance.GetTexture ("Rhys-Twitter-Image")),
				new Button (
					new Vector2 (0, -2), Engine.Instance.GetTexture ("Website-Text"), () => {
						//OpenURL("http://findlang.github.io");
					}
				),
				new Button (
					new Vector2 (0, -3), Engine.Instance.GetTexture ("Back-Text"), () => {
						LoadMenu (mainMenu);
					}
				)
			);

			levelSelectMenu = new MenuScene (
				new Image (new Vector2 (0, 6), Engine.Instance.GetTexture ("Choose-Level-Image")),
				new Button (
					new Vector2 (0, 4), Engine.Instance.GetTexture ("Map-1-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						//this.gameObject.GetComponent<GameManagerScript>().StartGame(0);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (0, 1), Engine.Instance.GetTexture ("Map-2-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						//this.gameObject.GetComponent<GameManagerScript>().StartGame(1);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (0, -2), Engine.Instance.GetTexture ("Map-3-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						//this.gameObject.GetComponent<GameManagerScript>().StartGame(2);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (0, -5), Engine.Instance.GetTexture ("Map-4-Image"),
					Engine.Instance.GetTexture ("Map-Button-Normal"), Engine.Instance.GetTexture ("Map-Button-Highlighted"),
					Engine.Instance.GetTexture ("Map-Button-Pressed"), () => {
						//this.gameObject.GetComponent<GameManagerScript>().StartGame(3);
						HideMenu ();
					}
				),
				new Button (
					new Vector2 (0, -7), Engine.Instance.GetTexture ("Back-Text"), () => {
						LoadMenu (mainMenu);
					}
				)
			);

			LoadMenu (mainMenu);
		}

		private void LoadMenu (MenuScene menu)
		{
			if (currentMenu != null) currentMenu.Hide ();
			currentMenu = menu;
			currentMenu.Show ();
		}

		private void HideMenu ()
		{
			currentMenu.Hide ();
		}

		public void Update ()
		{
			if (Engine.Instance.Input.GetKeyDown (Microsoft.Xna.Framework.Input.Keys.Down)) {
				currentMenu.NextElement ();
			}
			if (Engine.Instance.Input.GetKeyDown (Microsoft.Xna.Framework.Input.Keys.Up)) {
				currentMenu.PreviousElement ();
			}
			if (Engine.Instance.Input.GetKeyDown (Microsoft.Xna.Framework.Input.Keys.Enter)) {
				currentMenu.Press ();
			}
		}
	}
}
