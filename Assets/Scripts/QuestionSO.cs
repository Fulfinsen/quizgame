using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "Enter the question here";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;

    public int getCorrectAnswersIndex()
    {
        return correctAnswerIndex;
    }
    public string getAnswer (int index)
    { 
        return answers[index];
    }
    public string GetQuestion()
    {
        return question;
    }
}
