using UnityEngine;
using System.Collections;

public class UiButtonScript : MonoBehaviour {
	
	private bool selected = false;

	public Sprite normalSprite, pressedSprite;
	private SpriteRenderer sr;
	private MenuController mc;

	bool isUiAction;
	string actionName;
	GameObject nextElement, previousElement;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
		mc = GameObject.FindWithTag("GameController").GetComponent<MenuController>();
	}

	public void SetUpButton ( bool isUiAction, string actionName, GameObject previousElement) {
		this.isUiAction = isUiAction;
		this.actionName = actionName;
		this.previousElement = previousElement;
	}
	
	void Update () {
		if (selected) {
			//press
			if (Input.GetKey(MenuController.CONFIRM_KEY)) {
				//TODO swap to a different sprite
				sr.sprite = pressedSprite;
				if (isUiAction) {
					mc.OpenMenu(actionName);
				} else {
					mc.Invoke(actionName, 0f);
				}
			}

			//move up / down
			if (Input.GetKey(MenuController.DOWN_KEY) || Input.GetKey(MenuController.DOWN_KEY)) {
				selected = false;
				//change sprite
				if (Input.GetKey(MenuController.DOWN_KEY))
					previousElement.GetComponent<UiButtonScript>().Select();
				if (Input.GetKey(MenuController.UP_KEY))
					nextElement.GetComponent<UiButtonScript>().Select();
			}
		}
	}

	public void Select() {
		sr.sprite = normalSprite;
		selected = true;
	}
}
