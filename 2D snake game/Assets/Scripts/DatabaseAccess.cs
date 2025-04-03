using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System;

public class DatabaseAccess : MonoBehaviour
{
    private string baseUrl = "http://localhost:3000";
    private string saveUrl => $"{baseUrl}/newRun";

    public void SaveData(int score, int time)
    {
        StartCoroutine(SaveDataCoroutine(score, time));
    }

    IEnumerator SaveDataCoroutine(int score, int time)
    {
        var data = new { score = score, time = time };
        string jsonData = JsonConvert.SerializeObject(data);
        Debug.Log(jsonData);
        
        using (UnityWebRequest www = UnityWebRequest.Post(saveUrl, jsonData, "application/json"))
        {
            yield return www.SendWebRequest();
            
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score saved successfully");
            }
            else
            {
                Debug.LogError($"Error saving score: {www.error}");
            }
        }
    }
    
}