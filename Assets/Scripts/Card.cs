using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool hasBeenDrawn;
	public int tableIndex;

	public int cardCost;

	public int mecanica;
	public int narrativa;
	public int estetica;
	public int tecnologia;

	GameManager gameManager;

	private Animator anim;
	private Animator camAnim;

	public int cardEffect;

	public GameObject effect;
	public GameObject hollowCircle;

	public GameObject cardDescription;

	private GameObject descriptionOnScreen;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		anim = GetComponent<Animator>();
		camAnim = Camera.main.GetComponent<Animator>();
	}
	private void OnMouseDown()
	{
		int costToUse;
			
		if (gameManager.shouldUseHalfMana)
		{
			costToUse = (int)Math.Floor((double)cardCost / 2);
		} 
		else
        {
			costToUse = cardCost;
        }

		if (IsDestroyingCards())
		{
			hasBeenDrawn = true;
			DestroyCard();
			gameManager.cardsToDestroy--;
			if (gameManager.cardsToDestroy == 0)
			{
				gameManager.DrawCard();
				gameManager.DrawCard();
				gameManager.DrawCard();
			}
		}

		if (!hasBeenDrawn && costToUse <= gameManager.remainingMana) 
		{
			if (IsSpecialCard())
			{
				gameManager.remainingMana -= costToUse;
				hasBeenDrawn = true;
				DestroyCard();
				gameManager.ActivateEffect(cardEffect);
			}
			else 
            {
				for(int i = 0; i < gameManager.availableHandSlots.Length; i++){
				
					if(gameManager.availableHandSlots[i]){
						
						Instantiate(hollowCircle, transform.position, Quaternion.identity);
			
						camAnim.SetTrigger("shake");
						anim.SetTrigger("move");

						transform.position = gameManager.handSlots[i].position;
						hasBeenDrawn = true;
						gameManager.availableTableSlots[tableIndex] = true;
						gameManager.availableHandSlots[i] = false;

						gameManager.remainingMana -= costToUse;

						gameManager.shouldUseHalfMana = false;

						ScoreManager.Instance.AddPoints(this);
						
						gameManager.table.Remove(this);
						gameManager.hand.Add(this);

						if (gameManager.hand.Count == Const.handSize)
						{							
							gameManager.CalculatePoints();
						}

						return;
					}

				}			
			}
        }
	}

    public bool IsSpecialCard()
    {
		return cardEffect > 0;
    }
	private bool IsDestroyingCards()
	{
		return gameManager.cardsToDestroy > 0;
	}
	public void DestroyCard()
	{
		gameManager.availableTableSlots[tableIndex] = true;
		gameManager.table.Remove(this);

        if (IsSpecialCard())
        {
			gameManager.drawnSpecialCards--;

			if(gameManager.drawnSpecialCards < 0)
            {
				gameManager.drawnSpecialCards = 0;
			}
        }

		if (hasBeenDrawn)
        {
			Invoke(nameof(MoveToDiscardPile), 1f);
        }

		Instantiate(effect, transform.position, Quaternion.identity);
		gameObject.SetActive(false);
	}

	private void OnMouseEnter()
    {
		descriptionOnScreen = Instantiate(cardDescription, gameManager.descriptionSlot.transform.position, Quaternion.identity);
		descriptionOnScreen.transform.localScale = new Vector3((float)0.827, (float)0.827, (float)0.827);
		descriptionOnScreen.SetActive(true);
    }

    private void OnMouseExit()
    {
        Destroy(descriptionOnScreen);
    }

    private void OnBecameInvisible()
    {
		Destroy(descriptionOnScreen);
	}

    void MoveToDiscardPile()
	{		
		gameManager.discardPile.Add(this);		
	}

}