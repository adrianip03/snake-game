using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    public TextMeshProUGUI stopwatchText;
    private float startTime;

    private bool active = false;

    public void onGameStart()
    {
        startTime = Time.time;
        active = true;
    }

    public void onGameOver()
    {
        active = false;
    }

    void Update()
    {
        if (active)
        {
            float timeElapsed = Time.time - startTime;
            // Cap at 99:59:59 to prevent overflow
            timeElapsed = Mathf.Min(timeElapsed, 359999f); // 99 hours, 59 minutes, 59 seconds
            
            int hours = (int)(timeElapsed / 3600);
            int minutes = (int)((timeElapsed % 3600) / 60);
            int seconds = (int)(timeElapsed % 60);
            stopwatchText.text = string.Format("{0:00} : {1:00} : {2:00}", hours, minutes, seconds);
        }
    }
}
