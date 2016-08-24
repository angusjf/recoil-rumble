using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject block;
	public Sprite[] blockSprites;

	public static bool gameRunning = false;
	public bool timedMode;
	public const int winScore = 5;
	public GameObject winner;

	public event Action startEvent;
	public event Action startGameEvent;
	public event Action endGameEvent;
	public event Action pauseEvent;
	public event Action resumeEvent;

	public GameObject[] players;
	private Vector3 offset = new Vector3(-9.75f,7.25f,0);

	public static List<Vector3> respawnPositions = new List<Vector3>();
	private int nextRespawnPositionCounter = 0;

	public int currentMapNumber;
	int[,] currentMap = new int[30,40];

	const string PAUSE_BUTTON = "Pause";

	void Start () {
		startEvent();
	}

	void Update () {
		if (GameManagerScript.gameRunning) {
			//Pause
			if (Input.GetButtonDown (GameManagerScript.PAUSE_BUTTON))
				PauseGame ();
			//if any player wins then end the game
			for (int i = 0; i < players.Length; i++) {
				if (players[i].transform.position.y < -30) {
					players[i].GetComponent<PlayerController>().Respawn();
				}
				if (players[i].GetComponent<PlayerController>().m_score >= winScore) {
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
		players = new GameObject[3];
		gameRunning = true;

		// get map in game
		currentMap = LoadMap(currentMapNumber);
		GenerateLevel(currentMap);

		// get players in game
		for (int i = 0; i < players.Length; i ++) {
			players[i] = Instantiate (playerPrefab) as GameObject;
			players[i].GetComponent<PlayerController> ().m_playerNumber = i + 1;
		}

		if (startGameEvent != null) startGameEvent();
	}

	public void EndGame() {
		gameRunning = false;

		//clean up old game
		for (int i = 0; i < players.Length; i++) {
			if (players[i] != null) players[i].GetComponent<PlayerController>().SafeDestroy();
		}

		foreach (GameObject o in FindObjectsOfType<GameObject>()) {
			if (o.tag == "Solid") Destroy(o);
        }

		respawnPositions.Clear(); //get rid of old respawn points
		if (endGameEvent != null) endGameEvent();
	}

	public void PauseGame() {
		gameRunning = false;
		for (int i = 0; i < players.Length; i++) {
			players[0].GetComponent<PlayerController>().m_canMove = false;
		}
		if (pauseEvent != null) pauseEvent();
	}

	public void ResumeGame() {
		if (resumeEvent != null) resumeEvent();
	}

	public GameObject GetPlayer (int no) {
		return players[no];
	}

	int[,] LoadMap(int num) { // load a map from text file
		string mapsString = System.IO.File.ReadAllText(@"Assets/maps.txt");
		string mapString = mapsString.Split('-')[num];
		mapString = mapString.Replace("\n","");
		int x = 0, y = 0;
		int[,] map = new int[30,40];
		for (int i = 0; i < mapString.Length; i++) {
			map[x,y] = int.Parse(mapString.Substring(i,1));
			y++;
			if (y >= 40) {
				y = 0;
				x++;
			}
		}
		return map;
	}

	void GenerateLevel (int[,] map) { // make a new level from a map
		for (int i = 0; i < map.GetLength(0); i ++) {
			for (int j = 0; j < map.GetLength(1); j ++) {
				if (map[i,j] != 0) {
					//alone0, left1, top2, surrounded3
					CreateBlock(i,j,map[i,j]);
				}
			}
		}
	}

	void CreateBlock (int yPos, int xPos, int blockType) { // place a block in the scene
		if (blockType == 1) {
			respawnPositions.Add(GetLevelPos(xPos, -yPos)); 
		} else {
			(Instantiate(block, GetLevelPos(xPos, -yPos), Quaternion.identity) as GameObject).GetComponent<SpriteRenderer>().sprite = blockSprites[blockType - 2];
		}
	}

	Vector3 GetLevelPos(int x, int y) { // converts a map pos to world space
		return new Vector3 (x * 0.5f, y * 0.5f, 0) + offset;
	}

	public Vector3 GetNextRespawnPos () { // selects a vector3 from the list
		if (nextRespawnPositionCounter >= respawnPositions.Count)
			nextRespawnPositionCounter = 0;
		return respawnPositions[nextRespawnPositionCounter++];
	}
}
