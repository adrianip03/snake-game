using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // StartCoroutine(StartGame());
        ResetScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    public void UpdateScore(int points)
    {
        score += points;
        UpdateScoreText();
    }
}
