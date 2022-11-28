using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public int tableIndex;

	public int cardCost;
	public int cardValue;

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
	
	public GameObject parent;

	private GameObject descriptionOnScreen;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		anim = GetComponent<Animator>();
		camAnim = Camera.main.GetComponent<Animator>();
	}
	private void OnMouseDown()
	{
		if (!hasBeenPlayed && cardCost <= gameManager.remainingMana) 
		{

            if (IsDestroyingCards())
			{
				DestroyCard();
				gameManager.cardsToDestroy--;
			}
            else if (IsSpecialCard())
			{
				gameManager.remainingMana -= cardCost;
				gameManager.ActivateEffect(cardEffect);

				DestroyCard();
			}
			else 
            {
				for(int i = 0; i < gameManager.availableSelectedCardSlots.Length; i++){
				
					if(gameManager.availableSelectedCardSlots[i]){
						
						Instantiate(hollowCircle, transform.position, Quaternion.identity);
			
						camAnim.SetTrigger("shake");
						anim.SetTrigger("move");

						transform.position = gameManager.selectedCardSlots[i].position;
						hasBeenPlayed = true;
						gameManager.availableCardSlots[tableIndex] = true;
						gameManager.availableSelectedCardSlots[i] = false;

					    if (gameManager.shouldUseHalfMana)
					    {
							double halfCost = (double)cardCost / 2;
					        gameManager.remainingMana -= ((int)Math.Ceiling(halfCost));

							gameManager.shouldUseHalfMana = false;
							gameManager.cardsOnHand++;
							return;
					    }

						gameManager.remainingMana -= cardCost;

						ScoreManager.Instance.AddPoints(this);
						
						gameManager.cardsOnHand++;

						if (gameManager.cardsOnHand == gameManager.handSize)
						{
							gameManager.CalculatePoints();
						}

						return;
					}

				}			
			}
        }
			
		else if (hasBeenPlayed)
		{
			Invoke(nameof(MoveToDiscardPile), 1f);
		}
	}

    private bool IsSpecialCard()
    {
		return cardEffect > 0;
    }
	private bool IsDestroyingCards()
	{
		return gameManager.cardsToDestroy > 0;
	}
	private void DestroyCard()
	{
		gameManager.availableCardSlots[tableIndex] = true;
		hasBeenPlayed = true;

		Invoke(nameof(MoveToDiscardPile), 1f);
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
		
		Instantiate(effect, transform.position, Quaternion.identity);
		gameManager.discardPile.Add(this);
		gameObject.SetActive(false);
		
	}

}