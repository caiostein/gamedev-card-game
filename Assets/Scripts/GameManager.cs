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

	//CardEffects
	public Enum.CardEffects? activeCardEffect;
	public bool shouldUseHalfMana = false;
	public int cardsToDestroy;
	
	//Cost
	public int remainingMana;
	public TextMeshProUGUI costPointsRemainingText;


	public TextMeshProUGUI cardCostText;

	//Score
	public int totalScore = 0;
	public TextMeshProUGUI totalScoreText;
	public ScoreObject playerScore;

	//System
	private Animator camAnim;

	private void Start()
	{
		camAnim = Camera.main.GetComponent<Animator>();
	}
	private void Update()
	{
		deckSizeText.text = deck.Count.ToString();
		discardPileSizeText.text = discardPile.Count.ToString();

		if (!totalScoreText.text.Equals(totalScore.ToString()))
		{
			totalScoreText.text = totalScore.ToString();
		}

		costPointsRemainingText.text = "Mana Restante: " + remainingMana.ToString();

	}

	public void DrawCard()
	{
		if (deck.Count >= 1)
		{
			camAnim.SetTrigger("shake");

			Card randomCard = deck[Random.Range(0, deck.Count)];

			//avaliar se possivel deletar
			cardCostText.text = "Custo anterior: " + randomCard.cardCost.ToString();

			for (int i = 0; i < availableTableSlots.Length; i++)
			{
				if (availableTableSlots[i] == true)
				{
					randomCard.gameObject.SetActive(true);
					randomCard.tableIndex = i;
					randomCard.transform.position = tableSlots[i].position;
					table.Add(randomCard);

					Debug.Log(randomCard.cardCost);
					
					Transform organizeText = randomCard.transform.Find("CardCost");
					organizeText.localPosition = new Vector3(1.2f, 1.7f, 0);

					randomCard.GetComponentInChildren<TextMeshProUGUI>().text = randomCard.cardCost.ToString();

					randomCard.hasBeenPlayed = false;
					deck.Remove(randomCard);
					availableTableSlots[i] = false;
					return;
				}
			}
		}
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
		Debug.Log(ScoreManager.Instance.level1Score);
		Invoke(nameof(TriggerNextLevel), 0.2f);

	}

    public void SetScore()
    {
		Debug.Log($"Salvando Pontuação de: {totalScore} no banco de dados");

		ScoreObject scoreToUpload = new() { scoreValue = totalScore.ToString() };

		StartCoroutine(UploadScore(scoreToUpload.Stringify()));
	}

	public IEnumerator UploadScore(string profile, System.Action<bool> callback = null)
    {

		using (UnityWebRequest request = new UnityWebRequest("https://us-east-1.aws.data.mongodb-api.com/app/dbtest-ivtvy/endpoint/playerData", "POST"))
		{
			request.SetRequestHeader("Content-Type", "application/json");
			byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.Log(request.error);
				if (callback != null)
				{
					callback.Invoke(false);
				}
			}
			else
			{
				if (callback != null)
				{
					callback.Invoke(request.downloadHandler.text != "{}");
				}
			}
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
					break;
			}

			activeCardEffect = null;
		}

    }

	public void TriggerNextLevel()
    {
		
		foreach(Card card in table.ToList())
        {
			card.hasBeenPlayed = false;
			card.DestroyCard();
			table.Remove(card);
			deck.Add(card);
        }

		foreach(Card card in hand.ToList())
        {
			card.hasBeenPlayed = false;
			card.DestroyCard();
			hand.Remove(card);
			deck.Add(card);
        }

		foreach(Card card in discardPile.ToList())
        {
			card.hasBeenPlayed = false;
			discardPile.Remove(card);
			deck.Add(card);
        }

		for (int i = 0; i < availableHandSlots.Length; i++)
        {
			availableHandSlots[i] = true;
        }

        for (int i = 0; i < availableTableSlots.Length; i++)
        {
			availableTableSlots[i] = true;
        }

		remainingMana = Const.maximumMana;
		int activeLevel = ScoreManager.Instance.activeLevel;
		int levelToSet = activeLevel + 1;
		ScoreManager.Instance.SetActiveLevel(levelToSet);
	}

}
