using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
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
				hasBeenPlayed = true;
				DestroyCard();
				gameManager.cardsToDestroy--;
			}
            else if (IsSpecialCard())
			{
				gameManager.remainingMana -= cardCost;
				gameManager.ActivateEffect(cardEffect);
				hasBeenPlayed = true;
				DestroyCard();
			}
			else 
            {
				for(int i = 0; i < gameManager.availableHandSlots.Length; i++){
				
					if(gameManager.availableHandSlots[i]){
						
						Instantiate(hollowCircle, transform.position, Quaternion.identity);
			
						camAnim.SetTrigger("shake");
						anim.SetTrigger("move");

						transform.position = gameManager.handSlots[i].position;
						hasBeenPlayed = true;
						gameManager.availableTableSlots[tableIndex] = true;
						gameManager.availableHandSlots[i] = false;

					    if (gameManager.shouldUseHalfMana)
					    {
							double halfCost = (double)cardCost / 2;
					        gameManager.remainingMana -= ((int)Math.Ceiling(halfCost));

							gameManager.shouldUseHalfMana = false;

							gameManager.table.Remove(this);
							gameManager.hand.Add(this);

							Debug.Log(this.cardCost);

							return;
					    }

						gameManager.remainingMana -= cardCost;

						ScoreManager.Instance.AddPoints(this);
						
						gameManager.table.Remove(this);
						gameManager.hand.Add(this);
						Debug.Log(this.cardCost);

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

    private bool IsSpecialCard()
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

		if (hasBeenPlayed)
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