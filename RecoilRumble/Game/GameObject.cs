using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RecoilRumble
{
	public abstract class GameObject : Drawable, Updatable
	{
		public int X {
			set {
				position.X = value;
			}
			get {
				return (int)position.X;
			}
		}
		public int Y {
			set {
				position.Y = value;
			}
			get {
				return (int)position.Y;
			}
		}

		public int Scale { get { return 1; } }

		public int Layer { get; private set; }

		public Vector2 position;
		public Texture2D Sprite { get; set; }
		public GameObject Parent = null;
		public bool Visible { get; set; }

		public GameObject ()
		{
		}

		public GameObject (Vector2 position) : this ()
		{
			this.position = position;
		}

		public GameObject (Vector2 position, Texture2D sprite) : this (position)
		{
			this.Sprite	= sprite;
		}

		public virtual void Update()
		{
			position += Vector2.UnitY;
		}

		public virtual void Draw (SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (Sprite, position, null, null, null, 0, Vector2.One * Scale);
		}

	}
}
