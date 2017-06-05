using System;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace RecoilRumble
{
	public class GameRound : IEnumerable<GameObject>
	{
		private List<GameObject> gameObjects;

		public GameRound ()
		{
			gameObjects = new List<GameObject> ();
			GameObject player = new Player (Vector2.Zero, Engine.Instance.GetTexture("player_normal_1"));
			gameObjects.Add (player);
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
