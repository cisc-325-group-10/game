using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndTurnScript : MonoBehaviour {

	public GameObject gameManager;
	// Use this for initialization
	void Start () {
		this.GetComponent<Button> ().interactable = false;
	}

	public void endTurn() {
		bool isPlayerTurn = gameManager.GetComponent<GameManager> ().isPlayerTurn;
		gameManager.GetComponent<GameManager> ().isPlayerTurn = !isPlayerTurn;
		this.GetComponent<Button> ().interactable = false;
	}

	public void restart() {
		
	}
}
