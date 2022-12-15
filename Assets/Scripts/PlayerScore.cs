using System;
using UnityEngine;

public class PlayerScore
{
    public int pontuacao;
    public string nomeJogador;
    public string nivel;
    public string dataJogatina;

    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }
    public static PlayerScore Parse(string json)
    {
        return JsonUtility.FromJson<PlayerScore>(json);
    }
}