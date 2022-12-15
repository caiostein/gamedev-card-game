using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using System.Linq;

public class GameManager : MonoBehaviour
{
	//Card
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
		string levelDescText;
		if(Enum.levelDescriptionDict.TryGetValue(ScoreManager.Instance.activeLevel, out levelDescText))
        {
			levelDescription.text = levelDescText;
		}
	}

    private IEnumerator FillTable()
    {
		isHandlingCards = true;
		while (table.Count < Const.maxTableCards)
		{
			DrawCard();
			yield return new WaitForSeconds(0.3f);
		}	
		isHandlingCards = false;
		
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
           TriggerNextLevel();
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

    internal void CalculatePoints()
    {
		Invoke(nameof(TriggerNextLevel), 0.2f);
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

	public void TriggerNextLevel()
    {
		//ScoreManager.Instance.SetScore();

		ClearCardGroup(table);

        ClearCardGroup(hand);

        ClearDiscardPile();

        ClearTableSlots();
		ClearHandSlots();

        remainingMana = Const.startingMana;

		shouldForceDrawSpecialCard = true;
		drawnSpecialCards = 0;

		int levelToSet = ScoreManager.Instance.activeLevel + 1;
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

	public void ToggleInformationBox()
	{
		informationBox.SetActive(!informationBox.activeInHierarchy);
	}

}
