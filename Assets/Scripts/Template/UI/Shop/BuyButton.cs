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

    public void SubscribeOnClick(Action onClicked)
    {
        button.onClick.AddListener(onClicked.Invoke);
    }

    public void UnsubscribeFromClick(Action onClicked)
    {
        button.onClick.RemoveListener(onClicked.Invoke);
    }

    public void ShowButtonWithVideoCost()
    {
        gameObject.SetActive(true);
        videoCost.SetActive(true);

        // button.onClick.RemoveAllListeners();
        // button.onClick.AddListener(delegate
        // {
        //     OnButtonClick(onClicked);
        //     gameObject.SetActive(false);
        // });
    }

    public void ShowButtonWithCoinsCost(int cost, bool canBeBought)
    {
        gameObject.SetActive(true);
        coinsCost.SetActive(true);
        costText.color = canBeBought ? Color.white : Color.red;
        costText.SetText($"{cost}");
        buttonGroup.alpha = canBeBought ? 1f : 0.5f;
        buttonGroup.interactable = canBeBought;
        
        // button.onClick.RemoveAllListeners();
        // button.onClick.AddListener(delegate
        // {
        //     gameObject.SetActive(false);
        // });
    }
}
