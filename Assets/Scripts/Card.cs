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
			Instantiate(hollowCircle, transform.position, Quaternion.identity);
			
			camAnim.SetTrigger("shake");
			anim.SetTrigger("move");

			transform.position += Vector3.down * 3;
			hasBeenPlayed = true;
			gm.availableCardSlots[handIndex] = true;

			gm.costPointsRemaining -= cost;
			gm.totalScore += valuePoints;
			
		}
		else if (hasBeenPlayed)
		{
			Invoke("MoveToDiscardPile", 1f);
		}
	}

	void MoveToDiscardPile()
	{
		
			Instantiate(effect, transform.position, Quaternion.identity);
			gm.discardPile.Add(this);
			gameObject.SetActive(false);
		
	}



}
