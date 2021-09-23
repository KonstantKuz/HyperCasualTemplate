using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        PlayerWallet.Instance.OnMoneyChanged += UpdateMoneyTextInstantly;
        UpdateMoneyTextInstantly();
    }

    public void UpdateMoneyTextInstantly()
    {
        moneyText.SetText(PlayerWallet.Instance.GetCurrentMoney().ToString());
    }
}
