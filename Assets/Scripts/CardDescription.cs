using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDescription : MonoBehaviour
{
    GameManager gameManager;

    public TextMeshProUGUI cardCostText;
    public int cardCost;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.shouldUseHalfMana)
        {
            double halfCost = cardCost / 2;
            SetCardCost((int)Math.Ceiling(halfCost));
        } else
        {
            SetCardCost(cardCost);
        }
    }

    public void SetCardCost(int costToSet)
    {
        cardCostText.text = costToSet.ToString();
    }

}
