using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private float playerScore;
    private Text playerScoreText;
    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        playerScoreText = GameObject.Find("Canvas/ScoreText").GetComponent<Text>();
        playerScoreText.text = "Score:";
    }

    // Update is called once per frame
    public void UpdatePlayerScore()
    {
        playerScoreText.text = "Score: " + playerScore;
    }

    public void AddBonusPlayerScore()
    {
        playerScore += 10;
        UpdatePlayerScore();
    }

    public void AddPlayerScore()
    {
        playerScore += 5;
        UpdatePlayerScore();
    }

    public void MinusPlayerScore()
    {
        playerScore -= 3;
        UpdatePlayerScore();
    }
}
