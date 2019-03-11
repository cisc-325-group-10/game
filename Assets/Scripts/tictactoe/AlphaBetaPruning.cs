using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlphaBetaPruning : MonoBehaviour {

	public int n = 3;
	private char[,] matrix;

	int lb = 64;
	Tree T;
	char[,] currentMatrix;

	// Use this for initialization
	void Start () {
		matrix = new char[n,n];
		currentMatrix = new char[n, n];
		InstantiateMatrix (matrix);
		InstantiateMatrix (currentMatrix);
		/*matrix[0, 0] = 'X'; 
		matrix[0, 2] = 'X'; 
		matrix[1, 1] = 'X'; matrix[1, 2] = 'O';
		matrix[2, 0] = 'O'; matrix[2, 2] = 'O';
		T = CreateMatrix (matrix, 'X', T);

		Debug.Log("Truoc khi cat tia:");
		PrintTree(T);
		Debug.Log("Qua trinh cat tia");
		PruningTree(T);
		Debug.Log("Ket qua cat tia");
		PrintTree(T);
		Debug.Log ("----------------------");*/

	}

	public void RestAlphaBetaPruning() {
		matrix = new char[n,n];
		currentMatrix = new char[n, n];
		InstantiateMatrix (matrix);
		InstantiateMatrix (currentMatrix);
		T = new Tree ();
	}

	public void CreateMatrixFromGame(GameObject[] cells) {
		for (int i = 0; i < cells.Length; i++) {
			matrix [i / 3, i % 3] = cells [i].GetComponent<CellScript> ().infoCell;
		}
		T = new Tree ();
		Debug.Log(PrintGameMatrix (matrix));
		//T = CreateMatrix (matrix, 'X', T);
		T = CreateMatrixStack (matrix, 'X', T); //--Using Stack to create tree
		Debug.Log ("Create tree complete!!!");
		PruningTree(T); //-- Pruning after created minmax tree

	}

	public char[,] getCurrentMatrix() {
		return currentMatrix;
	}

	public int SearchBestMoving() {
		//Search for best moving
		Debug.Log("Search Best Moving");
		int index = -1;
		currentMatrix = new char[n, n];
		Debug.Log (" Count Tree child : " +T.numChild + " " + T.child.Count);
		for (int i = 0; i < T.numChild; i++) {
			if (T.child [i].info == 1) {
			Debug.Log(PrintGameMatrix (T.child [i].matrix) + " " + (char)T.child[i].label + "=" + T.child[i].info + 
				" Blank count : " + T.child[i].blankCount + " Turn : " + T.child[i].c);
				currentMatrix = T.child [i].matrix; 
				return findMovingIndex (T.matrix, T.child [i].matrix);
			} else if(T.child[i].info == 0) {
				currentMatrix = T.child [i].matrix; 
				index = findMovingIndex (T.matrix, T.child [i].matrix);
			}
		}
		if (index == -1) {
			index = findMovingIndex (T.matrix, T.child [0].matrix);
		}
		return index;
	}

	int findMovingIndex(char[,] parent, char[,] child) {
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				if (parent [i, j] != child [i, j]) {
					return (i * n + j);
				}
			}
		}
		return 0;
	}

	public string PrintGameMatrix(char[,] matrix) {
		string temp = "";
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				temp += matrix[i, j].ToString() + "  ";
			}

			temp += "\n";
		}

		return temp;
	}
	void InstantiateMatrix(char[,] mtx) {
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				mtx [i, j] = '_';
			}
		}
	}

	int SetLeafValue(char[,] matrix, char c) {
		int check = 0;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				if (((j < n - 2) && (matrix [i, j] == matrix [i, j + 1]) && (matrix [i, j + 1] == matrix [i, j + 2]) && (matrix [i, j] != '_'))
					|| ((i < n - 2) && (matrix [i, j] == matrix [i + 1, j]) && (matrix [i + 1, j] == matrix [i + 2, j]) && (matrix [i, j] != '_'))
					|| ((i < n - 2) && (j < n - 2) && (matrix [i, j] == matrix [i + 1, j + 1]) && (matrix [i + 1, j + 1] == matrix [i + 2, j + 2]) && (matrix [i, j] != '_'))
					|| ((i < n - 2) && (j > 1) && (matrix [i, j] == matrix [i + 1, j - 1]) && (matrix [i + 1, j - 1] == matrix [i + 2, j - 2]) && (matrix [i, j] != '_'))

					) {
					check = 1;
				}
				if (check == 1)
					break;

			}
			if (check == 1)
				break;
		}
		return check;
	}

	private Tree CreateMatrixStack(char[,] matrix, char c, Tree tree) {
		
		List<MatrixPos> A = new List<MatrixPos>();
		tree = setInformationTree (matrix, c, tree, A);

		Stack<Tree> stackTree = new Stack<Tree> ();
		stackTree.Push (tree);
		int count = 0;
		while (stackTree.Count > 0) {
			count++;
			Tree treeTemp = stackTree.Pop ();
			Debug.Log ("Stack Tree count : " + stackTree.Count + "\n" + PrintGameMatrix(treeTemp.matrix));
			for (int i = 0; i < treeTemp.numChild; i++) {
				A = new List<MatrixPos> ();
				Debug.Log ("/------------Child------------/");
				if(treeTemp.c == 'X')
					treeTemp.child [i] = setInformationTree (treeTemp.child[i].matrix,'O', treeTemp.child [i], A);
				else if(treeTemp.c == 'O') treeTemp.child [i] = setInformationTree (treeTemp.child[i].matrix,'X', treeTemp.child [i], A);
				if(treeTemp.child[i].numChild > 0)
					stackTree.Push (treeTemp.child [i]);
			}
		}
		return tree;
	}

	private Tree setInformationTree(char[,] matrix, char c, Tree tree, List<MatrixPos> A) {
		int check = 0;
		string temp = "";
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				if (matrix[i, j] == '_') {
					MatrixPos item = new MatrixPos ();
					item.i = i;
					item.j = j;
					A.Add (item);
				}
				temp += matrix[i, j].ToString() + "  ";
			}

			temp += "\n";
		}
		Debug.Log(temp);
		check = SetLeafValue (matrix, c);
		if (tree == null)
			tree = new Tree ();
		tree.numChild = A.Count;
		if (A.Count == 0)
		{
			tree.info = 0;
		}
		if ((c == 'X') && (check == 1)) { 
			tree.info = -1;
		} else {
			if ((c == 'X') && (check != 1) && (A.Count != 0)) {
				tree.info = -2;
			} else {
				if ((c == 'O') && (check == 1))
					tree.info = 1;
				else if ((c == 'O') && (check != 1) && (A.Count != 0)) {
					tree.info = 2;
				}
			}
		}

		lb++;
		tree.label = lb;
		tree.SetMatrix (matrix, n);
		tree.c = c;
		tree.blankCount = A.Count;
		tree.check = check;
	
		if ((check == 1) || (A.Count == 0))
			tree.numChild = 0;
		else {
			for (int i = 0; i < A.Count; i++) {
				Tree child = new Tree ();
				tree.AddChild (child);
				matrix[A[i].i, A[i].j] = c;
				tree.child [i].SetMatrix (matrix, n);
				matrix[A[i].i, A[i].j] = '_';
			}
		}
		return tree;
	}

	private Tree CreateMatrix(char[,] matrix, char c, Tree tree) {
		int check = 0;
		string temp = "";
		List<MatrixPos> A = new List<MatrixPos> ();
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				if (matrix[i, j] == '_') {
					MatrixPos item = new MatrixPos ();
					item.i = i;
					item.j = j;
					A.Add (item);
				}
				temp += matrix[i, j].ToString() + "  ";
			}

			temp += "\n";
		}
		Debug.Log(temp);
		check = SetLeafValue (matrix, c);
		if (tree == null)
			tree = new Tree ();
		tree.numChild = A.Count;
		if (A.Count == 0)
		{
			tree.info = 0;
		} 
		if ((c == 'X') && (check == 1)) { 
			tree.info = -1;
		} else {
			if ((c == 'X') && (check != 1) && (A.Count != 0)) {
				tree.info = -2;
			} else {
				if ((c == 'O') && (check == 1))
					tree.info = 1;
				else if ((c == 'O') && (check != 1) && (A.Count != 0)) {
					tree.info = 2;
				}
			}
		}

		lb++;
		tree.label = lb;
		tree.SetMatrix (matrix, n);
		tree.c = c;
		tree.blankCount = A.Count;
		tree.check = check;
		if ((check == 1) || (A.Count == 0))
			tree.numChild = 0;
		else {
			for (int i = 0; i < A.Count; i++) {
				Tree child = new Tree ();
				tree.AddChild (child);
				matrix[A[i].i, A[i].j] = c;
				tree.child [i].SetMatrix (matrix, n);
				if (c == 'X')
					CreateMatrix (matrix, 'O', tree.child [i]);
				else if (c == 'O') {
					CreateMatrix (matrix, 'X', tree.child [i]);
				}
				matrix[A[i].i, A[i].j] = '_';
			}
		}
		return tree;
	}

	bool checkLeaf(Tree p) {
		if (p.numChild == 0) return true;
		else return false;
	}

	int min(int a, int b) {
		if (a < b) return a;
		else return b;
	}

	int max(int a, int b) {
		if (a > b) return a;
		else return b;
	}

	int val(Tree p, char c, int Vq) {
		int value = -100;
		if (checkLeaf (p)) {
			Debug.Log (p.label + " (leaf)->>> " + p.info);
			value = p.info;
		} else {
			if (c == 'X') {
				Vq = -2;
			} else Vq = 2;
			for (int i = 0; i < p.numChild; i++) {
				if (c == 'X') {
					p.info = max (p.info, val (p.child [i], 'O', p.info));
					Debug.Log (p.label + "->>> " + p.info);
					value = p.info;
				} else if (c == 'O') {
					p.info = min (p.info, val (p.child [i], 'X', p.info));
					Debug.Log (p.label + "->>> " + p.info);
					value = p.info;
				}
			}
		}
		return value;
	}

	void PruningTree(Tree tree) {

		if (tree != null) {
			tree.info = val (tree, 'X', tree.info);
		}
	}

	void PrintTree(Tree tree, bool isParent) {
		Debug.Log("/-------Print Tree----------/");
		string temp = "";
		if (tree != null) {
			//if (!isparent && tree.blankCount != blankCount && tree.c == 'O')
				//return;

			temp += tree.label + " = " + tree.info + " , Blank count : " + tree.blankCount + " , Turn : " + tree.c + "\n" + PrintGameMatrix(tree.matrix);
			if (checkLeaf (tree)) {
				Debug.Log(temp + " : node la");

			}
			else
				Debug.Log (temp);
			if(!isParent) return;
			for (int i = 0; i < tree.numChild; i++) {
				PrintTree(tree.child[i], false);
			}
		}
	}

	public class Tree {
		public int numChild;
		public int info;
		public int label;
		public int blankCount;
		public char c;
		public int check;
		public List<Tree> child;
		public char[,] matrix; 

		public Tree() {
			child = new List<Tree>();
			numChild = 0;
			info = 0;
			label = 0;
		}

		public void AddChild(Tree item) {
			child.Add(item);
		}

		public void SetMatrix(char[,] ma, int n) {
			matrix = new char[n, n];
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++) {
					matrix[i, j] = ma[i, j];
				}
			}
		}
	}

	class MatrixPos {
		public int i;
		public int j;

		public MatrixPos() {
			i = -100;
			j = -100;
		}
	}
}
