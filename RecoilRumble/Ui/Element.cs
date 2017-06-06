using RecoilRumble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ui {

	public abstract class Element : Drawable {

		public bool visible;

		protected Vector2 position;
		protected Texture2D sprite;
		protected float layerDepth = 0;
		protected const int scale = 2;

		protected Element (Vector2 position){
			this.position = position;
		}

		public virtual void Draw (SpriteBatch spriteBatch)
		{
			if (visible) {
				spriteBatch.Draw (sprite, position, scale: Vector2.One * scale, layerDepth: layerDepth);
			}
		}
	}
}
