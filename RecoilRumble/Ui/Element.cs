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

		protected Vector2 position;
		public Texture2D Sprite { get; set; }
		public Drawable Parent = null;
		public bool Visible { get; set; }

		public Element (Vector2 position){
			this.position = position;
			Visible = false;
			//Sprite = Engine.Instance.GetTexture ("Slice-2");
			Engine.Instance.DrawableGameObjects.Add (this);
		}
	}
}
