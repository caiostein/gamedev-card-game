using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public int currentLineNumber;
    
    //Screen Elements
    public GameObject nextButton;
    public GameObject nameInputField;
    public GameObject submitNameButton;
    public GameObject skipButton;
    public GameObject playButton;
    public GameObject tutorialButton;



    private void Start()
    {
        currentLineNumber = 1;
        dialogText.text = Enum.dialogLines[currentLineNumber];
    }

    public void IncreaseCurrentLineNumber()
    {
        currentLineNumber++;
    }


    public void GetNextDialogLine()
    {
        if(currentLineNumber < Enum.dialogLines.Count)
        {
            IncreaseCurrentLineNumber();

            string lineToShow = Enum.dialogLines[currentLineNumber];

            switch (currentLineNumber)
            {
                case 5:
                    nextButton.SetActive(false);
                    nameInputField.SetActive(true);
                    submitNameButton.SetActive(true);
                    break;
                case 6:
                    lineToShow = lineToShow.Replace("[PlayerName]", ScoreManager.Instance.playerName.text);
                    break;
                case 7:
                    skipButton.SetActive(true);
                    break;
                case 11:
                    nextButton.SetActive(false);
                    skipButton.SetActive(false);
                    playButton.SetActive(true);
                    tutorialButton.SetActive(true);
                    break;
            }

            dialogText.text = lineToShow;
        }
    }
}
