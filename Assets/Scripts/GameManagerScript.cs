using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject[] blocks = new GameObject[5];

	public static bool gameRunning = false;
	public bool timedMode;
	public const int winScore = 5;
	public static int playerOneWins = 0, playerTwoWins = 0;
	public GameObject winner;

	public event Action startEvent;
	public event Action startGameEvent;
	public event Action endGameEvent;
	public event Action pauseEvent;
	public event Action resumeEvent;

	private GameObject playerOne, playerTwo;
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
			if (playerOne.GetComponent<PlayerController>().m_score >= winScore) {
				winner = playerOne;
				EndGame ();
			}
			else if (playerTwo.GetComponent<PlayerController>().m_score >= winScore) {
				winner = playerTwo;
				EndGame ();
			}
		} else {
			//resume
			if (Input.GetButtonDown (GameManagerScript.PAUSE_BUTTON)) {
				ResumeGame ();
			}
		}
	}

	public void StartGame () {
		gameRunning = true;

		// get map in game
		currentMap = LoadMap(currentMapNumber);
		GenerateLevel(currentMap);

		// get players in game
		playerOne = Instantiate (playerPrefab) as GameObject;
		playerOne.GetComponent<PlayerController> ().m_playerNumber = 1;
		playerTwo = Instantiate (playerPrefab) as GameObject;
		playerTwo.GetComponent<PlayerController> ().m_playerNumber = 2;

		if (startGameEvent != null) startGameEvent();
	}

	public void EndGame() {
		gameRunning = false;
		if (winner != null) {
			if (winner == playerOne) playerOneWins ++;
			else if (winner == playerTwo) playerTwoWins ++;
		}

		//clean up old game
		if (playerOne != null) playerOne.GetComponent<PlayerController>().SafeDestroy();
		if (playerTwo != null) playerTwo.GetComponent<PlayerController>().SafeDestroy();

		foreach (GameObject o in FindObjectsOfType<GameObject>()) {
			if (o.tag == "Solid") Destroy(o);
        }

		respawnPositions.Clear(); //get rid of old respawn points
		if (endGameEvent != null) endGameEvent();
	}

	public void PauseGame() {
		gameRunning = false;
		playerOne.GetComponent<PlayerController>().m_canMove = false;
		playerTwo.GetComponent<PlayerController>().m_canMove = false;
		if (pauseEvent != null) pauseEvent();
	}

	public void ResumeGame() {
		if (resumeEvent != null) resumeEvent();
	}

	public GameObject GetPlayer (int no) {
		return no == 1 ? playerOne : playerTwo;
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
					CreateBlock(i,j,map[i,j]);
				}
			}
		}
	}

	void CreateBlock (int yPos, int xPos, int blockType) { // place a block in the scene
		if (blockType == 1) {
			respawnPositions.Add(GetLevelPos(xPos, -yPos)); 
		} else {
			Instantiate(blocks[blockType - 2], GetLevelPos(xPos, -yPos), Quaternion.identity);
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
