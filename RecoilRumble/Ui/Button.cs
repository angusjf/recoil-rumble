using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Ui {

	public class Button : Element
	{

		public Button next = null, previous = null;

		private Texture2D text, normal, highlighted, pressed;
		private Image textObject;
		private Action action;
		private bool visible;

		public override bool Visible {
			get {
				return visible;
			}
			set {
				visible = value;
				textObject.Visible = value;
			}
		}

		public enum ButtonState
		{
			Normal, Highlighted, Pressed
		}

		public Button (Vector2 position, Texture2D text, Texture2D normal, Texture2D highlighted, Texture2D pressed, Action action) : base (position)
		{
			this.text = text;
			this.normal = normal;
			this.highlighted = highlighted;
			this.pressed = pressed;
			this.action = action;
			this.Sprite = normal;
			this.LayerDepth = 0.2f;
			textObject = new Image (this.position + Vector2.UnitX * 4 + Vector2.UnitY * 4, this.text);
		}

		public Button (Vector2 position, Texture2D text, Action action) : this (position, text, RecoilRumble.Engine.Instance.Content.Load<Texture2D> ("Button-Normal"), RecoilRumble.Engine.Instance.Content.Load<Texture2D> ("Button-Highlighted"), RecoilRumble.Engine.Instance.Content.Load<Texture2D> ("Button-Pressed"), action) { }

		/*public override void Show() {
			base.Show();
			SetState(ButtonState.Normal);
			//textObject = new Image("Button Text", typeof(Texture2DRenderer));
			//textObject.transform.SetParent(thisGameObject.transform);
			//textObject.GetComponent<Texture2DRenderer>().sprite = text;
			//textObject.transform.position = textObject.transform.parent.position + Vector3.back;
		}*/

		public void Press ()
		{
			SetState (ButtonState.Pressed);
			action ();
			SetState (ButtonState.Normal);
		}

		/*
		private IEnumerator PressReal() {
				//not possible :'(
		}*/

		public void SetState (ButtonState state)
		{
			switch (state) {
			case ButtonState.Normal:
				Sprite = normal;
				break;
			case ButtonState.Highlighted:
				Sprite = highlighted;
				break;
			case ButtonState.Pressed:
				Sprite = pressed;
				break;
			default:
				break;
			}
		}

		public override void Draw (SpriteBatch spriteBatch)
		{
			if (Visible)
				spriteBatch.Draw (text, position + Vector2.One * 4, scale: Vector2.One * 2, layerDepth: this.LayerDepth + 0.1f);
			base.Draw (spriteBatch);
		}
	}

}