using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RecoilRumble
{
	public class Player : GameObject, Updatable
	{
		public Player (Vector2 pos, Texture2D spr) : base (pos, spr)
		{
			
		}

		public override void Update ()
		{
			position += Vector2.UnitX;
			base.Update ();
		}
	}
}
