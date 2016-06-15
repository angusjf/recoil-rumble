using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	MenuPage currentPage;
	GameManagerScript gameManagerScript;
	
	List<GameObject> uiElements = new List<GameObject>();
	GameObject buttonPrefab;
	
	public static KeyCode CONFIRM_KEY = KeyCode.Return;
	public static KeyCode CANCEL_KEY = KeyCode.Backspace;
	public static KeyCode UP_KEY = KeyCode.UpArrow;
	public static KeyCode DOWN_KEY = KeyCode.UpArrow;

	enum MenuPage {
		MAIN,
			PLAY,
			OPTIONS,
				CONTROLS,
			CREDITS,
		PAUSE,
		END
	};
	
	void Start () {
		gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();
		currentPage = MenuPage.MAIN;
		ShowMenu();
	}

	public void OpenMenu (string menu) { //ONLY PUBLIC MENU METHOD
		switch(menu) {
			case "main":
				currentPage = MenuPage.MAIN;
				break;
			case "play":
				currentPage = MenuPage.MAIN;
				break;
			case "options":
				currentPage = MenuPage.MAIN;
				break;
			case "controls":
				currentPage = MenuPage.MAIN;
				break;
			case "credits":
				currentPage = MenuPage.MAIN;
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
		HideMenu ();
		switch(currentPage) {
			case MenuPage.MAIN:
				ShowEndMenu();
				break;
			case MenuPage.PAUSE:
				ShowEndMenu();
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
			case "start":
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
		//uiElements[uiElements.Count - 1].GetComponent<UiButtonScript>().SetUpButton(bool isUiAction, string actionName, GameObject previousElement); //TODO
	}

	private void ShowMainMenu () {		//MAIN
		//Instantiate a play button
		CreateButton(Vector3.zero, true, "play", uiElements[uiElements.Count - 2]); //MAYBE - 1 ???
		//link button to call openMenu with "play"
		//Instantiate a options button
		//link button to call openMenu with "option"
		//Instantiate a credits button
		//link button to call openMenu with "credits"
		//Instantiate a exit button
		//link button to call GameManager.Exit
	}

	private void ShowPlayMenu () {			//PLAY
		//Instanciate a points mode button
		//link button to call OpenMenu with points mode
		//Instanciate a time mode button
		//link button to call OpenMenu with time mode
		//Instanciate a back button
		//link button to call OpenMenu with Main
	}

	private void ShowOptionsMenu () {		//OPTIONS

	}

	private void ShowControlsMenu () {			//CONTROLS

	}

	private void ShowCreditsMenu () {		//CREDITS

	}

	private void ShowPauseMenu () {		//PAUSE
		
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
