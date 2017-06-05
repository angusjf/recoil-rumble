using RecoilRumble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ui {

	public abstract class Element : Drawable {

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

		public float LayerDepth { get; protected set; }
		public int Scale { get { return 2; } }
		public Texture2D Sprite { get; set; }
		public abstract bool Visible { get; set; }

		protected Vector2 position;

		public Element (Vector2 position){
			this.position = position;
		}

		public virtual void Draw (SpriteBatch spriteBatch)
		{
			if (Visible)
				spriteBatch.Draw (Sprite, position, scale: Vector2.One * Scale, layerDepth: LayerDepth);
		}
	}
}
