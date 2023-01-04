using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameDisplayer : MonoBehaviour
{
    public TextMeshProUGUI playerNameDisplayerUI;

    void Update()
    {
        if(ScoreManager.Instance != null)
        {
            SetTextValue(ScoreManager.Instance.playerName.text);
        }
    }

    private void SetTextValue(string nameToUse)
    {
        playerNameDisplayerUI.text = "Usar o nome: \n'" + nameToUse + "'?";
    }

}
