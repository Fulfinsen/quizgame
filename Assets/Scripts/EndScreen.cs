using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI finalScoreText;
    ScoreKepper scoreKeeper;
    void Awake()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKepper>();
    }


    public void showFinalScore()
    {
        finalScoreText.text = "Congratulations!\n You got a score of " + scoreKeeper.calculateScore() + "%";

    }
}
