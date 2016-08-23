using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	GameManagerScript gameManagerScript;
	AudioSource audioSource;
	MenuPage currentPage;

	List<GameObject> uiElements = new List<GameObject>();
	public GameObject imagePrefab;
	public GameObject buttonPrefab;

	public Sprite titleImage, mapsTitleImage, modesTitleImage, twitterImage1, twitterImage2,
		pausedImage, player1WinImage, player2WinImage, optionsTitleImage, creditsTitleImage;
	public Sprite[] wordSprites;
	public Sprite[] redTallySprites;
	public Sprite[] blueTallySprites;
	public Sprite[] mapButtonSprites;

	public AudioClip moveSound;
	public AudioClip pressSound;

	public const string CONFIRM_BUTTON = "MenuConfirm", CANCEL_BUTTON = "MenuCancel", UP_BUTTON = "MenuUp", DOWN_BUTTON = "MenuDown";

	enum MenuPage { MAIN, PLAY, MODE, OPTIONS, CONTROLS, CREDITS, PAUSE, END };

	void Awake () {
		gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
		audioSource = GetComponent<AudioSource>();
		gameManagerScript.startEvent += ShowMainMenu;
	}

	private void HideMenu () { //clears the list
		foreach (GameObject element in uiElements) {
			Destroy(element);
		}
		uiElements.Clear();
	}

	private void CreateButton (Vector3 position, UiButtonScript.ActionOnPressed actionOnPressed, GameObject previousElement, int wordSpriteNumber) { //only way to instantiate a button
		uiElements.Add(Instantiate(buttonPrefab, position, Quaternion.identity) as GameObject);
		uiElements[uiElements.Count - 1].GetComponent<UiButtonScript>().SetUpButton(actionOnPressed, previousElement, wordSprites[wordSpriteNumber]); //TODO
	}

	private void CreateButton (Vector3 position, UiButtonScript.ActionOnPressed actionOnPressed, GameObject previousElement, int wordSpriteNumber, Sprite[] customSprites) {
		uiElements.Add(Instantiate(buttonPrefab, position, Quaternion.identity) as GameObject);
		uiElements[uiElements.Count - 1].GetComponent<UiButtonScript>().SetUpButton(actionOnPressed, previousElement, wordSprites[wordSpriteNumber], customSprites); //TODO
	}

	private void CreateImage (Vector3 position, Sprite image) { //only way to instantiate a button
		uiElements.Add(Instantiate(imagePrefab, position, Quaternion.identity) as GameObject);
		uiElements[uiElements.Count - 1].GetComponent<SpriteRenderer>().sprite = image; //TODO
	}

	private void ShowMainMenu () {
		HideMenu();
		CreateImage(new Vector3(0,3f,0), titleImage);
		CreateButton(new Vector3(0,0,0), ShowPlayMenu, null, 0);
		CreateButton(new Vector3(0,-1f, 0), ShowOptionsMenu, uiElements[uiElements.Count - 1], 1);
		CreateButton(new Vector3(0,-2f, 0), ShowCreditsMenu, uiElements[uiElements.Count - 1], 2);
		CreateButton(new Vector3(0,-3f, 0), Application.Quit, uiElements[uiElements.Count - 1], 3);
	}

	private void ShowPlayMenu () {
		HideMenu();
		CreateImage(new Vector3(0,3f,0), mapsTitleImage);
		CreateButton(new Vector3(-4.5f, 0f, 0), SetMap0, null, 10, mapButtonSprites);
		CreateButton(new Vector3(-1.5f, 0f, 0), SetMap1, uiElements[uiElements.Count - 1], 11, mapButtonSprites);
		CreateButton(new Vector3(+1.5f, 0f, 0), SetMap2, uiElements[uiElements.Count - 1], 12, mapButtonSprites);
		CreateButton(new Vector3(+4.5f, 0f, 0), SetMap3, uiElements[uiElements.Count - 1], 13, mapButtonSprites);
		CreateButton(new Vector3(0,-2f, 0), ShowMainMenu, uiElements[uiElements.Count - 1], 6);
	}

	private void SetMap0() {
		gameManagerScript.currentMapNumber = 0;
		ShowModeMenu();
	}
	private void SetMap1() {
		gameManagerScript.currentMapNumber = 1;
		ShowModeMenu();
	}
	private void SetMap2() {
		gameManagerScript.currentMapNumber = 2;
		ShowModeMenu();
	}
	private void SetMap3() {
		gameManagerScript.currentMapNumber = 3;
		ShowModeMenu();
	}

	private void ShowModeMenu () {			//MODE
		HideMenu();
		CreateImage(new Vector3(0,3f,0), modesTitleImage);
		CreateButton(new Vector3(0,0f, 0), StartPointsGame, null, 4); //Instanciate a points mode button
		CreateButton(new Vector3(0,-1f, 0), StartTimedGame, uiElements[uiElements.Count - 1], 5); //Instanciate a time mode button
		CreateButton(new Vector3(0,-2f, 0), ShowPlayMenu, uiElements[uiElements.Count - 1], 6); //Instanciate a back button
	}

	void StartPointsGame() {
		gameManagerScript.timedMode = false;
		gameManagerScript.StartGame(); //TODO
		HideMenu();
	}
	void StartTimedGame () {
		gameManagerScript.timedMode = true;
		gameManagerScript.StartGame(); //TODO
		HideMenu();
	}

	private void ShowOptionsMenu () {		//OPTIONS
		HideMenu();
		CreateImage(new Vector3(0,3f,0), optionsTitleImage);
		CreateButton(new Vector3(0,-0f, 0), ShowMainMenu, null, 6);
	}

	private void ShowControlsMenu () {			//CONTROLS
		HideMenu();
		CreateButton(new Vector3(0,-0f, 0), ShowMainMenu, null /*uiElements[uiElements.Count - 1]*/, 6);
	}

	private void ShowCreditsMenu () {		//CREDITS
		HideMenu();
		CreateImage(new Vector3(0,3f,0), creditsTitleImage);
		CreateImage(new Vector3(0,1.8f,0), twitterImage1);
		CreateImage(new Vector3(0,1f,0), twitterImage2);
		CreateButton(new Vector3(0,0f, 0), OpenWebsite, null, 7);
		CreateButton(new Vector3(0,-1f, 0), ShowMainMenu, uiElements[uiElements.Count - 1], 6);
	}

	void OpenWebsite () {
		Application.OpenURL("http://findlang.github.io/");
	}

	void RetryGame () {
		gameManagerScript.StartGame();
		HideMenu();
	}

	void StopGame () {
		gameManagerScript.EndGame();
		HideMenu();
	}

	private void ShowPauseMenu () {		//PAUSE
		//Instantiate pause image
		CreateImage(new Vector3(0,2f,0), pausedImage);
		//TODO RESUME
		//TODO RESTART
		//TODO MENU
		CreateButton(new Vector3(0,0f, 0), RetryGame, null, 0);
		CreateButton(new Vector3(0,-1f, 0), StopGame, uiElements[uiElements.Count - 1], 6);
	}

	/*
	private void ShowEndMenu () {		//END
		// Title Image
	    if (gameManagerScript.winner.tag == "Player1") {
	    	CreateImage(new Vector3(0,2f,0), player1WinImage);
		} else if (gameManagerScript.winner.tag == "Player2") {
			CreateImage(new Vector3(0,2f,0), player2WinImage);
		} else {
			//no winner...
		}
		//score 1 TODO
		CreateImage(new Vector3(-3,-1.2f,0),
			redTallySprites[gameManagerScript.GetPlayer(1).GetComponent<PlayerController>().m_score]);

		//score 2 TODO
		CreateImage(new Vector3(3,-1.2f,0),
			blueTallySprites[gameManagerScript.GetPlayer(2).GetComponent<PlayerController>().m_score]);

		CreateButton(new Vector3(0,-3f, 0), false, "points", null, 8); //TODO change it to current mode eg "timed"
		CreateButton(new Vector3(0,-4f, 0), true, "main", uiElements[uiElements.Count - 1], 9);
	}
	*/
	public void playSound(string name) {
		if (name == "move") {
			audioSource.PlayOneShot(moveSound);
		} else if (name == "press") {
			audioSource.PlayOneShot(pressSound);
		}
	}
}
