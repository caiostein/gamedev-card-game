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

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.shouldUseHalfMana)
        {
            FormatHalfManaText();
            double halfCost = (double)cardCost / 2;
            SetCardCost((int)Math.Floor(halfCost));
        } else
        {
            SetCardCost(cardCost);
        }
    }

    public void SetCardCost(int costToSet)
    {
        cardCostText.text = costToSet.ToString();       
    }

    private void FormatHalfManaText()
    {
        cardCostText.colorGradient = new VertexGradient(Color.cyan);
        cardCostText.outlineColor = Color.black;
        cardCostText.outlineWidth = (float)0.2;
    }

}
