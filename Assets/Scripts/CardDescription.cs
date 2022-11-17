using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDescription : MonoBehaviour
{

    public TextMeshProUGUI cardCostText;
    public int cardCost;

    // Start is called before the first frame update
    void Start()
    {
        SetCardCost();
    }

    public void SetCardCost()
    {
        cardCostText.text = cardCost.ToString();
    }

}
