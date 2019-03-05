﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayScriptFastMath : MonoBehaviour
{
    public Text QuestionLabel;
    public Text Score;
    public Text TimerText;

    bool startQuestions = false;
    public int ansCorrectCount = 0;
    public int ansWrongCount = 0;
    public int totalQuestions = 0;
    public float timer = 0.0f;
    public float endTime = 30.0f;

    // creating math questions to display
    //      TODO: add more of these questions later
    private Question[] questions = new Question[]
        {
            new Question("1 + 1 = ", 2),
            new Question("28 + 70 = ", 98),
            new Question("10 - 5 = ", 5),
            new Question("17 - 3 = ", 14),
            new Question("20 * 2 = ", 40),
            new Question("5 * 3 = ", 15),
            new Question("4 / 2 = ", 2),
            new Question("18 / 3 = ", 6)
    };

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
     * returns a random number, corresponding to an index in our questions array
     */
     private int RandNum()
    {
        System.Random r = new System.Random();
        return r.Next(0, questions.Length);
    }

    /* -------------------------------------------------------------------------------------
     * Start is called before the first frame update
     */
    void Start()
    {
        QuestionLabel.text = "Say 'Start' to begin...";
    } // end Start method

    /*
     * This method runs the questions for the user to answer
     */
     private void runQuestions()
    {
        timer += Time.deltaTime;
        TimerUpdate(timer);

        if (totalQuestions == ansCorrectCount + ansWrongCount)
        {
            totalQuestions++;
            QuestionLabel.text = questions[RandNum()].getQuestion();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ansCorrectCount++;
            QuestionLabel.text = "Correct!";
            Score.text = "Score: " + ansCorrectCount.ToString();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ansWrongCount++;
            QuestionLabel.text = "Wrong...Try Again.";
        }
    }

    /*-------------------------------------------------------------------------------------
     * Updates the timer text if the time has passed a whole number
     */
     void TimerUpdate(float t)
    {
        if ((double)t % 0 == 0)
        {
            TimerText.text = "Time: " + t.ToString();
        }
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

        if (startQuestions == true && timer < endTime)
        {
            runQuestions();
        }

        if (timer >= endTime)
        {
            QuestionLabel.text = "Questions correct: " + ansCorrectCount.ToString();
        }

    } // end Update method
} // end DisplayScriptFastMath class