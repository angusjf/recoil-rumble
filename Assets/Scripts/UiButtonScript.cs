using UnityEngine;
using System.Collections;

public class UiButtonScript : MonoBehaviour {
	
	private bool selected = false;

	public GameObject wordPrefab;
	public Sprite normalSprite, selectedSprite, pressedSprite;
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

	public void SetUpButton (bool isUiAction, string actionName, GameObject previousElement, Sprite wordSprite) {
		this.isUiAction = isUiAction;
		this.actionName = actionName;
		this.previousElement = previousElement;
		//text
		text = Instantiate(wordPrefab, transform.position + new Vector3(0,0,-1), Quaternion.identity) as GameObject;
		text.transform.parent = gameObject.transform;
		text.GetComponent<SpriteRenderer>().sprite = wordSprite;
		//sprite
		if (previousElement == null) {
			Select ();
		} else {
			Deselect();
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
				nextElement.GetComponent<UiButtonScript> ().Select (); //BROKEN why
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

	IEnumerator Press() {
		sr.sprite = pressedSprite;
		text.transform.position += new Vector3 (0, -0.23f); // TODO exact number
		for (int i = 0; i < 20; i++) {yield return null;}
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
