using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject npcName;
    [SerializeField] private GameObject bgDropText;
    [SerializeField] private GameObject bgDropFull;
    [SerializeField] private GameObject npcObject;
    [SerializeField] private Image npcSprite;
    [SerializeField] private Image scenario;


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
                case 3:
                    scenario.sprite = Resources.Load<Sprite>("bg1");
                    break;
                case 4:
                    npcObject.SetActive(true);
                    break;
                case 5:
                    npcSprite.sprite = Resources.Load<Sprite>("funcionario_2");
                    npcName.SetActive(true);
                    break;
                case 6:
                    bgDropFull.SetActive(true);
                    bgDropText.SetActive(false);
                    nextButton.SetActive(false);
                    nameInputField.SetActive(true);
                    submitNameButton.SetActive(true);
                    npcName.SetActive(false);
                    break;
                case 7:
                    bgDropFull.SetActive(false);
                    bgDropText.SetActive(true);
                    npcName.SetActive(true);
                    npcSprite.sprite = Resources.Load<Sprite>("funcionario_3");
                    lineToShow = lineToShow.Replace("[PlayerName]", ScoreManager.Instance.playerName.text);
                    break;
                case 8:
                    skipButton.SetActive(true);
                    tutorialButton.SetActive(true);
                    break;
                case 9:
                    npcSprite.sprite = Resources.Load<Sprite>("funcionario_2");
                    break;
                case 12:
                    nextButton.SetActive(false);
                    skipButton.SetActive(false);
                    playButton.SetActive(true);

                    break;
            }

            dialogText.text = lineToShow;
        }
    }
}
