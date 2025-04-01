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
            int hours = (int)(timeElapsed / 3600);
            int minutes = (int)((timeElapsed % 3600) / 60);
            int seconds = (int)(timeElapsed % 60);
            stopwatchText.text = string.Format("{0:00} : {1:00} : {2:00}", hours, minutes, seconds);
        }
    }
}
