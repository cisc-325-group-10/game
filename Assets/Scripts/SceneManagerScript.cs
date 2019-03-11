using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // games: add more mini game names when we have more
    //string[] games = { "FastMathMini-Game", "ColourMemoryMini-Game" };
    List<string> games = new List<string>();
    string endScene = "GameEndScene";

    public bool gameEnd = true;
    public Text TotalTimerText;
    public Text LevelText;
    public float timer = 0.0f;
    public bool timerGoing = false;
    private int gamesToPlay = 2; // NOTE: make sure this number is always <= than the # of mini-games we have implemented
    private int gamesPlayed = 0;

    // Start is called before the first frame update
    void Start()
    {
        games.Add("FastMathMini-Game");
        games.Add("ColourMemoryMini-Game");
        games.Add("TicTacToe1");
        TotalTimerText.text = "Total Time: 0";
        LevelText.text = "Level: 1";
        StartCoroutine(LoadScene(getRandGame()));
    }

    void TimerUpdate(float t)
    {
        TotalTimerText.text = "Total Time: " + Math.Round(t).ToString();
    }

    /* -------------------------------------------------------------------------------------
    * returns a random number, within the given range
    */
    private string getRandGame()
    {
        System.Random r = new System.Random();
        int randNum = r.Next(0, games.Count);
        string game = games[randNum];
        games.RemoveAt(randNum);
        return game;
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }

    public IEnumerator switchScene()
    {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        if (gamesPlayed < gamesToPlay)
        {
            LevelText.text = "Level: " + (gamesPlayed + 1);
            yield return LoadScene(getRandGame());
            gamesPlayed++;
        }
        else
        {
            yield return LoadScene(endScene);
        }
    }

    // INTERACTS WITH ALEXA
    /*
    * Called when the user says something along the lines of "Alexa, move to the next game"
    *      "Alexa, move on", "Alexa, next"   
    */
    public string onNextGame()
    {
        if (gameEnd)
        {
            gameEnd = false;
            StartCoroutine(switchScene());
            return "On to the next game!";
        }
        return "You must complete this game before moving on!";
    }


    // Update is called once per frame
    public IEnumerator bounceTicTacToe()
    {
        Scene me = SceneManager.GetActiveScene();
        if (me.name.EndsWith("1"))
        {
            yield return LoadScene("TicTacToe2");
        }
        else {
            yield return LoadScene("TicTacToe1");
        }
        
        yield return SceneManager.UnloadSceneAsync(me);
        
    }

}
