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
    public static int colourNumDisplayed = 4;
    public static int colourNumOptions = 6; // if we add another colour option, update this
    public string[] seenColours = new string[colourNumDisplayed];

    bool playColours = false;
    public float colourTime = 1.0f;
    public float timer = 0.0f;
    private int counter = 0;
    bool endGame = false;

    /* -------------------------------------------------------------------------------------
     * Start is called before the first frame update
     */
    void Start()
    {
        ColourLabel.text = "Say 'Start' to begin";
        String sceneNum = SceneManager.GetActiveScene().name;
        Debug.Log(sceneNum);
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
        if ((timer >= colourTime) && (counter <= colourNumDisplayed))
        {
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
        else if (timer >= colourTime && counter > colourNumDisplayed)
        {
            ColourLabel.color = Color.white;
            ColourLabel.text = "Say 'Alexa, my answer is ________' to give your answer";
            counter++;
            playColours = false;
        }
    } // end playColourSequence method 

    // INTERACTS WITH ALEXA
    /*
     * Called when the user says something along the lines of "Alexa, start the game"
     *      "Alexa, start game"   
     */
    public String startGame()
    {
        playColours = true;
        return "<speak><voice name=\"Matthew\"> <say-as interpret-as=\"interjection\">dun dun dun</say-as> <break strength=\"medium\"/>  Starting Colour Memory Mini-Game </voice></speak>";
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
        if (answer0 == seenColours[0] && 
            answer1 == seenColours[1] && 
            answer2 == seenColours[2] &&
            answer3 == seenColours[3] &&
            answer4 == seenColours[4])
        {
            endGame = true;
            return "Correct!";
        }
        playColours = true;
        return "Incorrect, lets try with a different set of colours";
    }

    // INTERACTS WITH ALEXA
    /*
    * Called when the user says something along the lines of "Alexa, move to the next game"
    *      "Alexa, move on", "Alexa, next"   
    */
    public string onNextGame()
    {
        if (endGame)
        {
            return "On to the next game!";
        }
        return "Sorry, you have to finish this game first.";
    }

    /* -------------------------------------------------------------------------------------
     * Update is called once per frame
     */
    void Update()
    {
        // TODO: Take this out once testing with alexa
        if (Input.anyKey)
        {
            playColours = true;
        } 

        // when the user needs to see the colours again play them
        //      will either get an answer wrong or need to see the colours for the
        //      first time
        if (playColours == true)
        {
            playColourSequence();
        }

    } // end Update method
} // end DisplayScriptColourMemory class
