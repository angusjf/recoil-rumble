using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

	//game state
	public static bool gameStarted, gameOver;
	public const int winScore = 2;
	public static string lastWinner;
	public static int playerOneWins = 0, playerTwoWins = 0;

	public GameObject playerPrefab;
	public GameObject[] blocks = new GameObject[5];

	private GameObject playerOne, playerTwo;
	private Vector3 offset = new Vector3(-9.75f,7.25f,0);

	private MenuController menu;
	
	/* C O D E S
	 * 0 = nothing
	 * 1 = player one
	 * 2 = player two
	 * 3 = grass block
	 */

	int[,] levelMap = new int[30,40] {
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
	    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };

	void Awake () {
		menu = GetComponent<MenuController>();
	}

	void Update () {
		if (gameStarted) { //TODO remove
			if (Input.GetKeyDown(KeyCode.R)) GameOver(playerOne);

			if (playerOne.GetComponent<PlayerController>().m_score >= winScore) {
				GameOver(playerOne);
			}
			if (playerTwo.GetComponent<PlayerController>().m_score >= winScore) {
				GameOver(playerTwo);
			}

			if (Input.GetKeyDown(KeyCode.M)) ToggleMuted();
			
			if (playerOne.transform.position.y < -50f) {// && playerOne.GetComponent<PlayerController>().m_hasControl){
				playerOne.GetComponent<PlayerController>().Respawn();
			} else if (playerTwo.transform.position.y < -50f) {// && playerTwo.GetComponent<PlayerController>().m_hasControl) {
				playerTwo.GetComponent<PlayerController>().Respawn();
			}
		}
	}

	
	public void StartGame () {
		//clean up old game
		if (playerOneWins != 0 || playerTwoWins != 0) {
			Destroy(playerOne.GetComponent<PlayerController>().m_playerGun);
			Destroy(playerTwo.GetComponent<PlayerController>().m_playerGun);
			Destroy(playerOne);
			Destroy(playerTwo);
		} else {print ("skupping");}

		//make a new one
		GenerateLevel();
		gameStarted = true;
		gameOver = false;
		playerOne.SetActive(true);
		playerTwo.SetActive(true);
	}

	void ResetGame () {	
		
	}

	void GenerateLevel () {
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
			playerOne = Instantiate(playerPrefab) as GameObject;
			playerOne.GetComponent<PlayerController>().m_playerNumber = 1;
			playerOne.GetComponent<PlayerController>().m_startingPosition = new Vector3 (xPos * 0.5f, -yPos * 0.5f, 0) + offset;
			playerOne.GetComponent<PlayerController>().m_respawnPosition = new Vector3 (xPos * 0.5f, -yPos * 0.5f, 0) + offset;
		} else if (blockType == 2) {
			playerTwo = Instantiate(playerPrefab) as GameObject;
			playerTwo.GetComponent<PlayerController>().m_playerNumber = 2;
			playerTwo.GetComponent<PlayerController>().m_startingPosition = new Vector3 (xPos * 0.5f, -yPos * 0.5f, 0) + offset;
			playerTwo.GetComponent<PlayerController>().m_respawnPosition = new Vector3 (xPos * 0.5f, -yPos * 0.5f, 0) + offset;
		} else {
			Instantiate(blocks[blockType - 3], new Vector3 (xPos * 0.5f, -yPos * 0.5f, 0) + offset, Quaternion.identity);
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
		menu.ShowMenu();
	}
}
