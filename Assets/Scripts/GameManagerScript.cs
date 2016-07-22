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

	private MenuController menu;

	/* C O D E S
	 * 0 = nothing
	 * 1 = respawn pos
	 * 2 = block
	 */
	#region map
	int[,] levelMap = new int[30,40] {
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,1,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };
	#endregion
	#endregion

	#region constants
	public const KeyCode CONFIRM_KEY = KeyCode.Return;
	public const KeyCode CANCEL_KEY = KeyCode.Backspace;
	public const KeyCode UP_KEY = KeyCode.UpArrow;
	public const KeyCode DOWN_KEY = KeyCode.DownArrow;
	public const KeyCode PAUSE_KEY = KeyCode.P;
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
			if (Input.GetKey (GameManagerScript.PAUSE_KEY)) {
				Pause ();
			}

			if (playerOne.GetComponent<PlayerController>().m_score >= winScore) {
				GameOver(playerOne);
			}
			if (playerTwo.GetComponent<PlayerController>().m_score >= winScore) {
				GameOver(playerTwo);
			}

			if (Input.GetKeyDown(KeyCode.M)) ToggleMuted();
			
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

		GenerateLevel(); //HACK move down

		// get players in game
		// p1
		playerOne = Instantiate (playerPrefab) as GameObject;
		playerOne.GetComponent<PlayerController> ().m_playerNumber = 1;
		playerOne.GetComponent<PlayerController>().Respawn();
		// p2
		playerTwo = Instantiate (playerPrefab) as GameObject;
		playerTwo.GetComponent<PlayerController> ().m_playerNumber = 2;
		playerTwo.GetComponent<PlayerController>().Respawn();

		//make a new one
		gameStarted = true;
		gameOver = false;
		playerOne.SetActive(true);
		playerTwo.SetActive(true);
	}

	void Pause () {	
		Debug.Log("game would pause");
		menu.OpenMenu ("pause");
	}

	void GenerateLevel () {
		// making a new level
		for (int i = 0; i < levelMap.GetLength(0); i ++) {
			for (int j = 0; j < levelMap.GetLength(1); j ++) {
				if (levelMap[i,j] != 0) {
					CreateBlock(i,j,levelMap[i,j]);
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

	void ToggleMuted () {
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
		return respawnPositions[Random.Range(0, respawnPositions.Count)];//TODO
	}

	private Vector3 getLevelPos(int x, int y) {
		return new Vector3 (x * 0.5f, y * 0.5f, 0) + offset;
	}
}
