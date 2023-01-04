using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//Card
	public GameObject deckObject;
	public List<Card> deck;
	public TextMeshProUGUI deckSizeText;

	public Transform descriptionSlot;
	public Transform[] tableSlots;
	public bool[] availableTableSlots;

	public Transform[] handSlots;
	public bool[] availableHandSlots;

	public List<Card> discardPile;
	public TextMeshProUGUI discardPileSizeText;

	public List<Card> table;
	public List<Card> hand;

	private int lowestCost;

	public bool isHandlingCards;

	//CardEffects
	public Enum.CardEffects? activeCardEffect;
	public bool shouldUseHalfMana = false;
	public bool shouldForceDrawSpecialCard;
	public int cardsToDestroy;
	public int drawnSpecialCards;

	//Mana
	public int remainingMana;
	public TextMeshProUGUI remainingManaText;

	//Level
	public TextMeshProUGUI levelDescription;
    [SerializeField] private GameObject informationBox;
	[SerializeField] private GameObject resultsBox;
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI resultsText;
	[SerializeField] private Image[] levelScoreImages;
	[SerializeField] private GameObject ideaGain;
	public AudioSource cardHoverSound;
    public AudioSource hoverSound;
    public AudioSource clickSound;
	[SerializeField] private AudioSource drawSound;

    //FinalResults
    [SerializeField] private Image[] level1ScoreImages;
	[SerializeField] private Image[] level2ScoreImages;
	[SerializeField] private Image[] level3ScoreImages;
	[SerializeField] private Image[] level4ScoreImages;
	[SerializeField] private Image[] finalScoreImages;
    [SerializeField] private GameObject earlyResultsBox;
	[SerializeField] private GameObject finalResultsBox;
	[SerializeField] private TextMeshProUGUI finalResultsText;

	//Information
	[SerializeField] private Image schellLevelIcon;

	//System
	private Animator camAnim;

    private void Start()
	{
		camAnim = Camera.main.GetComponent<Animator>();

		remainingMana = Const.startingMana;

		shouldForceDrawSpecialCard = true;

        isHandlingCards = false;

        SetLevelDescriptionText();

		StartCoroutine(FillTable());
	}
	public void EndGame()
	{
		ScoreManager.Instance.ResetAll();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	private void SetLevelDescriptionText()
    {
        if (Enum.levelDescriptionDict.TryGetValue(ScoreManager.Instance.activeLevel, out string levelDescText))
        {
            levelDescription.text = levelDescText;
        }
    }

    private IEnumerator FillTable()
    {
		ToggleHandlingCards(true);
		while (table.Count < Const.maxTableCards)
		{
			DrawCard();
            if (!CheckPickAvailability() && !CheckDrawAvailability())
                ToggleEarlyResultsBox(true);
            yield return new WaitForSeconds(0.3f);
		}

        if (!informationBox.activeInHierarchy)
        {
			ToggleHandlingCards(false);
        }		
	}

	private void Update()
	{
		deckSizeText.text = deck.Count.ToString();
		discardPileSizeText.text = discardPile.Count.ToString();

		remainingManaText.text = remainingMana.ToString();
	}

	public void DrawCardUsingMana()
	{
	if (CheckDrawAvailability())
    {
		DrawCard();
        remainingMana--;
    }

	if (!CheckPickAvailability() && !CheckDrawAvailability())
		ToggleEarlyResultsBox(true);
	
    }

	public void DrawCard()
	{
		if (deck.Count >= 1)
		{
			camAnim.SetTrigger("shake");

			Card randomCard = deck[Random.Range(0, deck.Count)];
			drawSound.Play();
			for (int i = 0; i < availableTableSlots.Length; i++)
			{
				if (availableTableSlots[i] == true)
                {

                    if (shouldForceDrawSpecialCard && drawnSpecialCards < Const.maximumSpecialCards)
                    {
						randomCard = DrawSpecialCard();
						drawnSpecialCards++;

						SetupCardPosition(randomCard, i);
						return;
					}

					shouldForceDrawSpecialCard = false;

					if (randomCard.IsSpecialCard())
					{
						if (drawnSpecialCards >= Const.maximumSpecialCards)
						{							
							DrawCard();
							return;
						}
						drawnSpecialCards++;
					}

					SetupCardPosition(randomCard, i);
					return;
                }
            }
		}
	}

	public Card DrawSpecialCard()
	{
		List<Card> specialCardList = deck.Where(card => card.IsSpecialCard() == true).ToList();

		return specialCardList[Random.Range(0, specialCardList.Count)];
	}

    private void SetupCardPosition(Card randomCard, int index)
    {
        randomCard.gameObject.SetActive(true);
        randomCard.tableIndex = index;
        randomCard.transform.position = tableSlots[index].position;
        table.Add(randomCard);

        randomCard.hasBeenDrawn = false;
        deck.Remove(randomCard);
        availableTableSlots[index] = false;
    }

    public void Shuffle()
	{
		if (discardPile.Count >= 1)
		{
			foreach (Card card in discardPile)
			{
				deck.Add(card);
			}
			discardPile.Clear();
		}
	}

	public void ActivateEffect(int cardEffect)
    {
		if (activeCardEffect == null)
		{
			activeCardEffect = (Enum.CardEffects)cardEffect;

			switch (activeCardEffect)
			{
				case Enum.CardEffects.METADINHA:
					shouldUseHalfMana = true;
					break;
				case Enum.CardEffects.IDEIA:
					remainingMana += Const.manaToIncrease;
                    ideaGain.SetActive(false);
                    ideaGain.SetActive(true);
					break;
				case Enum.CardEffects.TROCA:
					cardsToDestroy = 2;
                    shouldUseHalfMana = false;
					SetDeckInteraction(false);
					break;
				case Enum.CardEffects.INSPIRACAO:
					shouldUseHalfMana = false;
					ClearCardGroup(table);
					ClearTableSlots();
					StartCoroutine(FillTable());
					break;
			}

			activeCardEffect = null;
		}

    }
	
	public bool CheckPickAvailability()
	{
        lowestCost = table.Min(c => c.cardCost);
		if(activeCardEffect == Enum.CardEffects.METADINHA)
			lowestCost = lowestCost / 2;
		if(lowestCost > remainingMana)
			return false;
		else 
			return true;
    }

	public bool CheckDrawAvailability()
	{
		if (remainingMana > 0 && availableTableSlots.Contains(true))
			return true;
		else
			return false;
	}

	public void TriggerLevelChange(int levelsToAdd)
    {
		ScoreManager.Instance.SetScore();

		ToggleLevelResultBox(false);

		ClearCardGroup(table);

        ClearCardGroup(hand);

        ClearDiscardPile();

        ClearTableSlots();
		ClearHandSlots();

		foreach(Image image in levelScoreImages)
        {
			image.sprite = Resources.Load<Sprite>("off_lamp");
        }

        remainingMana = Const.startingMana;

		shouldUseHalfMana = false;
		activeCardEffect = null;
		SetDeckInteraction(true);

		shouldForceDrawSpecialCard = true;
		drawnSpecialCards = 0;

        if(levelsToAdd == 0)
		{
			ScoreManager.Instance.ResetPoints();
        } else
        {
			ScoreManager.Instance.AddToTotal();
        }
		
		int levelToSet = ScoreManager.Instance.activeLevel + levelsToAdd;
		ScoreManager.Instance.SetActiveLevel(levelToSet);

		if(ScoreManager.Instance.activeLevel > 4)
        {
			ShowFinalResultsBox();
		}

		if(ScoreManager.Instance.activeLevel <= 4)
		{
		    StartCoroutine(FillTable());
			SetLevelDescriptionText();
        }
	}
	

	private void ClearHandSlots()
	{
		for (int i = 0; i < availableHandSlots.Length; i++)
        {
            availableHandSlots[i] = true;
        }
	}

	private void ClearTableSlots()
	{
		for (int i = 0; i < availableTableSlots.Length; i++)
        {
            availableTableSlots[i] = true;
        }
	}

    private void ClearCardGroup(List<Card> listToClear)
    {
        foreach (Card card in listToClear.ToList())
        {
            card.hasBeenDrawn = false;
            card.DestroyCard();
            listToClear.Remove(card);
            deck.Add(card);
        }
    }

	private void ClearDiscardPile()
    {
		foreach (Card card in discardPile.ToList())
		{
			card.hasBeenDrawn = false;
			discardPile.Remove(card);
			deck.Add(card);
		}
	}

	public void ToggleInformationBox(bool value)
	{
		TextMeshProUGUI helpText = informationBox.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;

		switch (ScoreManager.Instance.activeLevel)
		{
			case (int)Enum.Levels.MECANICA:
                helpText.text = Enum.levelHintDict[1];
				schellLevelIcon.sprite = Resources.Load<Sprite>("Schell_mec");
				break;
			case (int)Enum.Levels.NARRATIVA:
                helpText.text = Enum.levelHintDict[2];
                schellLevelIcon.sprite = Resources.Load<Sprite>("Schell_narrativa");
                break;
			case (int)Enum.Levels.ESTETICA:
                helpText.text = Enum.levelHintDict[3];
                schellLevelIcon.sprite = Resources.Load<Sprite>("Schell_estetica");
                break;
			case (int)Enum.Levels.TECNOLOGIA:
                helpText.text = Enum.levelHintDict[4];
                schellLevelIcon.sprite = Resources.Load<Sprite>("Schell_tec");
                break;
		}
		
		informationBox.SetActive(value);
	}

	public void ToggleLevelResultBox(bool value)
	{
		ToggleHandlingCards(true);

		scoreText.text = ScoreManager.Instance.GetLevelPoints().ToString() + " Pontos";

		int levelScore = ScoreManager.Instance.GetLevelPoints();

		switch (ScoreManager.Instance.activeLevel)
		{
			case (int)Enum.Levels.MECANICA:
				resultsText.text = GetResultsText(Enum.MecanicaFeedbackDict, levelScore);
				break;
			case (int)Enum.Levels.NARRATIVA:
				resultsText.text = GetResultsText(Enum.NarrativaFeedbackDict, levelScore);
				break;
			case (int)Enum.Levels.ESTETICA:
				resultsText.text = GetResultsText(Enum.EsteticaFeedbackDict, levelScore);
				break;
			case (int)Enum.Levels.TECNOLOGIA:
				resultsText.text = GetResultsText(Enum.TecnologiaFeedbackDict, levelScore);
				break;
		}

		resultsBox.SetActive(value);
	}

    public void ToggleEarlyResultsBox(bool value)
    {
        ToggleHandlingCards(value);
        earlyResultsBox.SetActive(value);        
    }

	private string GetResultsText(IDictionary<int, string> resultsDict, int Levelscore)
	{
		if (Levelscore < 10)
		{
			return resultsDict[1];
		}
		if (Levelscore >= 10 && Levelscore < 15)
		{
			levelScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			return resultsDict[2];
		}
		if (Levelscore >= 15 && Levelscore < 20)
		{
			levelScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			levelScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
			return resultsDict[3];
		}
		if (Levelscore >= 20)
		{
			levelScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			levelScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
			levelScoreImages[2].sprite = Resources.Load<Sprite>("on_lamp");
			return resultsDict[4];
		}
		return null;
	}

    private void ShowFinalResultsBox()
    {
		int level1Score = ScoreManager.Instance.level1Score;

		if (level1Score >= 10 && level1Score < 15)
		{
			level1ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");			
		}
		if (level1Score >= 15 && level1Score < 20)
		{
			level1ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level1ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");			
		}
		if (level1Score >= 20)
		{
			level1ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level1ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
			level1ScoreImages[2].sprite = Resources.Load<Sprite>("on_lamp");			
		}

		int level2Score = ScoreManager.Instance.level2Score;

		if (level2Score >= 10 && level2Score < 15)
		{
			level2ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (level2Score >= 15 && level2Score < 20)
		{
			level2ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level2ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (level2Score >= 20)
		{
			level2ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level2ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
			level2ScoreImages[2].sprite = Resources.Load<Sprite>("on_lamp");
		}

		int level3Score = ScoreManager.Instance.level3Score;

		if (level3Score >= 10 && level3Score < 15)
		{
			level3ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (level3Score >= 15 && level3Score < 20)
		{
			level3ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level3ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (level3Score >= 20)
		{
			level3ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level3ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
			level3ScoreImages[2].sprite = Resources.Load<Sprite>("on_lamp");
		}

		int level4Score = ScoreManager.Instance.level4Score;

		if (level4Score >= 10 && level4Score < 15)
		{
			level4ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (level4Score >= 15 && level4Score < 20)
		{
			level4ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level4ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (level4Score >= 20)
		{
			level4ScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			level4ScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
			level4ScoreImages[2].sprite = Resources.Load<Sprite>("on_lamp");
		}

		float finalScore = ScoreManager.Instance.finalScore / 4;

		if (finalScore >= 10 && finalScore < 15)
		{
			finalScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (finalScore >= 15 && finalScore < 20)
		{
			finalScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			finalScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
		}
		if (finalScore >= 20)
		{
			finalScoreImages[0].sprite = Resources.Load<Sprite>("on_lamp");
			finalScoreImages[1].sprite = Resources.Load<Sprite>("on_lamp");
			finalScoreImages[2].sprite = Resources.Load<Sprite>("on_lamp");
		}

		int scoreToUse = (int)System.Math.Floor(finalScore);

		finalResultsText.text = GetResultsText(Enum.FinalFeedbackDict, scoreToUse);

		finalResultsBox.SetActive(true);

	}

	public void ToggleHandlingCards(bool value)
    {
		SetDeckInteraction(!value);
		isHandlingCards = value;
    }

	public void SetDeckInteraction(bool value)
	{
        Image deckButton = deckObject.GetComponent(typeof(Image)) as Image;
        deckButton.raycastTarget = value;
    }

}
