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

        [SerializeField] private Transform coinsMoveFrom;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private Button getRewardButton;
        [SerializeField] private CanvasGroup getRewardButtonCanvas;
        [SerializeField] private float showNoThanksDelay;
        [SerializeField] private Button noThanksButton;
     
        private Action _onPanelClosed;

        private int _rewardForCurrentLevel;
        private int _currentMultiplierValue;

        private float ShowCoinsDelay = 2f;
    
        public void ShowPanel(int rewardForCurrentLevel, Action onPanelClosed)
        {
            _onPanelClosed = onPanelClosed;

            multiplierIndicator.gameObject.SetActive(true);
            
            _rewardForCurrentLevel = rewardForCurrentLevel;
        
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
            getRewardButtonCanvas.alpha = 0.5f;
            getRewardButtonCanvas.interactable = false;
            
            multiplierIndicator.StopCount();
            
            AdsManager.Instance.onRewardedAdRewarded += GetMultipliedReward;
            AdsManager.Instance.onRewardedAdFailedOrDiscarded += GetNormalReward;
        
            AdsManager.Instance.ShowRewardedAd(RewardedVideoPlacement.CoinMultiplicator.ToString());
        }

        private void GetNormalReward()
        {
            PlayerWallet.Instance.IncreaseCurrency(CurrencyType.Coin.ToString(), _rewardForCurrentLevel);
            Close();
        }

        private void GetMultipliedReward()
        {
            PlayerWallet.Instance.IncreaseCurrency(CurrencyType.Coin.ToString(), _rewardForCurrentLevel * _currentMultiplierValue);
            CoinAnimator.Instance.SpawnMovingCoins(coinsMoveFrom, CurrencyViewer.Instance.CoinImage);
            DelayHandler.Instance.DelayedCallCoroutine(ShowCoinsDelay, Close);
        }

        private void Close()
        {
            multiplierIndicator.gameObject.SetActive(false);
            _onPanelClosed?.Invoke();
        }
    }
}