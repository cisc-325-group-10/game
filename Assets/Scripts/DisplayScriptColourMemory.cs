using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class DisplayScriptColourMemory : MonoBehaviour
{
    // COLOURS USED: red, white, yellow, green, blue, gray/grey
    public Text ColourLabel;
    public static int colourNumDisplayed = 5;
    public static int colourNumOptions = 6; // if we add another colour option, update this
    public string[] seenColours = new string[colourNumDisplayed];

    bool playColours = false;
    bool gameStarted = false;
    public float colourTime = 1.0f;
    public float timer = 0.0f;
    public float waitTimer = 0.0f;
    private bool resetTimer = false;
    private int counter = 0;

    /* -------------------------------------------------------------------------------------
     * Start is called before the first frame update
     */
    void Start()
    {
        //ColourLabel.text = "Say 'Start' to begin playing.";
        //String sceneNum = SceneManager.GetActiveScene().name;
        //Debug.Log(sceneNum);
    } // end Start method

    /* -------------------------------------------------------------------------------------
     * returns a random number, corresponding to an index in our materials
     *      array
     */
    private int RandNum()
    {
        System.Random r = new System.Random();
        return r.Next(0, colourNumOptions);
    } // end RandNum method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to red, and adds 'Red' to our list
     *      of colours that the user has seen
     */
    private void ChangeToRed(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". RED";
        ColourLabel.color = Color.red;
        seenColours[i] = "red";
    } // end ChangeToRed method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to white, and adds 'White' to our list
     *      of colours that the user has seen
     */
    private void ChangeToWhite(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". WHITE";
        ColourLabel.color = Color.white;
        seenColours[i] = "white";
    } // end ChangeToWhite method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to yellow, and adds 'Yellow' to our list
     *      of colours that the user has seen
     */
    private void ChangeToYellow(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". YELLOW";
        ColourLabel.color = Color.yellow;
        seenColours[i] = "yellow";
    } // end ChangeToYellow method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to green, and adds 'Green' to our list
     *      of colours that the user has seen
     */
    private void ChangeToGreen(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". GREEN";
        ColourLabel.color = Color.green;
        seenColours[i] = "green";
    } // end ChangeToGreen method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to blue, and adds 'Blue' to our list
     *      of colours that the user has seen
     */
    private void ChangeToBlue(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". BLUE";
        ColourLabel.color = Color.blue;
        seenColours[i] = "blue";
    } // end ChangeToBlue method

    /* -------------------------------------------------------------------------------------
    * Changes the colour to gray, and adds 'Gray' to our list
    *      of colours that the user has seen
    */
    private void ChangeToGray(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". GRAY";
        ColourLabel.color = Color.gray;
        seenColours[i] = "gray";
    } // end ChangeToGray method

    /* -------------------------------------------------------------------------------------
     * plays the colour sequence when
     */
    void playColourSequence()
    {
        // update time
        timer += Time.deltaTime;

        // change the colour of the ColourLabel, and populate the coloursSeen
        //       array
        if ((timer >= colourTime) && (counter < colourNumDisplayed))
        {
            //resetTimer = false;
            int randNum = 0;
            randNum = RandNum();

            switch (randNum)
            {
                case 1:
                    // white
                    ChangeToWhite(counter);
                    break;
                case 2:
                    // yellow
                    ChangeToYellow(counter);
                    break;
                case 3:
                    // green
                    ChangeToGreen(counter);
                    break;
                case 4:
                    // blue
                    ChangeToBlue(counter);
                    break;
                case 5:
                    // gray
                    ChangeToGray(counter);
                    break;
                case 0:
                default:
                    // red
                    ChangeToRed(counter);
                    break;
            }

            timer = 0;
            counter++;
        }
        else if (timer >= colourTime && counter >= colourNumDisplayed)
        {
            ColourLabel.color = Color.white;
            ColourLabel.text = "Say 'Alexa, the colours are ________' to give your answer";
            counter++;
            playColours = false;
            resetTimer = true;
        }
    } // end playColourSequence method 

    // INTERACTS WITH ALEXA
    /*
     * Called when the user says something along the lines of "Alexa, start the game"
     *      "Alexa, start game"   
     */
    public String startGame()
    {
        timer = 0;
        if (!gameStarted)
        {
            gameStarted = true;
            playColours = true;
            FindObjectOfType<SceneManagerScript>().timerGoing = false;
            return "<speak> <say-as interpret-as=\"interjection\">dun dun dun</say-as> <break strength=\"medium\"/>  Starting Colour Memory Mini-Game. A series of colours will be displayed on the screen. Remember them and repeat them back by saying the colours are <break time=\"500ms\"/> and then the series of colours.</speak>";
        }
        return "This game is already running";
    }

    public String onHelpRequest()
    {
        if (!gameStarted)
        {
            return "Say start game when you're ready to begin playing.";
        }
        return "<speak>Say the colours are <break time=\"500ms\"/> followed by the sequence of colours you were shown.</speak>";
    }

    // INTERACTS WITH ALEXA
    /*
    * Called when the user gives a colourGame type answer 
    * TODO: check on how the alexa formats these strings
    */
    public String onAnswer(string answer0, string answer1, string answer2, string answer3, string answer4)
    { 
        // if all answers are correct, move to end of game
        //      if not, show a new set of colours
        if (fixGray(answer0).Equals(seenColours[0]) &&
            fixGray(answer1).Equals(seenColours[1]) &&
            fixGray(answer2).Equals(seenColours[2]) &&
            fixGray(answer3).Equals(seenColours[3]) &&
            fixGray(answer4).Equals(seenColours[4]))
        {
            FindObjectOfType<SceneManagerScript>().gameEnd = true;
            FindObjectOfType<SceneManagerScript>().timerGoing = false;
            ColourLabel.text = "Correct! Say 'Move on' to go to the next game.";
            return "Correct! say move on";
        }
        playColours = true;
        return "Incorrect, lets try with a different set of colours";
    }

    /*
     * Fixes any answers that are grey/gray
     */
     private String fixGray(string s)
    {
        if (s == "grey")
        {
            s = "gray";
        }
        return s;
    }

    /* -------------------------------------------------------------------------------------
     * Update is called once per frame
     */
    void Update()
    {
        // TESTING CODE:
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            String startString = startGame();
            ColourLabel.text = startString;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            String answerString = onAnswer("red","red","red","red","red");
            ColourLabel.text = answerString;
        } */

        // when the user needs to see the colours again play them
        //      will either get an answer wrong or need to see the colours for the
        //      first time
        if (playColours == true)
        {
            if (resetTimer == true)
            {
                timer = 0;
                counter = 0;
                resetTimer = false;
            }
            playColourSequence();
        }

    } // end Update method
} // end DisplayScriptColourMemory class
