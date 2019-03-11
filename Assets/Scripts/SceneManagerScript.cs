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

    public bool gameEnd = true;
    public Text TotalTimerText;
    public float timer = 0.0f;
    public bool timerGoing = false;

    // Start is called before the first frame update
    void Start()
    {
        games.Add("FastMathMini-Game");
        games.Add("ColourMemoryMini-Game");
        TotalTimerText.text = "Total Time: 0";
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
        yield return LoadScene(getRandGame());
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
    void Update()
    {
        if (timerGoing)
        {
            timer += Time.deltaTime;
            TimerUpdate(timer);
        }
    }
}
