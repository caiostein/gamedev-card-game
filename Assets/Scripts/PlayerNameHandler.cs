using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameHandler : MonoBehaviour
{
    public TMP_Text playerName;

    public void SetPlayerNameInput(TMP_Text playerName)
    {
        ScoreManager.Instance.playerName = playerName;
    }
}
