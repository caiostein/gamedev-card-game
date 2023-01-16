using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameHandler : MonoBehaviour
{
    public TMP_Text playerName;
    public Button submitButton;
    public TMP_InputField nameField;

    public void SetPlayerNameInput(TMP_Text playerName)
    {
        ScoreManager.Instance.playerName = playerName;
    }

    private void Update()
    {
        if (!Regex.IsMatch(nameField.text, @"[a-zA-Z]+\d"))
        {
            submitButton.interactable = false;
            submitButton.GetComponentInChildren<TMP_Text>().color = new Color32(0, 0, 0, 100);
        }
        else
        {
            submitButton.interactable = true;
            submitButton.GetComponentInChildren<TMP_Text>().color = new Color32(0, 0, 0, 255);
        }
    }
}
