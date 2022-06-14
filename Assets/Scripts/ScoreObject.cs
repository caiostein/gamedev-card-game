using UnityEngine;

public class ScoreObject
{
    public string scoreValue;

    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }
    public static ScoreObject Parse(string json)
    {
        return JsonUtility.FromJson<ScoreObject>(json);
    }
}