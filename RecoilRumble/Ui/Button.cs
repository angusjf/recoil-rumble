using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Ui {

	public class Button : Element
	{
		public Button next = null, previous = null;

		public enum ButtonState { Normal, Highlighted, Pressed }

		private Texture2D normal, text, highlighted, pressed;
		private Action action;

		public Button (Vector2 position, Texture2D text, Texture2D normal, Texture2D highlighted, Texture2D pressed, Action action) : base (position)
		{
			this.text = text;
			this.normal = normal;
			this.highlighted = highlighted;
			this.pressed = pressed;
			this.action = action;
			SetState (ButtonState.Normal);
		}

		public void Press ()
		{
			SetState (ButtonState.Pressed);
			action ();
			SetState (ButtonState.Normal);
		}

		public void SetState (ButtonState state)
		{
			switch (state)
			{
				case ButtonState.Normal:
					sprite = normal;
					break;
				case ButtonState.Highlighted:
					sprite = highlighted;
					break;
				case ButtonState.Pressed:
					sprite = pressed;
					break;
				default:
					break;
			}
		}

		public override void Draw (SpriteBatch spriteBatch)
		{
			if (visible) {
				spriteBatch.Draw (text, position + Vector2.One * 4, scale: Vector2.One * scale, layerDepth: this.layerDepth + 0.1f);
			}
			base.Draw (spriteBatch);
		}
	}

}