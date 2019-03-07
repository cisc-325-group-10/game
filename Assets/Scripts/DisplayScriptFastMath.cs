﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class DisplayScriptFastMath : MonoBehaviour
{
    public Text QuestionLabel;
    public Text Score;
    public Text TimerText;

    bool startQuestions = false;
    bool initial = true;
    public int ansCorrectCount = 0;
    public int ansWrongCount = 0;
    public int totalQuestions = 0;
    public float timer = 0.0f;
    Question currentQuestion = null;

    /* -------------------------------------------------------------------------------------
     * Class holds information for the math problem
     */
    public class Question
    {
        String question;
        int answer;

        /* -----------------------------------------------------------------------
         * Constructor method for Question class
         */
        public Question(String q, int a)
        {
            question = q;
            answer = a;
        }

        /* -----------------------------------------------------------------------
         * AnswerCorrect: returns true if the answer is correct (input matches the current
         *      answer) and false if not       
         */
        public bool answerCorrect(int i)
        {
            if (i == answer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         * returns the string in the question variable
         */
        public String getQuestion()
        {
            return question;
        }
    } // end Question class

    /* -------------------------------------------------------------------------------------
     * returns a random number, within the given range
     */
    private int RandNum(int min, int max)
    {
        System.Random r = new System.Random();
        return r.Next(min, max);
    }

    /* -------------------------------------------------------------------------------------
 * generates a random question of 2 random numbers and either addition, subtraction, 
 *      multiplication, division or mod   
 */
    private Question generateQuestion()
    {
        Question q = new Question("", 0);
        // 0 = +, 1 = -, * = 2, / = 3, % = 4
        int operation = RandNum(0, 5);
        int num1, num2, answer;

        switch (operation)
        {
            case 1:
                // subtraction
                num1 = RandNum(50, 101);
                num2 = RandNum(0, 50);
                answer = num1 - num2;
                return new Question(num1 + " - " + num2, answer);
                break;
            case 2:
                // multiplication
                num1 = RandNum(0, 11);
                num2 = RandNum(0, 11);
                answer = num1 * num2;
                return new Question(num1 + " * " + num2, answer);
                break;
            case 3:
                // division
                answer = RandNum(0, 21);
                num1 = answer * RandNum(0, 6);
                num2 = num1 / answer;
                return new Question(num1 + " / " + num2, answer);
                break;
            case 4:
                // mod
                num1 = RandNum(0, 101);
                num2 = RandNum(0, 101);
                answer = num1 % num2;
                return new Question(num1 + " % " + num2, answer);
                break;
            case 0:
            default:
                // addition
                num1 = RandNum(0, 101);
                num2 = RandNum(0, 101);
                answer = num1 + num2;
                return new Question(num1 + " + " + num2, answer);
                break;
        }
    }

    /* -------------------------------------------------------------------------------------
     * Start is called before the first frame update
     */
    void Start()
    {
        QuestionLabel.text = "Say 'Start the game' to begin";
        //String sceneNum = SceneManager.GetActiveScene().name;
        //Debug.Log(sceneNum);
    } // end Start method

    public String startGame()
    {
        // put in dun dun dun
        return "<speak><voice name=\"Matthew\"> <say-as interpret-as=\"interjection\">dun dun dun</say-as> <break strength=\"medium\"/>  Starting Fast Math Mini-Game </voice></speak>";
    }

    /*
     * This method runs the questions for the user to answer
     */
    private void runQuestions()
    {
        if (currentQuestion == null)
        {
            currentQuestion = generateQuestion();
        }
        timer += Time.deltaTime;
        TimerUpdate(timer);

        if (totalQuestions == ansCorrectCount + ansWrongCount)
        {
            totalQuestions++;
            //QuestionLabel.text = questions[RandNum(0, questions.Length)].getQuestion();
            QuestionLabel.text = currentQuestion.getQuestion();
        }

        // if answer is correct
        if (Input.GetKeyDown(KeyCode.W))
        {
            ansCorrectCount++;
            //QuestionLabel.text = "Correct!";
            Score.text = "Score: " + ansCorrectCount.ToString();
            currentQuestion = null;
        }
        // if answer is not correct
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ansWrongCount++;
            ansCorrectCount = 0;
            Score.text = "Score: " + 0;
            //QuestionLabel.text = "Wrong...Try Again.";
            currentQuestion = null;
        }
    }

    /*-------------------------------------------------------------------------------------
     * Updates the timer text if the time has passed a whole number
     */
    void TimerUpdate(float t)
    {
        TimerText.text = "Time: " + Math.Round(t).ToString();
    }


    /* -------------------------------------------------------------------------------------
     * Update is called once per frame
     */
    void Update()
    {
        // TODO: get user input from alexa here
        if (startQuestions != true && Input.anyKey)
        {
            startQuestions = true;
        }

        if (ansCorrectCount < 10 && startQuestions == true)
        {
            runQuestions();
        }

        if (ansCorrectCount == 10)
        {
            QuestionLabel.text = "Questions wrong: " + ansWrongCount.ToString()
                + '\n' + "Time: " + Math.Round(timer).ToString()
                + '\n' + "Say 'Alexa, move to next game' to \n go to the next game.";
        }

    } // end Update method
} // end DisplayScriptFastMath class
