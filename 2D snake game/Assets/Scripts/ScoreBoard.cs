using UnityEngine;
using TMPro;
public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI newHighScoreText;
    private int score = 0;


    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void ResetScore()
    {
        newHighScoreText.gameObject.SetActive(false);
        score = 0;
        UpdateScoreText();
        UpdateHighScoreText();
    }

    public void UpdateScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }

    public void UpdateHighScore()
    {
        Debug.Log("Updating high score");
        Debug.Log("Current score: " + score);
        Debug.Log("High score: " + PlayerPrefs.GetInt("HighScore"));
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            if (!newHighScoreText.gameObject.activeSelf)
            {
                newHighScoreText.gameObject.SetActive(true);
            }
            PlayerPrefs.SetInt("HighScore", score);
            UpdateHighScoreText();
        }
    }
}
