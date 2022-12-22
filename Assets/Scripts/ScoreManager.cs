using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int activeLevel;

    public int level1Score = 0;
    public int level2Score = 0;
    public int level3Score = 0;
    public int level4Score = 0;
    public int finalScore = 0;

    public TMP_Text playerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveLevel(int level)
    {
        Instance.activeLevel = level;
    }

    public void AddPoints(Card card)
    {
        switch (activeLevel)
        {
            case (int)Enum.Levels.MECANICA:
                level1Score += card.mecanica;
                break;
            case (int)Enum.Levels.NARRATIVA:
                level2Score += card.narrativa;
                break;
            case (int)Enum.Levels.ESTETICA:
                level3Score += card.estetica;
                break;
            case (int)Enum.Levels.TECNOLOGIA:
                level4Score += card.tecnologia;
                break;
        }
    }

    public int GetLevelPoints()
    {
        return activeLevel switch
        {
            (int)Enum.Levels.MECANICA => level1Score,
            (int)Enum.Levels.NARRATIVA => level2Score,
            (int)Enum.Levels.ESTETICA => level3Score,
            (int)Enum.Levels.TECNOLOGIA => level4Score,
            _ => 0,
        };
    }

    public void SetScore()
    {
        string levelToSet = "";
        int scoreToSet = 0;

        switch (activeLevel)
        {
            case (int)Enum.Levels.MECANICA: 
                levelToSet = "mecanica";
                scoreToSet = level1Score;
                break;
            case (int)Enum.Levels.NARRATIVA:
                levelToSet = "narrativa";
                scoreToSet = level2Score;
                break;
            case (int)Enum.Levels.ESTETICA:
                levelToSet = "estetica";
                scoreToSet = level3Score;
                break;
            case (int)Enum.Levels.TECNOLOGIA:
                levelToSet = "tecnologia";
                scoreToSet = level4Score;
                break;
        }

        Debug.Log($"Salvando Pontuação de: {scoreToSet} no banco de dados");

        PlayerScore scoreToUpload = new() { pontuacao = scoreToSet, nomeJogador = Instance.playerName.text, nivel = levelToSet, dataJogatina = DateTime.Now.ToString("dd-MM-yyyy HH:mm") };

        StartCoroutine(UploadScore(scoreToUpload.Stringify()));
    }

    public IEnumerator UploadScore(string scoreToUpload, Action<bool> callback = null)
    {

        using UnityWebRequest request = new("https://us-east-1.aws.data.mongodb-api.com/app/dbtest-ivtvy/endpoint/score", "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(scoreToUpload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
            if (callback != null)
            {
                callback.Invoke(false);
            }
        }
        else
        {
            if (callback != null)
            {
                callback.Invoke(request.downloadHandler.text != "{}");
            }
        }
    }


    public void ResetPoints()
    {
        switch (activeLevel)
        {
            case (int)Enum.Levels.MECANICA:
                level1Score = 0;
                break;
            case (int)Enum.Levels.NARRATIVA:
                level2Score = 0;
                break;
            case (int)Enum.Levels.ESTETICA:
                level3Score = 0;
                break;
            case (int)Enum.Levels.TECNOLOGIA:
                level4Score = 0;
                break;
        }
    }

    public void ResetAll()
    {
        level1Score = 0;
        level2Score = 0;
        level3Score = 0;
        level4Score = 0;
        finalScore = 0;
        activeLevel = 1;
    }

    internal void AddToTotal()
    {
        finalScore += GetLevelPoints();
    }
}
