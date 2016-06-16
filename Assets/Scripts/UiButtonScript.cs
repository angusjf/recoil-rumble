using UnityEngine;
using System.Collections;

public class UiButtonScript : MonoBehaviour {
	
	private bool selected = false;

	public Sprite[] normalSprites = new Sprite[8], selectedSprites = new Sprite[8], pressedSprites = new Sprite[8];
	Sprite normalSprite, selectedSprite, pressedSprite;
	private SpriteRenderer sr;
	private MenuController mc;

	bool isUiAction;
	string actionName;
	GameObject nextElement, previousElement;

	void Awake () {
		sr = GetComponent<SpriteRenderer>();
		mc = GameObject.FindWithTag("GameController").GetComponent<MenuController>();
	}

	public void SetUpButton (bool isUiAction, string actionName, GameObject previousElement) {
		this.isUiAction = isUiAction;
		this.actionName = actionName;
		this.previousElement = previousElement;

		//sprite
		SetSprites();
		if (previousElement == null) {
			Select ();
		} else {
			sr.sprite = normalSprite;
			//linked list
			previousElement.GetComponent<UiButtonScript>().SetNextElement(this.gameObject);
		}
	}
	
	void Update () {
		if (selected) {
			//press
			if (Input.GetKeyDown(GameManagerScript.CONFIRM_KEY)) {
				//TODO swap to a different sprite
				sr.sprite = pressedSprite;
				if (isUiAction) {
					mc.OpenMenu(actionName);
				} else {
					mc.Invoke(actionName, 0f);
				}
			}

			//move up / down
			if (Input.GetKeyDown (GameManagerScript.DOWN_KEY) && nextElement != null) {
				nextElement.GetComponent<UiButtonScript> ().Select (); //BROKEN
				Deselect();
			}
			if (Input.GetKeyDown (GameManagerScript.UP_KEY) && previousElement != null) {
				previousElement.GetComponent<UiButtonScript> ().Select ();
				Deselect();
			}
		}
	}

	public void Select() {
		sr.sprite = selectedSprite;
		selected = true;
	}

	void Deselect () {
		sr.sprite = normalSprite;
		selected = false;
	}

	public void SetNextElement(GameObject element) {
		nextElement = element;
	}

	void SetSprites () {
		switch (actionName) {
		case "play": //aka back
			normalSprite = normalSprites[0];
			selectedSprite = selectedSprites[0];
			pressedSprite = pressedSprites[0];
			break;
		case "options":
			normalSprite = normalSprites[1];
			selectedSprite = selectedSprites[1];
			pressedSprite = pressedSprites[1];
			break;
		case "credits":
			normalSprite = normalSprites[2];
			selectedSprite = selectedSprites[2];
			pressedSprite = pressedSprites[2];
			break;
		case "quit":
			normalSprite = normalSprites[3];
			selectedSprite = selectedSprites[3];
			pressedSprite = pressedSprites[3];
			break;
		case "pause":
			normalSprite = normalSprites[4];
			selectedSprite = selectedSprites[4];
			pressedSprite = pressedSprites[4];
			break;
		case "end":
			normalSprite = normalSprites[5];
			selectedSprite = selectedSprites[5];
			pressedSprite = pressedSprites[5];
			break;
		case "main":
			normalSprite = normalSprites[6];
			selectedSprite = selectedSprites[6];
			pressedSprite = pressedSprites[6];
			break;
		default:
			normalSprite = normalSprites[7];
			selectedSprite = selectedSprites[7];
			pressedSprite = pressedSprites[7];
			break;
		}
		selectedSprite = selectedSprites[0];//REMOVE THIS 
		pressedSprite = pressedSprites[0]; //AND THIS !!!!!!!!!!!!
	}
}
