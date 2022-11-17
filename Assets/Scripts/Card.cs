using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public int handIndex;

	public int cost;
	public int valuePoints;

	GameManager gm;

	private Animator anim;
	private Animator camAnim;

	public GameObject effect;
	public GameObject hollowCircle;

	public GameObject cardDescription;

	private GameObject clone;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
		anim = GetComponent<Animator>();
		camAnim = Camera.main.GetComponent<Animator>();
	}
	private void OnMouseDown()
	{
		Debug.Log(gm.costPointsRemaining);
		if (!hasBeenPlayed && cost <= gm.costPointsRemaining) 
		{

			for(int i = 0; i < gm.availableSelectedCardSlots.Length; i++){
				
				if(gm.availableSelectedCardSlots[i]){
					
					Instantiate(hollowCircle, transform.position, Quaternion.identity);
			
					camAnim.SetTrigger("shake");
					anim.SetTrigger("move");

					transform.position = gm.selectedCardSlots[i].position;
					hasBeenPlayed = true;
					gm.availableCardSlots[handIndex] = true;
					gm.availableSelectedCardSlots[i] = false;

					gm.costPointsRemaining -= cost;
					gm.totalScore += valuePoints;

					return;
				}
			}
			
		}
		else if (hasBeenPlayed)
		{
			Invoke("MoveToDiscardPile", 1f);
		}
	}

    private void OnMouseEnter()
    {
		clone = Instantiate(cardDescription, gm.descriptionSlot.transform.position, Quaternion.identity);
		clone.transform.localScale = new Vector3((float)0.827, (float)0.827, (float)0.827);
		clone.SetActive(true);
    }

    private void OnMouseExit()
    {
		GameObject.Destroy(clone);
    }

    void MoveToDiscardPile()
	{
		
			Instantiate(effect, transform.position, Quaternion.identity);
			gm.discardPile.Add(this);
			gameObject.SetActive(false);
		
	}



}
