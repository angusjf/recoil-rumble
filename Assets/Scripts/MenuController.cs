﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	MenuPage currentPage;
	GameManagerScript gameManagerScript;
	
	List<GameObject> uiElements = new List<GameObject>();
	public GameObject buttonPrefab;

	enum MenuPage {
		MAIN,
			PLAY,
			OPTIONS,
				CONTROLS,
			CREDITS,
		PAUSE,
		END
	};
	
	void Awake () { //no start needed 
		gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
	}

	public void OpenMenu (string menu) { //ONLY PUBLIC MENU METHOD
		switch(menu) {
			case "main":
				currentPage = MenuPage.MAIN;
				break;
			case "play":
				currentPage = MenuPage.PLAY;
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
				gameManagerScript.StartGame();
				HideMenu();
				break;
			case "timed":
				gameManagerScript.StartGame();
				HideMenu();
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
			default:
				break;
		}
	}

	private void CreateButton (Vector3 position, bool isUiAction, string actionName, GameObject previousElement) { //only way to instantiate a button
		uiElements.Add(Instantiate(buttonPrefab, position, Quaternion.identity) as GameObject);
		uiElements[uiElements.Count - 1].GetComponent<UiButtonScript>().SetUpButton(isUiAction, actionName, previousElement); //TODO
	}

	private void ShowMainMenu () {		//MAIN
		//Instantiate a play button
		CreateButton(new Vector3(0,0,0), true, "play", null);
		//Instantiate a options button
		CreateButton(new Vector3(0,-1f, 0), true, "options", uiElements[uiElements.Count - 1]);
		//Instantiate a credits button
		CreateButton(new Vector3(0,-2f, 0), true, "credits", uiElements[uiElements.Count - 1]);
		//Instantiate a exit button
		CreateButton(new Vector3(0,-3f, 0), false, "quit", uiElements[uiElements.Count -  1]);
	}

	private void ShowPlayMenu () {			//PLAY
		//Instanciate a points mode button
		CreateButton(new Vector3(0,0f, 0), false, "points", null);
		//Instanciate a time mode button
		CreateButton(new Vector3(0,-1f, 0), false, "timed", uiElements[uiElements.Count -  1]);
		//Instanciate a back button
		CreateButton(new Vector3(0,-2f, 0), true, "main", uiElements[uiElements.Count -  1]);
	}

	private void ShowOptionsMenu () {		//OPTIONS
		//Instanciate a back button
		CreateButton(new Vector3(0,-0f, 0), true, "main", null /*uiElements[uiElements.Count -  1]*/);
	}

	private void ShowControlsMenu () {			//CONTROLS
		//Instanciate a back button
		CreateButton(new Vector3(0,-0f, 0), true, "main", null /*uiElements[uiElements.Count -  1]*/);
	}

	private void ShowCreditsMenu () {		//CREDITS
		//Instanciate a back button
		CreateButton(new Vector3(0,-0f, 0), true, "main", null /*uiElements[uiElements.Count -  1]*/);
	}

	private void ShowPauseMenu () {		//PAUSE
		//Instanciate a back button
		CreateButton(new Vector3(0,-0f, 0), true, "main", null /*uiElements[uiElements.Count -  1]*/);
	}

	private void ShowEndMenu () {		//END
	/*
		// Title Image
		uiElements.Add(Instantiate (titleImagePrefab) as GameObject);
		if (GameManagerScript.lastWinner == null) {
			uiElements[0].GetComponent<SpriteRenderer> ().sprite = titleSprites [0];
		} else {
			uiElements[0].GetComponent<SpriteRenderer> ().sprite = GameManagerScript.lastWinner == "Player1" ? titleSprites [1] : titleSprites [2];
		}

		// Ready Buttons
		// ReadyOne
		uiElements.Add(Instantiate (readyButtonPrefab) as GameObject);
		uiElements[1].transform.position = Vector3.right * 3;
		uiElements.Add(Instantiate (readyButtonPrefab) as GameObject);
		uiElements[2].transform.position = Vector3.left * 3;

		// Tally Images
		uiElements.Add(Instantiate (tallyImagePrefab) as GameObject);		
		if (GameManagerScript.playerOneWins < 1  TODO || GameManagerScript.playerOneWins > tornament lengthghh ) {
			uiElements[3].GetComponent<SpriteRenderer> ().enabled = false;
		} else {
			uiElements[3].GetComponent<SpriteRenderer> ().enabled = true;
			uiElements[3].GetComponent<SpriteRenderer> ().sprite = tally1Sprites[GameManagerScript.playerOneWins - 1];
		}
		uiElements.Add(Instantiate (tallyImagePrefab) as GameObject);		
		if (GameManagerScript.playerTwoWins < 1) {
			uiElements[4].GetComponent<SpriteRenderer> ().enabled = false;
		} else {
			uiElements[4].GetComponent<SpriteRenderer> ().enabled = true;
			uiElements[4].GetComponent<SpriteRenderer> ().sprite = tally2Sprites[GameManagerScript.playerTwoWins - 1];
		}
	*/
	}
}
