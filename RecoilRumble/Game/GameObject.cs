using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RecoilRumble.Game
{
	public abstract class GameObject : Drawable, Updatable
	{
		public Vector2 position;
		private const int scale = 1;
		protected Texture2D sprite;
		public bool visible = true;
		public bool FlipX {
			set {
				if (value == ((spriteEffects | SpriteEffects.FlipVertically) == SpriteEffects.None)) {
					spriteEffects = spriteEffects | SpriteEffects.FlipHorizontally;
				}
			}
		}
		public bool FlipY {
			set {
				if (value == ((spriteEffects | SpriteEffects.FlipVertically) == SpriteEffects.None)) {
					spriteEffects |= SpriteEffects.FlipVertically;
				}
			}
		}
		private SpriteEffects spriteEffects;

		protected GameObject (Vector2 position, Texture2D sprite)
		{
			this.position = position;
			this.sprite	= sprite;
		}

		public virtual void Update()
		{
			
		}

		public virtual void Draw (SpriteBatch spriteBatch)
		{
			if (visible) {
				spriteBatch.Draw (
					sprite,
					position,
					scale: Vector2.One * scale,
					effects: spriteEffects
				);
			}
		}
	}
}
