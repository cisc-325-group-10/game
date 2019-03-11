﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // games: add more mini game names when we have more
    //string[] games = { "FastMathMini-Game", "ColourMemoryMini-Game" };
    List<string> games = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        games.Add("FastMathMini-Game");
        games.Add("ColourMemoryMini-Game");
        games.Add("TicTacToe1");
        StartCoroutine(LoadScene(games[2]));
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
