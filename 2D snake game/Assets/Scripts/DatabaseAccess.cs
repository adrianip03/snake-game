using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DatabaseAccess : MonoBehaviour
{
    private string webUrl = "http://localhost:8888/sqlconnect/newRun.php";

    public void SaveData(int score, int time)
    {
        StartCoroutine(SaveDataCoroutine(score, time));
    }

    IEnumerator SaveDataCoroutine(int score, int time)
    {
        WWWForm form = new WWWForm();
        form.AddField("score", score);
        form.AddField("time", time);
        
        using (UnityWebRequest www = UnityWebRequest.Post(webUrl, form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Data saved successfully");
                }
                else
                {
                    Debug.Log("Failed to save data. Error: " + www.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Error: " + www.error);
            }
        }
    }
}
