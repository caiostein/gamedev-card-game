using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using System.Linq;
using UnityEngine.UI;

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
			
		if (!CheckPickAvailability())
           ToggleResultsBox(true);
    }

	public void DrawCard()
	{
		if (deck.Count >= 1)
		{
			camAnim.SetTrigger("shake");

			Card randomCard = deck[Random.Range(0, deck.Count)];

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
					break;
				case Enum.CardEffects.TROCA:
					cardsToDestroy = 2;
					shouldUseHalfMana = false;
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
		//ScoreManager.Instance.SetScore();

		ToggleResultsBox(false);

		ClearCardGroup(table);

        ClearCardGroup(hand);

        ClearDiscardPile();

        ClearTableSlots();
		ClearHandSlots();

        remainingMana = Const.startingMana;

		shouldForceDrawSpecialCard = true;
		drawnSpecialCards = 0;

        if(levelsToAdd == 0)
		{
			ScoreManager.Instance.ResetPoints();
        }
		
		int levelToSet = ScoreManager.Instance.activeLevel + levelsToAdd;
		ScoreManager.Instance.SetActiveLevel(levelToSet);

		StartCoroutine(FillTable());
		SetLevelDescriptionText();
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
				break;
			case (int)Enum.Levels.NARRATIVA:
				helpText.text = Enum.levelHintDict[2];
				break;
			case (int)Enum.Levels.ESTETICA:
				helpText.text = Enum.levelHintDict[3];
				break;
			case (int)Enum.Levels.TECNOLOGIA:
				helpText.text = Enum.levelHintDict[4];
				break;
		}

		informationBox.SetActive(value);
	}

	public void ToggleResultsBox(bool value)
	{
		ToggleHandlingCards(true);

		scoreText.text = ScoreManager.Instance.GetLevelPoints().ToString() + " Pontos";

		int	levelScore = ScoreManager.Instance.GetLevelPoints();


		//TODO: Implementar l�gica de exibi��o de score

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

    private string GetResultsText(IDictionary<int, string> resultsDict, int Levelscore)
    {
		if(Levelscore < 10)
        {
			return resultsDict[1];
        }
		if(Levelscore >= 10 && Levelscore < 15)
        {
			return resultsDict[2];
        }
		if(Levelscore >= 15 && Levelscore < 20)
        {
			return resultsDict[3];
        }
		if(Levelscore >= 20)
        {
			return resultsDict[4];
        }
		return null;
    }

    public void ToggleHandlingCards(bool value)
    {
		Image deckButton = deckObject.GetComponent(typeof(Image)) as Image;
		deckButton.raycastTarget = !value;
		isHandlingCards = value;
    }

}
