using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject block;
	public Sprite[] blockSprites;
	public Sprite[] redSprites, blueSprites; //alive, dead, flying

	public static bool gameRunning = false;
	public bool timedMode;
	public const int winScore = 5;
	public GameObject winner;

	public event Action startEvent;
	public event Action startGameEvent;
	public event Action finishGameEvent;
	public event Action pauseEvent;
	public event Action resumeEvent;

	GameObject[] players;
	private Vector3 offset = new Vector3(-9.75f,7.25f,0);

	public static List<Vector3> respawnPositions = new List<Vector3>();
	private byte nextRespawnPositionCounter = 0;

	public byte currentMapNumber;
	byte[,] currentMap = new byte[30,40];

	const string PAUSE_BUTTON = "Pause";

	void Start () {
		startEvent();
		StartGame();
	}

	void Update () {
		if (GameManagerScript.gameRunning) {
			//Pause
			if (Input.GetButtonDown (GameManagerScript.PAUSE_BUTTON)) {
				PauseGame ();
			}
			//if any player wins then end the game
			for (int i = 0; i < players.Length; i++) {
				if (players[i].transform.position.y < -13 && players[i].GetComponent<PlayerController>().isAlive) {
					players[i].GetComponent<PlayerController>().Respawn();
				}
				if (players[i].GetComponent<PlayerController>().score >= winScore) {
					winner = players[i];
					EndGame ();
				}
			}
		} else {
			//resume
			if (Input.GetButtonDown (GameManagerScript.PAUSE_BUTTON)) {
				ResumeGame ();
			}
		}
	}

	public void StartGame () {
		players = new GameObject[2];
		gameRunning = true;

		// get map in game
		currentMap = LoadMap(currentMapNumber);
		GenerateLevel(currentMap);

		// get players in game
		for (int i = 0; i < players.Length; i ++) {
			players[i] = Instantiate (playerPrefab) as GameObject;
			string tag = i == 0 ? "Player1" : "Player2";
			string hAx = i == 0 ? "Horizontal1" : "Horizontal2";
			string vAx = i == 0 ? "Vertical1" : "Vertical2";
			string jAx = i == 0 ? "Jump1" : "Jump2";
			string fAx = i == 0 ? "Fire1" : "Fire2";
			Sprite[] s = i == 0 ? redSprites : blueSprites;
			Color colo = i == 0 ? Color.red : Color.blue;
			players[i].GetComponent<PlayerController> ().Setup(tag, hAx, vAx, jAx, fAx, s, colo);
		}

		if (startGameEvent != null) startGameEvent();
	}

	public void EndGame() {
		gameRunning = false;
		for (int i = 0; i < players.Length; i++) {
			if (players[i] != null) players[i].GetComponent<PlayerController>().destroyEvent();
		}

		foreach (GameObject o in FindObjectsOfType<GameObject>()) {
			if (o.tag == "Solid") Destroy(o);
        }
		respawnPositions.Clear(); //get rid of old respawn points
	}

	public void FinishGame() {
		EndGame();
		if (finishGameEvent != null) finishGameEvent();
	}

	public void RestartGame() {
		EndGame();
		StartGame();
	}

	public void PauseGame() {
		gameRunning = false;
		for (int i = 0; i < players.Length; i++) {
			players[0].GetComponent<PlayerController>().canMove = false;
		}
		if (pauseEvent != null) pauseEvent();
	}

	public void ResumeGame() {
		gameRunning = true;
		for (int i = 0; i < players.Length; i++) {
			players[0].GetComponent<PlayerController>().canMove = true;
		}
		if (resumeEvent != null) resumeEvent();
	}

	public GameObject[] GetPlayers () {
		return players;
	}

	byte[,] LoadMap(byte num) { // load a map from text file
		string mapsString = System.IO.File.ReadAllText(@"Assets/maps.txt");
		string mapString = mapsString.Split('-')[num];
		mapString = mapString.Replace("\n","");
		byte[,] map = new byte[40,30];
		int x = 0, y = 0;
		for (int i = 0; i < mapString.Length; i++) {
			map[x,y] = byte.Parse(mapString.Substring(i,1));
			x++;
			if (x >= 40) {
				x = 0;
				y++;
			}
		}
		return map;
	}

	void GenerateLevel (byte[,] map) { // make a new level from a map
		for (int y = 0; y < map.GetLength(1); y ++) {
			for (int x = 0; x < map.GetLength(0); x ++) {
				if (map[x,y] == 1) {
					respawnPositions.Add(GetLevelPos(x, y));
				} else if (map[x,y] == 2) {
					//TODO stop index - 1

					bool left = map[x - 1, y] == 2;
					bool right = map[x + 1, y] == 2;
					bool up = map[x, y - 1] == 2;
					bool down = map[x, y + 1] == 2;

					if (!left && right && down && !up)//top left BLOCK
						CreateBlock(x, y, 0);
					else if (left && right && down && !up)//top mid
						CreateBlock(x, y, 1);
					else if (left && !right && down && !up)//top right
						CreateBlock(x, y, 2);
					else if (!left && right && down && up)//mid left
						CreateBlock(x, y, 3);
					else if (left && right && down && up)//mid mid
						CreateBlock(x, y, 4);
					else if (left && !right && down && up)//mid right
						CreateBlock(x, y, 5);
					else if (!left && right && !down && up)//bot left
						CreateBlock(x, y, 6);
					else if (left && right && !down && up)//bot mid
						CreateBlock(x, y, 7);
					else if (left && !right && !down && up)//bot right
						CreateBlock(x, y, 8);
					else if (!left && !right && down && !up)//top VERT
						CreateBlock(x, y, 9);
					else if (!left && !right && down && up)//mid
						CreateBlock(x, y, 10);
					else if (!left && !right && !down && up)//bot
						CreateBlock(x, y, 11);
					else if (!left && right && !down && !up)//left HORIZ
						CreateBlock(x, y, 12);
					else if (left && right && !down && !up) //mid
						CreateBlock(x, y, 13);
					else if (left && !right && !down && !up)//right
						CreateBlock(x, y, 14);
					else
						CreateBlock(x, y, -1);
				}
			}
		}
	}

	void CreateBlock (int xPos, int yPos, int blockRef) { // place a block in the scene
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
		GameObject newBlock = Instantiate(block, GetLevelPos(xPos, yPos), Quaternion.identity) as GameObject;
		newBlock.GetComponent<SpriteRenderer>().sprite = blockSprites[blockSprite];
		newBlock.GetComponent<SpriteRenderer>().flipX = flippedX;
		newBlock.GetComponent<SpriteRenderer>().flipY = flippedY;
	}

	Vector3 GetLevelPos(int x, int y) { // converts a map pos to world space
		return new Vector3 (x * 0.5f, y * -0.5f, 0) + offset;
	}

	public Vector3 GetNextRespawnPos () { // selects a vector3 from the list
		if (respawnPositions.Count == 0) {
			print("no respawn positons found");
			return Vector3.zero;
		}
		if (nextRespawnPositionCounter >= respawnPositions.Count)
			nextRespawnPositionCounter = 0;
		return respawnPositions[nextRespawnPositionCounter++];
	}
}
