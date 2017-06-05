using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RecoilRumble
{
	public class Engine : Game
	{
		GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;

		public static Engine Instance { get; private set; }

		public List<Updatable> UpdateableGameObjects { get; private set; }
		public List<Drawable> DrawableGameObjects { get; private set; }

		public UiController Ui { get; private set; }
		public InputController Input { get; private set; }

		public Engine ()
		{
			Instance = this;
			UpdateableGameObjects = new List<Updatable>();
			DrawableGameObjects = new List<Drawable>();
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			Ui = new UiController ();
			Input = new InputController ();
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
		}

		protected override void Update (GameTime gameTime)
		{
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape)) Exit ();

			foreach (Updatable obj in UpdateableGameObjects) {
				obj.Update ();
			}

			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);


			spriteBatch.Begin ();
			foreach (Drawable obj in DrawableGameObjects) {
				if (obj.Visible)
					spriteBatch.Draw (obj.Sprite, new Vector2(obj.X, obj.Y));
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
