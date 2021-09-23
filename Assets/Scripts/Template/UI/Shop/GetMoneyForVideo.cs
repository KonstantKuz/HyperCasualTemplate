using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMoneyForVideo : MonoBehaviour
{
    [SerializeField] private int _moneyAmount;
    [SerializeField] private Button button;
    [SerializeField] private Transform coinsMoveFrom;
    [SerializeField] private Transform coinsMoveTo;

    private void Awake()
    {
        button.onClick.AddListener(TryGetMoneyForVideo);
    }

    private void TryGetMoneyForVideo()
    {
        AdsManager.Instance.onRewardedAdRewarded += delegate
        {
            CoinAnimator.Instance.SpawnMovingCoins(coinsMoveFrom, coinsMoveTo);
            PlayerWallet.Instance.IncreaseMoney(_moneyAmount);
        };
        
        AdsManager.Instance.ShowRewardedAd(RewardedVideoPlacement.Shop_Coins.ToString());
    }
}
