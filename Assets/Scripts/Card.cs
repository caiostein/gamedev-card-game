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
		if (!gameManager.isHandlingCards)
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

            if (IsDestroyingCards() && gameManager.table.Find(card => card == this))
            {
                hasBeenDrawn = true;
                DestroyCard();
                gameManager.cardsToDestroy--;
                Renderer cardRenderer = this.GetComponent<Renderer>();
                cardRenderer.material.color = Color.white;
                gameManager.clickSound.Play();
                if (gameManager.cardsToDestroy == 0)
                {
                    gameManager.DrawCard();
                    gameManager.DrawCard();
                    gameManager.DrawCard();
                    gameManager.SetDeckInteraction(true);
                    if (!gameManager.CheckPickAvailability() && !gameManager.CheckDrawAvailability())
                    {
                        gameManager.ToggleEarlyResultsBox(true);
                    }                        
                }
            }

            if (!hasBeenDrawn && costToUse <= gameManager.remainingMana)
            {
                if (IsSpecialCard())
                {
                    gameManager.remainingMana -= costToUse;
                    hasBeenDrawn = true;
                    gameManager.clickSound.Play();
                    DestroyCard();
                    gameManager.ActivateEffect(cardEffect);
                    
                    if (!IsDestroyingCards() && !gameManager.CheckPickAvailability() && !gameManager.CheckDrawAvailability())
                    {
                        gameManager.ToggleEarlyResultsBox(true);
                    }
                }
                else
                {
                    for (int i = 0; i < gameManager.availableHandSlots.Length; i++)
                    {

                        if (gameManager.availableHandSlots[i])
                        {

                            Instantiate(hollowCircle, transform.position, Quaternion.identity);

                            camAnim.SetTrigger("shake");
                            anim.SetTrigger("move");
                            anim.SetBool("onTable", false);

                            transform.position = gameManager.handSlots[i].position;
                            hasBeenDrawn = true;
                            gameManager.availableTableSlots[tableIndex] = true;
                            gameManager.availableHandSlots[i] = false;

                            gameManager.remainingMana -= costToUse;

                            gameManager.shouldUseHalfMana = false;

                            gameManager.clickSound.Play();

                            ScoreManager.Instance.AddPoints(this);

                            gameManager.table.Remove(this);
                            gameManager.hand.Add(this);

                            if (gameManager.hand.Count == Const.handSize)
                            {
                                gameManager.ToggleLevelResultBox(true);
                            } 

                            else if (!gameManager.CheckPickAvailability() && !gameManager.CheckDrawAvailability())
                            {
                                gameManager.ToggleEarlyResultsBox(true);
                            }                               

                            return;
                        }

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

            if (gameManager.drawnSpecialCards < 0)
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
        if (!gameManager.isHandlingCards)
        {
            gameManager.cardHoverSound.Play();
            anim.SetBool("hover", true);
            descriptionOnScreen = Instantiate(cardDescription, gameManager.descriptionSlot.transform.position, Quaternion.identity);
		    descriptionOnScreen.transform.localScale = new Vector3((float)0.827, (float)0.827, (float)0.827);
		    descriptionOnScreen.SetActive(true);

            Renderer cardRenderer = this.GetComponent<Renderer>();

            if (IsDestroyingCards())
            {    
                Renderer renderer = descriptionOnScreen.GetComponent<Renderer>();
                if (ColorUtility.TryParseHtmlString("#FF7777", out Color color))
                {
                    renderer.material.color = color;
                    cardRenderer.material.color = color;
                }
            }
            if (gameManager.shouldUseHalfMana)
            {
                Renderer renderer = descriptionOnScreen.GetComponent<Renderer>();
                if (ColorUtility.TryParseHtmlString("#7BCEE2", out Color color))
                {
                    renderer.material.color = color;
                    cardRenderer.material.color = color;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        Destroy(descriptionOnScreen);
        anim.SetBool("hover", false);
        Renderer cardRenderer = this.GetComponent<Renderer>();
        cardRenderer.material.color = Color.white;
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
