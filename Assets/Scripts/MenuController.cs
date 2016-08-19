using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	MenuPage currentPage;
	GameManagerScript gameManagerScript;
	AudioSource audioSource;
	
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

	enum MenuPage {
		MAIN,
			PLAY,
				MODE,
			OPTIONS,
				CONTROLS,
			CREDITS,
		PAUSE,
		END
	};
	
	void Awake () { //no start needed 
		gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
		audioSource = GetComponent<AudioSource>();
	}

	public void OpenMenu (string menu) { //ONLY PUBLIC MENU METHOD
		switch(menu) {
			case "main":
				currentPage = MenuPage.MAIN;
				break;
			case "play":
				currentPage = MenuPage.PLAY;
				break;
			case "modes":
				currentPage = MenuPage.MODE;
				break;
			case "options":
				currentPage = MenuPage.OPTIONS;
				break;
			case "controls":
				currentPage = MenuPage.CONTROLS;
				break;
			case "credits":
				currentPage = MenuPage.CREDITS;
				break;
			case "pause":
				currentPage = MenuPage.PAUSE;
				break;
			case "end":
				currentPage = MenuPage.END;
				break;
			default:
				Debug.Log("how did this happen");
				break;
		} 

		ShowMenu();
	}

	private void ShowMenu () { //internal method
		print("swapping to menu " + currentPage);
		HideMenu ();
		switch(currentPage) {
			case MenuPage.MAIN:
				ShowMainMenu();
				break;
			case MenuPage.PLAY:
				ShowPlayMenu();
				break;
			case MenuPage.MODE:
				ShowModeMenu();
				break;
			case MenuPage.OPTIONS:
				ShowOptionsMenu();
				break;
			case MenuPage.CONTROLS:
				ShowControlsMenu();
				break;
			case MenuPage.CREDITS:
				ShowCreditsMenu();
				break;
			case MenuPage.PAUSE:
				ShowPauseMenu();
				break;
			case MenuPage.END:
				ShowEndMenu();
				break;
			default:
				Debug.Log("how did this happen");
				break;
		}
	}

	private void HideMenu () { //clears the list
		foreach (GameObject element in uiElements) {
			Destroy(element);
		}
	}

	public void StartUiAction(string action) { //for doing things like pause
		switch (action) {
			case "points":
				gameManagerScript.timedMode = false;
				gameManagerScript.StartGame(); //TODO
				HideMenu();
				break;
			case "timed":
				gameManagerScript.timedMode = true;
				gameManagerScript.StartGame(); //TODO
				HideMenu();
				break;
			case "map0":
				gameManagerScript.currentMapNumber = 0;
				OpenMenu("modes");
				break;
			case "map1":
				gameManagerScript.currentMapNumber = 1;
				OpenMenu("modes");
				break;
			case "map2":
				gameManagerScript.currentMapNumber = 2;
				OpenMenu("modes");
				break;
			case "map3":
				gameManagerScript.currentMapNumber = 3;
				OpenMenu("modes");
				break;
			case "web":
				Application.OpenURL("http://findlang.github.io/");
				OpenMenu("main");
				break;
			case "pause":
				print("pause not implemented yet");
				break;
			case "unpause":
				print("unpause not implemented yet");
				break;
			case "quit":
				Application.Quit ();
				break;
			case "retry":
				print("retry not implemented yet");
				break;
			case "stop-game":
				print("stop game not implemented yet");
				break;
			default:
				break;
		}
	}

	private void CreateButton (Vector3 position, bool isUiAction, string actionName, GameObject previousElement, int wordSpriteNumber) { //only way to instantiate a button
		uiElements.Add(Instantiate(buttonPrefab, position, Quaternion.identity) as GameObject);
		uiElements[uiElements.Count - 1].GetComponent<UiButtonScript>().SetUpButton(isUiAction, actionName, previousElement, wordSprites[wordSpriteNumber]); //TODO
	}

	private void CreateButton (Vector3 position, bool isUiAction, string actionName, GameObject previousElement, int wordSpriteNumber, Sprite[] customSprites) {
		uiElements.Add(Instantiate(buttonPrefab, position, Quaternion.identity) as GameObject);
		uiElements[uiElements.Count - 1].GetComponent<UiButtonScript>().SetUpButton(isUiAction, actionName, previousElement, wordSprites[wordSpriteNumber], customSprites); //TODO
	}

	private void CreateImage (Vector3 position, Sprite image) { //only way to instantiate a button
		uiElements.Add(Instantiate(imagePrefab, position, Quaternion.identity) as GameObject);
		uiElements[uiElements.Count - 1].GetComponent<SpriteRenderer>().sprite = image; //TODO
	}

	private void ShowMainMenu () {		//MAIN
		//TODO Instaticate title image
		CreateImage(new Vector3(0,3f,0), titleImage);
		//Instantiate a play button
		CreateButton(new Vector3(0,0,0), true, "play", null, 0);
		//Instantiate a options button
		CreateButton(new Vector3(0,-1f, 0), true, "options", uiElements[uiElements.Count - 1], 1);
		//Instantiate a credits button
		CreateButton(new Vector3(0,-2f, 0), true, "credits", uiElements[uiElements.Count - 1], 2);
		//Instantiate a exit button
		CreateButton(new Vector3(0,-3f, 0), false, "quit", uiElements[uiElements.Count - 1], 3);
	}

	private void ShowPlayMenu () {			//PLAY
		CreateImage(new Vector3(0,3f,0), mapsTitleImage);
		//Instanciate buttons for the maps
		CreateButton(new Vector3(-4.5f,0f, 0), false, "map0", null, 10, mapButtonSprites);
		CreateButton(new Vector3(-1.5f,0f, 0), false, "map1", uiElements[uiElements.Count - 1], 11, mapButtonSprites);
		CreateButton(new Vector3(1.5f,0f, 0), false, "map2", uiElements[uiElements.Count - 1], 12, mapButtonSprites);
		CreateButton(new Vector3(4.5f,0f, 0), false, "map3", uiElements[uiElements.Count - 1], 13, mapButtonSprites);
		//Instanciate a back button
		CreateButton(new Vector3(0,-2f, 0), true, "main", uiElements[uiElements.Count - 1], 6);
	}

	private void ShowModeMenu () {			//MODE
		CreateImage(new Vector3(0,3f,0), modesTitleImage);
		//Instanciate a points mode button
		CreateButton(new Vector3(0,0f, 0), false, "points", null, 4);
		//Instanciate a time mode button
		CreateButton(new Vector3(0,-1f, 0), false, "timed", uiElements[uiElements.Count - 1], 5);
		//Instanciate a back button
		CreateButton(new Vector3(0,-2f, 0), true, "play", uiElements[uiElements.Count - 1], 6);
	}

	private void ShowOptionsMenu () {		//OPTIONS
		//title
		CreateImage(new Vector3(0,3f,0), optionsTitleImage);
		//Instanciate a back button
		CreateButton(new Vector3(0,-0f, 0), true, "main", null /*uiElements[uiElements.Count - 1]*/, 6);
	}

	private void ShowControlsMenu () {			//CONTROLS
		//Instanciate a back button
		CreateButton(new Vector3(0,-0f, 0), true, "main", null /*uiElements[uiElements.Count - 1]*/, 6);
	}

	private void ShowCreditsMenu () {		//CREDITS
		//title
		CreateImage(new Vector3(0,3f,0), creditsTitleImage);
		//Instaticate credit image
		CreateImage(new Vector3(0,1.8f,0), twitterImage1);
		//Instaticate credit image
		CreateImage(new Vector3(0,1f,0), twitterImage2);
		//Instanciate a back button
		CreateButton(new Vector3(0,0f, 0), false, "web", null, 7);
		//Instanciate a back button
		CreateButton(new Vector3(0,-1f, 0), true, "main", uiElements[uiElements.Count - 1], 6);
	}

	private void ShowPauseMenu () {		//PAUSE
		//Instantiate pause image
		CreateImage(new Vector3(0,2f,0), pausedImage);
		//TODO RESUME
		//TODO RESTART
		//TODO MENU
		CreateButton(new Vector3(0,0f, 0), false, "retry", null /*uiElements[uiElements.Count - 1]*/, 0);
		CreateButton(new Vector3(0,-1f, 0), false, "stop-game", uiElements[uiElements.Count - 1], 6);
	}

	private void ShowEndMenu () {		//END
		// Title Image
	    if (GameManagerScript.lastWinner == "Player1") {
	    	CreateImage(new Vector3(0,2f,0), player1WinImage);
		} else if (GameManagerScript.lastWinner == "Player2") {
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

	public void playSound(string name) {
		if (name == "move") {
			audioSource.PlayOneShot(moveSound);
		} else if (name == "press") {
			audioSource.PlayOneShot(pressSound);
		}
	}
}
