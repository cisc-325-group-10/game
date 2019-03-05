﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayScript : MonoBehaviour
{
    public Text ColourLabel;
    public static int colourNumDisplayed = 4;
    public static int colourNumOptions = 5; // if we add another colour option, update this
    public string[] seenColours = new string[colourNumDisplayed];

    bool playColours = false;
    bool initial = true;
    public float colourTime = 1.0f;
    public float timer = 0.0f;
    private int counter = 0;

    /* -------------------------------------------------------------------------------------
     * Start is called before the first frame update
     */
    void Start()
    {
        ColourLabel.text = "Say 'Start' to begin...";
    } // end Start method

    /* -------------------------------------------------------------------------------------
     * returns a random number, corresponding to an index in our materials
     *      array
     */
    private int RandNum()
    {
        System.Random r = new System.Random();
        return r.Next(0, colourNumDisplayed);
    } // end RandNum method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to red, and adds 'Red' to our list
     *      of colours that the user has seen
     */
    private void ChangeToRed(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". RED";
        ColourLabel.color = Color.red;
        seenColours[i] = "RED";
    } // end ChangeToRed method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to orange, and adds 'Orange' to our list
     *      of colours that the user has seen
     */
    private void ChangeToWhite(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". WHITE";
        ColourLabel.color = Color.white;
        seenColours[i] = "WHITE";
    } // end ChangeToWhite method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to yellow, and adds 'Yellow' to our list
     *      of colours that the user has seen
     */
    private void ChangeToYellow(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". YELLOW";
        ColourLabel.color = Color.yellow;
        seenColours[i] = "YELLOW";
    } // end ChangeToYellow method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to green, and adds 'Green' to our list
     *      of colours that the user has seen
     */
    private void ChangeToGreen(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". GREEN";
        ColourLabel.color = Color.green;
        seenColours[i] = "GREEN";
    } // end ChangeToGreen method

    /* -------------------------------------------------------------------------------------
     * Changes the colour to blue, and adds 'Blue' to our list
     *      of colours that the user has seen
     */
    private void ChangeToBlue(int i)
    {
        ColourLabel.text = (i + 1).ToString() + ". BLUE";
        ColourLabel.color = Color.blue;
        seenColours[i] = "BLUE";
    } // end ChangeToBlue method

    /* -------------------------------------------------------------------------------------
     * plays the colour sequence when
     */
     void playColourSequence()
     {
        if (counter <= colourNumOptions + 1)
        {
            timer += Time.deltaTime;
        }

        // TODO: implement starting to change colours when 
        //      the user indicates they are ready

        // change the colour of the ColourLabel, and populate the coloursSeen
        //       array
        if ((timer >= colourTime) && (counter < colourNumDisplayed + 1))
        {
            int randNum = 0;
            randNum = RandNum();
            if (randNum == 0)
            {
                ChangeToRed(counter);
            }
            else if (randNum == 1)
            {
                ChangeToWhite(counter);
            }
            else if (randNum == 2)
            {
                ChangeToYellow(counter);
            }
            else if (randNum == 3)
            {
                ChangeToGreen(counter);
            }
            else
            {
                ChangeToBlue(counter);
            }

            timer = 0;
            counter++;
        }
        else if ((timer >= colourTime) && (counter == colourNumDisplayed + 1))
        {
            ColourLabel.color = Color.white;
            ColourLabel.text = "Repeat the colours";
            counter++;
            playColours = false;

            // EXTRA TESTING CODE: tests that seenColours is working
            //string str = "";
            //for (int i = 0; i < seenColours.Length; i++)
            //{
            //    str = str + seenColours[i] + " ";
            //}
            //
            //ColourLabel.text = str;
        }
        else if (counter > colourNumDisplayed + 1)
        {
            initial = false;
        }
    } // end playColourSequence method

    /* -------------------------------------------------------------------------------------
     * Update is called once per frame
     */
    void Update()
    {
        if (Input.anyKey)
        {
            if (initial == true)
            {
                // TODO: update to get user input from Alexa
                playColours = true;
            }
        }

        // when the user indicates they need to see the colour sequence, play it and
        //      update the coloursSeen array
        if (playColours == true)
        {
            playColourSequence();
        }

        // TODO: get the user's inputs from Alexa
        //      if they're correct, end
        //      if not, give them a new colour sequence
        if (initial == false)
        {
            // if the user is correct
            // TODO: add alexa stuff here
            ColourLabel.text = "CORRECT: say 'move to next room' to move to the next room";

            // if the user in not correct
            //      set playColours to true again
            // TODO: add alexa stuff here
            // ColourLabel.text = "INCORRECT: say 'try again' to try another colour sequence";
        }

    } // end Update method
} // end DisplayScript class