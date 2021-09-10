using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    [SerializeField ]private TextMeshProUGUI bombScoreText;

    int score;
    int bombScore;

    private void Start()
    {
        this.score = 0;
        this.scoreText = this.gameObject.GetComponent<TextMeshProUGUI>();
        this.scoreText.text = this.score.ToString();

        this.bombScore = 0;
        this.bombScoreText.text = this.bombScore.ToString();
    }

    public void IncreaseScore(int inScore)
    {
        this.score += inScore;
        this.scoreText.text = this.score.ToString();
    }

    public void IncreaseBombCount()
    {
        this.bombScore++;
        this.bombScoreText.text = this.bombScore.ToString();
    }
}
