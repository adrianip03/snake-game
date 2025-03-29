using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI tapToStartText;
    public bool isGameActive = false;
    public Snake snake;
    public Food food;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(false);
        tapToStartText.gameObject.SetActive(true);
        ResetScore();
    }

    void Update()
    {
        if (!isGameActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            StartGame();
        }
    }

    void FixedUpdate()
    {
        if (tapToStartText.gameObject.activeSelf)
        {
            float sine = Mathf.Sin(Time.time * 2f);
            // Vary font size between 100 and 150 using a sine wave
            float fontSize = 125f + 25f * sine;
            tapToStartText.fontSize = fontSize;
            
            // Vary alpha between 0.5 and 1.0 using a sine wave
            float alpha = 0.75f + 0.25f * sine;
            Color textColor = tapToStartText.color;
            textColor.a = alpha;
            tapToStartText.color = textColor;
        }
    }
    
    public void StartGame()
    {
        isGameActive = true;
        gameOverText.gameObject.SetActive(false);
        tapToStartText.gameObject.SetActive(false);
        ResetScore();
        snake.OnGameStart();
        food.OnGameStart();
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
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        tapToStartText.gameObject.SetActive(true);
        tapToStartText.text = "Tap to restart";
        isGameActive = false;
        snake.OnGameOver();
        food.OnGameOver();
    }
}
