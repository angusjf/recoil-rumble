using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ui
{
	public class SimpleButton : Button
	{
		public SimpleButton (Vector2 position, Texture2D text, Action action)
			: base (position, text, RecoilRumble.Engine.Instance.Content.Load<Texture2D> ("Button-Normal"), RecoilRumble.Engine.Instance.Content.Load<Texture2D> ("Button-Highlighted"), RecoilRumble.Engine.Instance.Content.Load<Texture2D> ("Button-Pressed"), action)
		{ }
	}
}
