using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Get1000 : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Transform coinsMoveFrom;
    [SerializeField] private Transform coinsMoveTo;

    private void Awake()
    {
        button.onClick.AddListener(TryGet1000ForReward);
    }

    private void TryGet1000ForReward()
    {
        PlayerWallet.Instance.IncreaseMoney(1000);
        // CoinAnimator.Instance.SpawnMovingCoins(coinsMoveFrom, coinsMoveTo, 1f);
        // PlayerWallet.Instance.IncreaseMoney(1000);
        
        // ADManager.Instance.onRewardedAdRewarded += delegate
        // {
        //     CoinAnimator.Instance.SpawnMovingCoins(coinsMoveFrom, coinsMoveTo, 1f);
        //     PlayerWallet.Instance.IncreaseMoney(1000);
        // };
        //
        // ADManager.Instance.ShowRewardedAd(RewardedVideoPlacement.Shop_Coins);
    }
}
