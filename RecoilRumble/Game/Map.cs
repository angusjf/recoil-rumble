using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RecoilRumble.Game
{
	public class Map : IEnumerable<GameObject>
	{
		public byte [,] data;
		private List<Vector2> respawnPositions;
		private List<GameObject> gameObjects;
		private Texture2D [] blockSprites;

		private Map (byte[,] data)
		{
			this.data = data;
			respawnPositions = new List<Vector2> ();
			blockSprites = new Texture2D [8];
			for (int i = 0; i < blockSprites.Length; i++)
				blockSprites [i] = Engine.Instance.GetTexture ("block_sprites-" + i);
		}

		public static Map LoadMap (int num)
		{ // load a map from text file
			string mapsString = System.IO.File.ReadAllText (@"Content/maps.txt");
			string mapString = mapsString.Split ('-') [num];
			mapString = mapString.Replace ("\n", "");
			byte [,] map = new byte [40, 30];
			int x = 0, y = 0;
			for (int i = 0; i < mapString.Length; i++) {
				map [x, y] = byte.Parse (mapString.Substring (i, 1));
				x++;
				if (x >= 40) {
					x = 0;
					y++;
				}
			}
			return new Map (map);
		}

		public IEnumerator<GameObject> GetEnumerator ()
		{
			if (gameObjects == null) {
				gameObjects = new List<GameObject> ();
				GenerateLevel ();
			}
			return gameObjects.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}

		private void GenerateLevel ()
		{
			// make a new level from a map
			for (int y = 0; y < data.GetLength (1); y++) {
				for (int x = 0; x < data.GetLength (0); x++) {
					if (data [x, y] == 1) {
						respawnPositions.Add (GetLevelPos (x, y));
					} else if (data [x, y] == 2) {
						//TODO stop index - 1

						bool left = data [x - 1, y] == 2;
						bool right = data [x + 1, y] == 2;
						bool up = data [x, y - 1] == 2;
						bool down = data [x, y + 1] == 2;

						if (!left && right && down && !up)//top left BLOCK
							CreateBlock (x, y, 0);
						else if (left && right && down && !up)//top mid
							CreateBlock (x, y, 1);
						else if (left && !right && down && !up)//top right
							CreateBlock (x, y, 2);
						else if (!left && right && down && up)//mid left
							CreateBlock (x, y, 3);
						else if (left && right && down && up)//mid mid
							CreateBlock (x, y, 4);
						else if (left && !right && down && up)//mid right
							CreateBlock (x, y, 5);
						else if (!left && right && !down && up)//bot left
							CreateBlock (x, y, 6);
						else if (left && right && !down && up)//bot mid
							CreateBlock (x, y, 7);
						else if (left && !right && !down && up)//bot right
							CreateBlock (x, y, 8);
						else if (!left && !right && down && !up)//top VERT
							CreateBlock (x, y, 9);
						else if (!left && !right && down && up)//mid
							CreateBlock (x, y, 10);
						else if (!left && !right && !down && up)//bot
							CreateBlock (x, y, 11);
						else if (!left && right && !down && !up)//left HORIZ
							CreateBlock (x, y, 12);
						else if (left && right && !down && !up) //mid
							CreateBlock (x, y, 13);
						else if (left && !right && !down && !up)//right
							CreateBlock (x, y, 14);
						else
							CreateBlock (x, y, -1);
					}
				}
			}
		}

		void CreateBlock (int xPos, int yPos, int blockRef)
		{ // place a block in the scene
			int blockSprite = 0;
			bool flippedX = false, flippedY = false;
			switch (blockRef) {
			case 0:
				blockSprite = 0;
				break;
			case 1:
				blockSprite = 1;
				break;
			case 2:
				blockSprite = 0;
				flippedX = true;
				break;
			case 3:
				blockSprite = 3;
				break;
			case 4:
				blockSprite = 4;
				break;
			case 5:
				blockSprite = 3;
				flippedX = true;
				break;
			case 6:
				blockSprite = 0;
				flippedY = true;
				break;
			case 7:
				blockSprite = 1;
				flippedY = true;
				break;
			case 8:
				blockSprite = 0;
				flippedX = true;
				flippedY = true;
				break;
			case 9: //vertical 
				blockSprite = 2;
				break;
			case 10:
				blockSprite = 5;
				break;
			case 11:
				blockSprite = 2;
				flippedY = true;
				break;
			case 12: // horizontal
				blockSprite = 6;
				break;
			case 13:
				blockSprite = 7;
				break;
			case 14:
				blockSprite = 6;
				flippedX = true;
				break;
			default:
				break;
			}
			Block newBlock = new Block (GetLevelPos (xPos, yPos), blockSprites [blockSprite]);
			newBlock.FlipX = !flippedX;
			newBlock.FlipY = !flippedY;
			gameObjects.Add (newBlock);
		}

		private Vector2 GetLevelPos (int x, int y)
		{
			// converts a map pos to world space
			return new Vector2 (x * 16f, y * 16f);// + new Vector2 (-9.75f, 7.25f);;
		}


		int e = 0;
		public Vector2 NextRespawnPosition ()
		{
			//todo better implementation
			e++;
			return respawnPositions[e % respawnPositions.Count];
		}
	}
}
