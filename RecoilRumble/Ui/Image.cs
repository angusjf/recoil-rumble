using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ui {

	public class Image : Element {

		public Image (Vector2 position, Texture2D sprite) : base (position) {
			this.Sprite = sprite;
		}

	}

}