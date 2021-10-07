using System;
using Templates.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.UI.RewardMultiplier
{
    public class RewardMultiplierPanel : MonoBehaviour
    {
        [SerializeField] private RewardedMultiplierIndicator multiplierIndicator;

        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private Button getRewardButton;
        [SerializeField] private float showNoThanksDelay;
        [SerializeField] private Button noThanksButton;

        private int _rewardForCurrentLevel;
        private int _currentMultiplierValue;
        private Action _onRewardMultiplied;
        private Action _onRewardDiscarded;
    
        public void StartCount(int rewardForCurrentLevel, Action onRewardMultiplied, Action onRewardDiscarded)
        {
            multiplierIndicator.gameObject.SetActive(true);
            
            _rewardForCurrentLevel = rewardForCurrentLevel;
            _onRewardMultiplied = onRewardMultiplied;
            _onRewardDiscarded = onRewardDiscarded;
        
            multiplierIndicator.StartCount(UpdateMultiplierValue);
            
            getRewardButton.onClick.AddListener(TryMultiplyReward);
        
            DelayHandler.Instance.DelayedCallCoroutine(showNoThanksDelay, delegate
            {
                noThanksButton.gameObject.SetActive(true);
                noThanksButton.onClick.AddListener(GetNormalReward);
            });
        }

        private void UpdateMultiplierValue(int value)
        {
            _currentMultiplierValue = value;
            coinsText.SetText((_rewardForCurrentLevel * _currentMultiplierValue).ToString());
        }
        
        private void TryMultiplyReward()
        {
            getRewardButton.gameObject.SetActive(false);
            multiplierIndicator.StopCount();
            
            AdsManager.Instance.onRewardedAdRewarded += GetMultipliedReward;
            AdsManager.Instance.onRewardedAdFailedOrDiscarded += GetNormalReward;
        
            AdsManager.Instance.ShowRewardedAd(RewardedVideoPlacement.CoinMultiplicator.ToString());
        }

        private void GetNormalReward()
        {
            PlayerWallet.Instance.IncreaseCurrency(CurrencyType.Coin.ToString(), _rewardForCurrentLevel);
            _onRewardDiscarded?.Invoke();
            Close();
        }

        private void GetMultipliedReward()
        {
            PlayerWallet.Instance.IncreaseCurrency(CurrencyType.Coin.ToString(), _rewardForCurrentLevel * _currentMultiplierValue);
            _onRewardMultiplied?.Invoke();
            Close();
        }

        private void Close()
        {
            multiplierIndicator.gameObject.SetActive(false);
        }
    }
}