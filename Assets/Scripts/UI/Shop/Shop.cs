using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static int Money = 50;

    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        OpenShop();
    }

    public void OpenShop()
    {
        UpdateMoney();
    }

    public void CloseShop()
    {

    }

    public void BuyItem(BuyableItem sender)
    {
        if (sender.Cost <= Money && !sender.Bought)
        {
            Debug.Log("buy " + sender.Name);
            Money -= sender.Cost;
            sender.Bought = true;
            moneyText.text = "Bought!";
            UpdateMoney();
        }
    }

    private void UpdateMoney()
    {
        moneyText.text = "$ " + Money;
    }
}
