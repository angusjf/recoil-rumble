using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {
	#region varaibles
	//game state
	public static bool gameStarted, gameOver, gamePaused;
	public const int winScore = 5;
	public static string lastWinner;
	public static int playerOneWins = 0, playerTwoWins = 0;

	public GameObject playerPrefab;
	public GameObject[] blocks = new GameObject[5];

	private GameObject playerOne, playerTwo;
	private Vector3 offset = new Vector3(-9.75f,7.25f,0);

	public static List<Vector3> respawnPositions = new List<Vector3>();
	private int nextRespawnPositionCounter = 0;

	private MenuController menu;

	public bool timedMode;
	public int currentMapNumber;
	int[,] currentMap = new int[30,40];
	#endregion

	#region constants
	//UI
	public const string CONFIRM_BUTTON = "MenuConfirm";
	public const string CANCEL_BUTTON = "MenuCancel";
	public const string UP_BUTTON = "MenuUp";
	public const string DOWN_BUTTON = "MenuDown";
	//In Game
	public const string PAUSE_BUTTON = "Pause";
	#endregion

	void Awake () {
		menu = GetComponent<MenuController>();
	}

	void Start () {
		menu.OpenMenu ("main");
	}

	void Update () {
		if (gameStarted && !gameOver) {
			//INPUT - PAUSE
			if (Input.GetButtonDown (GameManagerScript.PAUSE_BUTTON)) {
				Pause ();
			}

			if (playerOne.GetComponent<PlayerController>().m_score >= winScore) {
				GameOver(playerOne);
			}
			if (playerTwo.GetComponent<PlayerController>().m_score >= winScore) {
				GameOver(playerTwo);
			}

			if (playerOne.transform.position.y < -50f) {
				playerOne.GetComponent<PlayerController>().Respawn();
			} else if (playerTwo.transform.position.y < -50f) {
				playerTwo.GetComponent<PlayerController>().Respawn();
			}
		}
	}

	public void StartGame () {
		//clean up old game
		if (playerOneWins != 0 || playerTwoWins != 0) {
			//another game
			playerOne.GetComponent<PlayerController>().SafeDestroy();
			playerTwo.GetComponent<PlayerController>().SafeDestroy();

			foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
				if (o.tag == "Solid") {
					Destroy(o);
				}
	        }

		} else {
			//brand new game
		}

		//get map
		currentMap = LoadMap(currentMapNumber);
		GenerateLevel(currentMap);

		// get players in game
		// p1
		playerOne = Instantiate (playerPrefab) as GameObject;
		playerOne.GetComponent<PlayerController> ().m_playerNumber = 1;
		// p2
		playerTwo = Instantiate (playerPrefab) as GameObject;
		playerTwo.GetComponent<PlayerController> ().m_playerNumber = 2;

		//make a new one
		gameStarted = true;
		gameOver = false;
		playerTwo.GetComponent<PlayerController>().Spawn();
		playerOne.GetComponent<PlayerController>().Spawn();
	}

	void Pause () {	
		Debug.Log("game would pause");
		menu.OpenMenu ("pause");
	}

	int[,] LoadMap(int num) {
		//maps held in 40x30 grid, '-' separated
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

	void GenerateLevel (int[,] map) {
		/* C O D E S
		 * 0 = nothing
		 * 1 = respawn pos
		 * 2 = block
		 */
		// making a new level
		for (int i = 0; i < map.GetLength(0); i ++) {
			for (int j = 0; j < map.GetLength(1); j ++) {
				if (map[i,j] != 0) {
					CreateBlock(i,j,map[i,j]);
				}
			}
		}
	}

	void CreateBlock (int yPos, int xPos, int blockType) {
		if (blockType == 1) {
			respawnPositions.Add(getLevelPos(xPos, -yPos)); 
		} else {
			Instantiate(blocks[blockType - 2], getLevelPos(xPos, -yPos), Quaternion.identity);
		}
	}

	void ToggleMuted () { //TODO implement
		playerOne.GetComponent<AudioSource>().mute = !playerOne.GetComponent<AudioSource>().mute;
		playerTwo.GetComponent<AudioSource>().mute = !playerTwo.GetComponent<AudioSource>().mute;
	}

	public void GameOver (GameObject winner) {
		gameOver = true;
		gameStarted = false;
		lastWinner = winner.tag;
		if (winner == playerOne) playerOneWins ++;
		else if (winner == playerTwo) playerTwoWins ++;
		menu.OpenMenu("end");
	}

	public GameObject GetPlayer (int no) {
		return no == 1 ? playerOne : playerTwo;
	}

	public Vector3 GetRandomRespawnPos () {
		if (nextRespawnPositionCounter >= respawnPositions.Count)
			nextRespawnPositionCounter = 0;
		return respawnPositions[nextRespawnPositionCounter++];
	}

	private Vector3 getLevelPos(int x, int y) {
		return new Vector3 (x * 0.5f, y * 0.5f, 0) + offset;
	}
}
