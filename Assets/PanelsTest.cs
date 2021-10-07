using System;
using System.Collections;
using System.Collections.Generic;
using Templates;
using Templates.ItemSystems.GiftSystem.UI;
using Templates.UI.RewardMultiplier;
using UnityEngine;

public class PanelsTest : MonoBehaviour
{
    [SerializeField] private RewardMultiplierPanel _rewardMultiplierPanel;
    [SerializeField] private GiftProgressUpdatePanel _giftProgressUpdatePanel;
    private void Awake()
    {
        Observer.Instance.OnWinLevel += ShowMultiplierPanel;
    }

    private void ShowMultiplierPanel()
    {
        _rewardMultiplierPanel.ShowPanel(125, ShowGiftPanel);
    }

    private void ShowGiftPanel()
    {
        _giftProgressUpdatePanel.ShowPanel(Observer.Instance.OnLoadNextScene);
    }
}
