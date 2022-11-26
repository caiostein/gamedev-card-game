using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class GameManager : MonoBehaviour
{
	//Card
	public List<Card> deck;
	public TextMeshProUGUI deckSizeText;

	public Transform descriptionSlot;
	public Transform[] cardSlots;
	public bool[] availableCardSlots;

	public Transform[] selectedCardSlots;
	public bool[] availableSelectedCardSlots;

	public List<Card> discardPile;
	public TextMeshProUGUI discardPileSizeText;

	//CardEffects
	public Card.CardEffects? activeCardEffect;
	public bool shouldUseHalfMana = false;
	public int cardsToDestroy;

	private const int manaToIncrease = 3;
	
	//Cost
	public int remainingMana;
	public TextMeshProUGUI costPointsRemainingText;


	public TextMeshProUGUI cardCostText; // aqui eu criei um teste, fora da estrutura da carta para checar o comportamento

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

	public void DrawCard()
	{
		if (deck.Count >= 1)
		{
			camAnim.SetTrigger("shake");

			Card randomCard = deck[Random.Range(0, deck.Count)];
			cardCostText.text = "Custo anterior: " + randomCard.cardCost.ToString();
			for (int i = 0; i < availableCardSlots.Length; i++)
			{
				if (availableCardSlots[i] == true)
				{
					randomCard.gameObject.SetActive(true);
					randomCard.tableIndex = i;
					randomCard.transform.position = cardSlots[i].position;
					
					Transform organizeText = randomCard.transform.Find("CardCost");
					organizeText.localPosition = new Vector3(1.2f, 1.7f, 0);

					randomCard.GetComponentInChildren<TextMeshProUGUI>().text = randomCard.cardCost.ToString();

					randomCard.hasBeenPlayed = false;
					deck.Remove(randomCard);
					availableCardSlots[i] = false;
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
			activeCardEffect = (Card.CardEffects)cardEffect;

			switch (activeCardEffect)
			{
				case Card.CardEffects.Metadinha:
					shouldUseHalfMana = true;
					break;
				case Card.CardEffects.Ideias:
					remainingMana += manaToIncrease;
					break;
				case Card.CardEffects.Troca:
					cardsToDestroy = 2;
					//
					break;
			}

			activeCardEffect = null;
		}

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

}
