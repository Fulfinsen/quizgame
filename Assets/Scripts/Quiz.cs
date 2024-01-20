using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random;


public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> question = new List<QuestionSO>(); 
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answersButton;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Buton colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKepper scoreKepper;

    [Header("Progress bar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;


    void Awake()
    {
        timer = FindAnyObjectByType<Timer>();
        scoreKepper = FindAnyObjectByType<ScoreKepper>();
        progressBar.maxValue = question.Count;
        progressBar.value = 0;

    }
    private void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            getNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            displayAnswer(-1);
            setButtonState(false);
        }
    }

    void displayAnswer(int index)
    {
        Image buttonImage;
        if (index == currentQuestion.getCorrectAnswersIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answersButton[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKepper.incrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentQuestion.getCorrectAnswersIndex();
            string correctAnswer = currentQuestion.getAnswer(correctAnswerIndex);
            questionText.text = "Incorrect! the correct asnwer was \n" + correctAnswer;
            buttonImage = answersButton[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void setButtonState(bool state)
    {
        for (int i = 0; i < answersButton.Length; i++)
        {
            Button button = answersButton[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
    void setDefaultButtonSprite()
    {

        for (int i = 0; i < answersButton.Length; i++)
        {
            Image buttonImage = answersButton[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
    void getNextQuestion()
    {
        if(question.Count > 0)
        {
            setButtonState(true);
            setDefaultButtonSprite();
            getRandomQuestion();
            displayQuestion();
            progressBar.value++;
            scoreKepper.incrementQuestionsSeen();
        }
    }

    void getRandomQuestion()
    {
        int index = Random.Range(0, question.Count);
        currentQuestion = question[index];

        if(question.Contains(currentQuestion))
        {
            question.Remove(currentQuestion);
        }
    }
    void displayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answersButton.Length; i++)
        {
            TextMeshProUGUI buttonText = answersButton[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.getAnswer(i);
        }
    }

    public void onAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        displayAnswer(index);
        setButtonState(false);
        timer.cancelTimer();
        scoreText.text = "Score: " + scoreKepper.calculateScore() + "%";


    }
}
