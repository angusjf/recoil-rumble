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

		public Vector2 position;
		public Texture2D Sprite { get; set; }
		public GameObject Parent = null;
		public bool Visible { get; set; }

		public GameObject ()
		{
			Engine.Instance.UpdateableGameObjects.Add (this);
			Engine.Instance.DrawableGameObjects.Add (this);
		}

		public GameObject (Vector2 position) : this ()
		{
			this.position = position;
		}

		public GameObject (Vector2 position, Texture2D sprite) : this (position)
		{
			this.Sprite	= sprite;
		}

		public void Update() {
			position += Vector2.UnitY;
		}
	}
}
