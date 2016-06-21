using UnityEngine;
using System.Collections;

public class UiButtonScript : MonoBehaviour {
	
	private bool selected = false;

	public Sprite normalSprite, selectedSprite, pressedSprite;
	private Sprite[] wordSprites = new Sprite[2];
	private SpriteRenderer sr;
	private MenuController mc;
	private GameObject text;

	bool isUiAction;
	string actionName;
	GameObject nextElement, previousElement;

	void Awake () {
		sr = GetComponent<SpriteRenderer>();
		mc = GameObject.FindWithTag("GameController").GetComponent<MenuController>();
	}

	public void SetUpButton (bool isUiAction, string actionName, GameObject previousElement, Sprite[] wordSprites) {
		this.isUiAction = isUiAction;
		this.actionName = actionName;
		this.previousElement = previousElement;
		this.wordSprites = wordSprites;
		//text
		text = Instantiate() as GameObject;
		//sprite
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
				StartCoroutine(Press());
			}
			//move up / down
			if (Input.GetKeyDown (GameManagerScript.DOWN_KEY) && nextElement != null) {
				Deselect();
				nextElement.GetComponent<UiButtonScript> ().Select (); //BROKEN why
			}
			if (Input.GetKeyDown (GameManagerScript.UP_KEY) && previousElement != null) {
				previousElement.GetComponent<UiButtonScript> ().Select ();
				Deselect();
			}
		}
	}

	public void Select() {
		sr.sprite = selectedSprite;
		text.GetComponent<SpriteRenderer>().sprite = normalSprite;
		selected = true;
	}

	void Deselect () {
		sr.sprite = normalSprite;
		text.GetComponent<SpriteRenderer>().sprite = normalSprite;
		selected = false;
	}

	IEnumerator Press() {
		sr.sprite = pressedSprite;
		for (int i = 0; i < 20; i++)
			yield return null;
		if (isUiAction) {
			mc.OpenMenu(actionName);
		} else {
			mc.StartUiAction(actionName);
		}
	}

	public void SetNextElement(GameObject element) {
		nextElement = element;
	}
}
