using System;
using Microsoft.Xna.Framework.Graphics;
namespace RecoilRumble.Game
{
	public class AnimationController
	{
		private Texture2D sprite;
		private PlayerSpriteSet spriteSet;
		private bool facingLeft = true;
		public bool FacingLeft {
			set {
				facingLeft = value;
				//change dir if need be
			}
		}

		public AnimationController (Texture2D sprite, PlayerSpriteSet spriteSet)
		{
			this.sprite = sprite;
			this.spriteSet = spriteSet;
		}
	}
}
