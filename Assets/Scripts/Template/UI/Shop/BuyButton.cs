using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject coinsCost;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject videoCost;
    [SerializeField] private CanvasGroup buttonGroup;

    public void ShowButtonWithVideoCost(Action onClicked)
    {
        gameObject.SetActive(true);
        videoCost.SetActive(true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate
        {
            onClicked();
            gameObject.SetActive(false);
        });
    }

    public void ShowButtonWithCoinsCost(Action onClicked, int cost, bool canBeBought)
    {
        gameObject.SetActive(true);
        coinsCost.SetActive(true);
        costText.color = canBeBought ? Color.white : Color.red;
        costText.SetText($"{cost}");
        buttonGroup.alpha = canBeBought ? 1f : 0.5f;
        buttonGroup.interactable = canBeBought;
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate
        {
            onClicked();
            gameObject.SetActive(false);
        });
    }
}
