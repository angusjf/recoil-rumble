using System;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace RecoilRumble.Game
{
	public class GameRound : IEnumerable<GameObject>
	{
		private List<GameObject> gameObjects;

		private Map map;
		Player player1;//, player2;

		private List<Vector2> respawnPositions;

		public GameRound (Map map)
		{
			gameObjects = new List<GameObject> ();
			this.map = map;
			gameObjects.AddRange (map);
			player1 = new Player (
				Vector2.Zero,
				Microsoft.Xna.Framework.Input.Keys.A,
				Microsoft.Xna.Framework.Input.Keys.D,
				Microsoft.Xna.Framework.Input.Keys.Space,
				Microsoft.Xna.Framework.Input.Keys.LeftShift,
				new PlayerSpriteSet(Engine.Instance.GetTexture ("player_normal_1"))
			);
			gameObjects.Add (player1);
		}

		public IEnumerator<GameObject> GetEnumerator ()
		{
			return ((IEnumerable<GameObject>)gameObjects).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<GameObject>)gameObjects).GetEnumerator ();
		}
	}
}
