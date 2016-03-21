using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

	Text score1, score2;
	GameObject player1, player2;

	void Start () {
		score1 = GameObject.Find("Player One Combo Text").GetComponent<Text>();
		score2 = GameObject.Find("Player Two Combo Text").GetComponent<Text>();

		player1 = GameObject.FindWithTag("Player 1");
		player2 = GameObject.FindWithTag("Player 2");
	}
	
	void Update () {
		score1.text = (player1.GetComponent<PlayerController>().m_currentVelocity.y.ToString());
		score2.text = (player2.GetComponent<PlayerController>().m_currentVelocity.y.ToString());
	}
}
