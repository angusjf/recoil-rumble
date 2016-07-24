using UnityEngine;
using System.Collections;

public class UiButtonScript : MonoBehaviour {
	#region refs
	public GameObject wordPrefab;
	public Sprite normalSprite, selectedSprite, pressedSprite;
	private SpriteRenderer sr;
	private MenuController mc;
	#endregion
	#region state
	private bool selected = false;
	private bool pressed = false;
	private GameObject text;
	private bool isUiAction;
	private string actionName;
	private GameObject nextElement, previousElement;
	#endregion

	void Awake () {
		sr = GetComponent<SpriteRenderer>();
		mc = GameObject.FindWithTag("GameController").GetComponent<MenuController>();
	}

	public void SetUpButton (bool isUiAction, string actionName, GameObject previousElement, Sprite wordSprite) {
		this.isUiAction = isUiAction;
		this.actionName = actionName;
		this.previousElement = previousElement;
		//text setup
		text = Instantiate(wordPrefab, transform.position + new Vector3(0,0,-1), Quaternion.identity) as GameObject;
		text.transform.parent = gameObject.transform;
		text.GetComponent<SpriteRenderer>().sprite = wordSprite;
		//sprite setup
		if (previousElement == null) {
			Select ();
		} else {
			Deselect();
			previousElement.GetComponent<UiButtonScript>().SetNextElement(this.gameObject); //linked list
		}
	}
	
	void Update () {
		if (selected) {
			//press
			if (Input.GetKeyDown(GameManagerScript.CONFIRM_KEY) && !pressed) {
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

	void Select() {
		sr.sprite = selectedSprite;
		selected = true;
	}

	void Deselect () {
		sr.sprite = normalSprite;
		selected = false;
	}

	IEnumerator Press() {
		sr.sprite = pressedSprite;
		pressed = true;
		text.transform.position += new Vector3 (0, -0.17f); // TODO exact number
		for (int i = 0; i < 10; i++) {yield return null;}
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
