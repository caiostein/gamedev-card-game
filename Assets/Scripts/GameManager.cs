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
	
	//Mana
	public int remainingMana;
	public TextMeshProUGUI remainingManaText;

	//Level
	public TextMeshProUGUI levelDescription;

	//System
	private Animator camAnim;

	private void Start()
	{
		camAnim = Camera.main.GetComponent<Animator>();

		remainingMana = Const.maximumMana;

		SetLevelDescriptionText();

		StartCoroutine(DrawAtBeginning());
	}

    private void SetLevelDescriptionText()
    {
		string levelDescText;
		if(Enum.levelDescriptionDict.TryGetValue(ScoreManager.Instance.activeLevel, out levelDescText))
        {
			levelDescription.text = levelDescText;
		}
	}

    private IEnumerator DrawAtBeginning()
    {
		for (int i = 0; i <= table.Capacity; i++)
		{
			DrawCard();
			yield return new WaitForSeconds(0.3f);
		}			
	}

	private void Update()
	{
		deckSizeText.text = deck.Count.ToString();
		discardPileSizeText.text = discardPile.Count.ToString();

		remainingManaText.text = remainingMana.ToString();
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
					randomCard.gameObject.SetActive(true);
					randomCard.tableIndex = i;
					randomCard.transform.position = tableSlots[i].position;
					table.Add(randomCard);

					randomCard.hasBeenDrawn = false;
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
		//Debug.Log($"Salvando Pontuação de: {totalScore} no banco de dados");

		//ScoreObject scoreToUpload = new() { scoreValue = totalScore.ToString() };

		//StartCoroutine(UploadScore(scoreToUpload.Stringify()));
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
        ClearCardGroup(table);

        ClearCardGroup(hand);

        ClearDiscardPile();

        ClearCardSlots();

        remainingMana = Const.maximumMana;

        int levelToSet = ScoreManager.Instance.activeLevel + 1;
        ScoreManager.Instance.SetActiveLevel(levelToSet);

		StartCoroutine(DrawAtBeginning());
		SetLevelDescriptionText();
	}

    private void ClearCardSlots()
    {
        for (int i = 0; i < availableHandSlots.Length; i++)
        {
            availableHandSlots[i] = true;
        }

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
}
