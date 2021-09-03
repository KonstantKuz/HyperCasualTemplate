using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : Singleton<PlayerWallet>
{
    [SerializeField] private int defaultMoney;
    public Action OnMoneyChanged;
    
    private const string PrefsMoney = "Money";
    
    private void Awake()
    {
        OnMoneyChanged();
    }

    public static void IncreaseMoney(int value)
    {
        int currentMoney = GetCurrentMoney();
        currentMoney += value;
        PlayerPrefs.SetInt(PrefsMoney, currentMoney);
        Instance.OnMoneyChanged?.Invoke();
    }

    public static void DecreaseMoney(int value)
    {
        int currentMoney = GetCurrentMoney();
        currentMoney -= value;
        PlayerPrefs.SetInt(PrefsMoney, currentMoney);
        Instance.OnMoneyChanged();
    }

    public static int GetCurrentMoney()
    {
        return PlayerPrefs.GetInt(PrefsMoney, Instance.defaultMoney);
    }
}