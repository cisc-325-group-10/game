using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {

	public GameObject X;
	public GameObject O;

	public GameObject gameManager;
	public char infoCell;

	// Use this for initialization
	void Start () {
		infoCell = '_';
	}
		
	public void SetXO() {
		bool isEnd = gameManager.GetComponent<GameManager> ().isEnd;
		if (isEnd)
			return;
		bool isPlayerTurn = gameManager.GetComponent<GameManager> ().isPlayerTurn;
		if (isPlayerTurn && infoCell == '_') {
			StartCoroutine (setPlayerTurn ());
		}
		
	}

	public void SetXO_CPU() {
		StartCoroutine (setCPUTurn ());
	}

	public void ResetCell() {
		X.SetActive (false);
		O.SetActive (false);
		infoCell = '_';
	}

	public IEnumerator setPlayerTurn() {
		O.SetActive (true);
		infoCell = 'O';
		gameManager.GetComponent<GameManager> ().isPlayerTurn = false;
		yield return new WaitForSeconds (0.5f);
		gameManager.GetComponent<GameManager> ().isCPUMoved = false;
	}

	IEnumerator setCPUTurn() {
		X.SetActive (true);
		infoCell = 'X';
		yield return new WaitForSeconds (0.5f);
		gameManager.GetComponent<GameManager> ().isPlayerTurn = true;
	}
}
