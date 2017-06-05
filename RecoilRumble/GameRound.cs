using System;
using Microsoft.Xna.Framework;
namespace RecoilRumble
{
	public class GameRound
	{
		public GameRound ()
		{
			GameObject player = new Player (Vector2.Zero, Engine.Instance.GetTexture("player_normal_1"));
		}
	}
}
