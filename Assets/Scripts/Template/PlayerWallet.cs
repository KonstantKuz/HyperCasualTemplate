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

    public void IncreaseMoney(int value)
    {
        ChangeMoney(value);
    }

    public void DecreaseMoney(int value)
    {
        ChangeMoney(-value);
    }

    private void ChangeMoney(int value)
    {
        int currentMoney = GetCurrentMoney();
        currentMoney += value;
        if (currentMoney <= 0 && value < 0)
        {
            Debug.Log("Player has no money.");
            return;
        }
        PlayerPrefs.SetInt(PrefsMoney, currentMoney);
        Instance.OnMoneyChanged?.Invoke();
    }

    public bool HasMoney(int amount)
    {
        return GetCurrentMoney() >= amount;
    }

    public int GetCurrentMoney()
    {
        return PlayerPrefs.GetInt(PrefsMoney, Instance.defaultMoney);
    }
}