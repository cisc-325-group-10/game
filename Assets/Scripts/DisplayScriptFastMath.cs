using System.Collections;
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
        public int num1;
        public int num2;
        public string operation;

        /* -----------------------------------------------------------------------
         * Constructor method for Question class
         */
        public Question(String q, int a, int n1, int n2, string o)
        {
            question = q;
            answer = a;
            num1 = n1;
            num2 = n2;
            operation = o;
        }

        /* -----------------------------------------------------------------------
         * AnswerCorrect: returns true if the answer is correct (input matches the current
         *      answer) and false if not       
         */
        public bool answerCorrect(int i)
        {
            return i == answer;
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
    System.Random r = new System.Random();
    private int RandNum(int min, int max)
    {
        return r.Next(min, max);
    }

    /* -------------------------------------------------------------------------------------
 * generates a random question of 2 random numbers and either addition, subtraction, 
 *      multiplication, division or mod   
 */
    private Question generateQuestion()
    {
        Question q = new Question("", 0, 0, 0, "");
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
                return new Question(num1 + " - " + num2, answer, num1, num2, "-");
                break;
            case 2:
                // multiplication
                num1 = RandNum(0, 11);
                num2 = RandNum(0, 11);
                answer = num1 * num2;
                return new Question(num1 + " * " + num2, answer, num1, num2, "*");
                break;
            case 3:
                // division
                answer = RandNum(0, 21);
                num1 = answer * RandNum(0, 6);
                num2 = num1 / answer;
                return new Question(num1 + " / " + num2, answer, num1, num2, "/");
                break;
            case 4:
                // mod
                num1 = RandNum(0, 101);
                num2 = RandNum(0, 101);
                answer = num1 % num2;
                return new Question(num1 + " % " + num2, answer, num1, num2, "%");
                break;
            case 0:
            default:
                // addition
                num1 = RandNum(0, 101);
                num2 = RandNum(0, 101);
                answer = num1 + num2;
                return new Question(num1 + " + " + num2, answer, num1, num2, "+");
                break;
        }
    }

    /* -------------------------------------------------------------------------------------
     * Start is called before the first frame update
     */
    void Start()
    {
        //QuestionLabel.text = "Say 'Start the game' to begin";
        //String sceneNum = SceneManager.GetActiveScene().name;
        //Debug.Log(sceneNum);
    } // end Start method

    // INTERACTS WITH ALEXA
    /*
     * Called when the user says something along the lines of "Alexa, start the game"
     *      "Alexa, start game"   
     */
    public String startGame()
    {
        if (!startQuestions)
        {
            startQuestions = true;
            currentQuestion = generateQuestion();
            FindObjectOfType<SceneManagerScript>().timerGoing = true;
            return "<speak> <say-as interpret-as=\"interjection\">dun dun dun</say-as> <break strength=\"medium\"/>  Starting Fast Math Mini-Game. You must get 10 questions right in a row in order to with this game and move on. Please answer these math questions as they are given to you by saying my answer is <break time=\"500ms\"/> followed by your answer.</speak>";
        }
        return "This game is already running";
    }

    public String onHelpRequest()
    {
        if (!startQuestions)
        {
            return "Say start game when you're ready to begin playing.";
        }
        string question = currentQuestion.getQuestion();
        if (currentQuestion.operation.Equals("+"))
        {
            return "Say My answer is <break time=\"500ms\"/> followed by your answer to tell me your answer to " + currentQuestion.num1.ToString() + " plus " + currentQuestion.num2.ToString();
        } else if (currentQuestion.operation.Equals("-"))
        {
            return "Say My answer is <break time=\"500ms\"/> followed by your answer to tell me your answer to " + currentQuestion.num1.ToString() + " minus " + currentQuestion.num2.ToString();
        } else if (currentQuestion.operation.Equals("*"))
        {
            return "Say My answer is <break time=\"500ms\"/> followed by your answer to tell me your answer to " + currentQuestion.num1.ToString() + " times " + currentQuestion.num2.ToString();
        } else if (currentQuestion.operation.Equals("/"))
        {
            return "Say My answer is <break time=\"500ms\"/> followed by your answer to tell me your answer to " + currentQuestion.num1.ToString() + " divided by " + currentQuestion.num2.ToString();
        } else if (currentQuestion.operation.Equals("%"))
        {
            return "Say My answer is <break time=\"500ms\"/> followed by your answer to tell me your answer to " + currentQuestion.num1.ToString() + " mod " + currentQuestion.num2.ToString();
        }

        return "<speak>Say My answer is <break time=\"500ms\"/> followed by your answer to tell me your answer to the question.</speak>";
    }

    // INTERACTS WITH ALEXA
    /*
    * Called when the user says something along the lines of "Alexa, my answer is (answer)"
    *      where (answer is the answer string passed into the function)   
    */
    public String onAnswer(string answer)
    {
        int ans;
        if (int.TryParse(answer, out ans))
        {
            if (currentQuestion.answerCorrect(ans))
            {
                currentQuestion = generateQuestion();
                ansCorrectCount++;
                //Score.text = "Score: " + ansCorrectCount.ToString();

                if (ansCorrectCount == 2)
                {
                    //QuestionLabel.text = "Questions wrong: " + ansWrongCount.ToString()
                        //+ '\n' + "Say 'Alexa, move to next game' to \n go to the next game.";
                    startQuestions = false;
                    FindObjectOfType<SceneManagerScript>().timerGoing = false;
                    FindObjectOfType<SceneManagerScript>().gameEnd = true;
                    return "Good job! You got 10 questions right in a row. You finished with " + ansWrongCount.ToString() + " questions wrong. Say move on to play the next game.";
                }

                return "correct, your score is " + ansCorrectCount.ToString();
            }
        }
        currentQuestion = generateQuestion();
        ansWrongCount++;
        ansCorrectCount = 0;
       //Score.text = "Score: 0";
        return "Incorrect, are you even trying? your score was reset to 0";
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
        if (startQuestions)
        {
            //timer += Time.deltaTime;
            //TimerUpdate(timer);
            if (currentQuestion != null)
            {
                QuestionLabel.text = currentQuestion.getQuestion();
            }
        }

        /*
        if (ansCorrectCount == 10)
        {
            QuestionLabel.text = "Questions wrong: " + ansWrongCount.ToString()
                + '\n' + "Time: " + Math.Round(timer).ToString()
                + '\n' + "Say 'Alexa, move to next game' to \n go to the next game.";
        }*/

    } // end Update method
} // end DisplayScriptFastMath class
