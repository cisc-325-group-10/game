using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public bool isPlayerTurn = true;
	public Text turnText;
	public Text winLose;

    private bool dead = false;

	public GameObject[] cells;
	public bool isCPUMoved = false;

	private int n = 3;
	public bool isEnd = false;

	private string mode;
	private string level;

	// Use this for initialization
	void Start () {
        mode = "Single";
        level = "Normal";

		isPlayerTurn = true;
		turnText.text = "Player";
	}


    public string onPlayerCommand(string position) {
        if (isEnd) {
            return "no more moves can be made";
        }
        int ans;
        if (int.TryParse(position, out ans))
        {
            if (ans > 0 && ans < 10) {
                if (!isPlayerTurn) {
                   return "It's not your turn";
                }
                CellScript cell = cells[ans - 1].GetComponent<CellScript>();
                if (cell.infoCell == '_') {
                    StartCoroutine(cell.setPlayerTurn());
                    return "Move made";
                }
                
            }
        }
            return "not a valid move";
    }

	public void restart() {
        if (!dead) {
            dead = true;
            StartCoroutine(FindObjectOfType<SceneManagerScript>().bounceTicTacToe());
        }
        
	}

	// Update is called once per frame
	void Update () {
		if (turnText.text == "") {
			isEnd = true;
		}
		if (isPlayerTurn) {
			turnText.text = "Player";
		} else { //AI turn
			turnText.text = "CPU";
			StartCoroutine (CPUProcessWaiting ());
		}
	}

	IEnumerator CPUProcessWaiting() {
		yield return new WaitForSeconds (0.5f);
		if (!isCPUMoved) {
			if (checkCreateMatrix ()) { //create best move base on AlphaBetaPruning.
				CPUProcessing(false);
			} else { //create first move for CPU
				CPUProcessing(true);
			}
            //CPUProcessing(false);
      
                isCPUMoved = true;
    
			
		}
	}

	bool checkCreateMatrix() {
		int count = 0;
		for (int i = 0; i < cells.Length; i++) {
			if (cells [i].GetComponent<CellScript> ().infoCell != '_') {
				count++;
				if (count == 3)
					return true; 
			}
		}
		return false;
	}

	int randomMovingCPU(List<int> cellsIndexPlayer) {
		int indexCPU = cellsIndexPlayer[Random.Range (0, cellsIndexPlayer.Count)];
		Debug.Log ("Random CPU : " + indexCPU);
		return indexCPU;
	}

	int getBestMove() {
		Debug.Log ("/----------AlphaBeta Pruning-------------/");
		this.GetComponent<AlphaBetaPruning> ().CreateMatrixFromGame (cells);
		int bestMove = this.GetComponent<AlphaBetaPruning> ().SearchBestMoving ();
		Debug.Log ("BestMove : " + bestMove);
		return bestMove;
	}

	void CPUProcessing(bool isFirstMove) {
		int indexCPU = 0;
		/*Get Current index player*/
		string temp = "";
		List<int> cellsIndexPlayer = new List<int> ();
		int cellIndexPlayer = 0;
		for (int i = 0; i < cells.Length; i++) {
			if (cells [i].GetComponent<CellScript> ().infoCell == '_') {
				cellsIndexPlayer.Add (i);
			} else if (cells [i].GetComponent<CellScript> ().infoCell == 'O') {
				cellIndexPlayer = i;
			}
		}
		temp = "";
		for (int i = 0; i < cellsIndexPlayer.Count; i++) {
			temp += cellsIndexPlayer[i] + ",";
		}
		Debug.Log ("After :" + temp);
		/*4 Cornors*/
		bool processBestMove = false;

		int randStupid = Random.Range (1, 101);

		if (level == "Hard") {
			if (isFirstMove)
				processBestMove = true;
			else {
				//if (randStupid >= 95) { //5 % base on Lucky 
					//indexCPU = randomMovingCPU (cellsIndexPlayer);
				//} else
					processBestMove = true;
			}
		} else if (level == "Normal") {
			if (isFirstMove)
				processBestMove = true;
			else {
				if (randStupid % 2 == 0) { // 50 % base on Lucky 
					indexCPU = randomMovingCPU (cellsIndexPlayer);
				} else
					processBestMove = true;
			}

		} else if (level == "Easy") {
			indexCPU = randomMovingCPU (cellsIndexPlayer); //Totally base on lucky
		}

		if(processBestMove) {
			if (isFirstMove) {
				Debug.Log("Get hard index");
				//------4 cornors -----------//
				if (cellIndexPlayer == 0 || cellIndexPlayer == 0 ||
					cellIndexPlayer == 6 || cellIndexPlayer == 8) {
					indexCPU = 4;
			
				} else if (cellIndexPlayer == 4) {
					int[] foreCornors = { 0, 2, 6, 8 };
					indexCPU = foreCornors [Random.Range (0, foreCornors.Length)];
				} else {
					indexCPU = 4;
				}
			} else {
				
				indexCPU = getBestMove ();

			}
		}
		StartCoroutine (setXO (indexCPU));
	}

	public int checkPlayerWinLose() {// 1 : Win, -1 : Lose , 0 : Draw
		int check = -100;
		int count = 1;
		char[,] matrix = new char[n, n];

		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				matrix [i, j] = cells [i * n + j].GetComponent<CellScript>().infoCell;
				if (matrix [i, j] == 'X' || matrix[i, j] == 'O') {
					count++;
				}
			}
		}
	

		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				if (((j < n - 2) && (matrix [i, j] == matrix [i, j + 1]) && (matrix [i, j + 1] == matrix [i, j + 2]) && (matrix [i, j] != '_'))
				    || ((i < n - 2) && (matrix [i, j] == matrix [i + 1, j]) && (matrix [i + 1, j] == matrix [i + 2, j]) && (matrix [i, j] != '_'))
				    || ((i < n - 2) && (j < n - 2) && (matrix [i, j] == matrix [i + 1, j + 1]) && (matrix [i + 1, j + 1] == matrix [i + 2, j + 2]) && (matrix [i, j] != '_'))
				    || ((i < n - 2) && (j > 1) && (matrix [i, j] == matrix [i + 1, j - 1]) && (matrix [i + 1, j - 1] == matrix [i + 2, j - 2]) && (matrix [i, j] != '_'))) {
					if (matrix [i, j] == 'O')
						check = 1;
					else
						check = -1;
				}
				if (check == 1 || check == -1)
					break;

			}
			if (check == 1 || check == -1)
				break;
		}
		if (check == -100 && count == 9)
			return 0;
		return check;
	}



	public void checkWinPlayerTurn() {
		int check = checkPlayerWinLose ();
		if (check == 1) {
			winLose.text = "YOU WIN, Say Alexa, next game to move on!";
            FindObjectOfType<SceneManagerScript>().gameEnd = true;
            turnText.text = "";
        } else if (check == 0) {
			winLose.text = "DRAW";
			turnText.text = "";
            restart();
        }
	}

	IEnumerator setXO(int indexCPU) {
		if (indexCPU == -1)
			yield return null;
		cells [indexCPU].GetComponent<CellScript> ().SetXO_CPU ();
		yield return new WaitForSeconds (0.5f);
		int check = checkPlayerWinLose ();
		if (check == -1) {
			winLose.text = "YOU LOSE";
			turnText.text = "";
            restart();
		} else if (check == 0) {

			winLose.text = "DRAW";
			turnText.text = "";
            restart();
        }
	}
}
