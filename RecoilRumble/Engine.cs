using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilRumble
{
	public class Engine : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;

		public static Engine Instance { get; private set; }

		public UiController UiManager { get; private set; }
		public InputController InputManager { get; private set; }
		public GameManager GameManager { get; private set; }

		public Engine ()
		{
			Instance = this;
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			Window.Title = "Recoil Rumble";
			IsMouseVisible = false;
			graphics.PreferredBackBufferWidth = 640 * 1;
			graphics.PreferredBackBufferHeight = 480 * 1;
		}

		protected override void Initialize ()
		{
			UiManager = new UiController ();
			InputManager = new InputController ();
			GameManager = new GameManager ();

			UiManager.LoadMainMenu ();

			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
		}

		protected override void Update (GameTime gameTime)
		{
			if (GameManager.CurrentRound != null)
			{
				foreach (Updatable obj in GameManager.CurrentRound)
				{
					obj.Update ();
				}
			}

			UiManager.Update ();
			InputManager.Update ();
			GameManager.Update ();

			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.Black);

			spriteBatch.Begin (SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, null);

			if (UiManager.CurrentMenu != null)
			{
				foreach (Drawable obj in UiManager.CurrentMenu)
				{
					obj.Draw (spriteBatch);
				}
			}

			if (GameManager.CurrentRound != null)
			{
				foreach (Drawable obj in GameManager.CurrentRound)
				{
					obj.Draw (spriteBatch);
				}
			}

			spriteBatch.End ();

			base.Draw (gameTime);
		}

		public Texture2D GetTexture (string name)
		{
			return Content.Load<Texture2D> (name);
		}
	}
}
