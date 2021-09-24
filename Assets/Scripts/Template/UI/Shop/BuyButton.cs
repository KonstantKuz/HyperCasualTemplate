using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public enum PriceStatus
{
    Video,
    Coins,
}

public class BuyButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject coinsCost;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject videoCost;
    [SerializeField] private CanvasGroup buttonGroup;

    public Action OnClicked;

    private void Awake()
    {
        button.onClick.AddListener(delegate { OnClicked?.Invoke(); });
    }

    public void ShowButtonWithVideoCost()
    {
        UpdatePriceStatus(PriceStatus.Video);
    }

    public void ShowButtonWithCoinsCost(int price)
    {
        UpdatePriceStatus(PriceStatus.Coins);
        
        bool canBeBought = PlayerWallet.Instance.GetCurrentMoney() >= price;
        costText.color = canBeBought ? Color.white : Color.red;
        costText.SetText($"{price}");
        buttonGroup.alpha = canBeBought ? 1f : 0.5f;
        buttonGroup.interactable = canBeBought;
    }

    private void UpdatePriceStatus(PriceStatus priceStatus)
    {
        gameObject.SetActive(true);
        videoCost.SetActive(priceStatus == PriceStatus.Video);
        coinsCost.SetActive(priceStatus == PriceStatus.Coins);
    }
}
