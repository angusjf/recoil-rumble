using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ui {

	public class Image : Element {

		public override bool Visible {
			get;
			set;
		}

		public Image (Vector2 position, Texture2D sprite) : base (position) {
			this.Sprite = sprite;
			this.LayerDepth = 0.1f;
		}

	}

}