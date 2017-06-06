using System;
using Microsoft.Xna.Framework.Graphics;

namespace RecoilRumble
{
	public class PlayerSpriteSet
	{
		public Texture2D
		idleTexture,
		walkingTexture;

		public PlayerSpriteSet (Texture2D idleTexture)
		{
			this.idleTexture = idleTexture;
		}
	}
}
