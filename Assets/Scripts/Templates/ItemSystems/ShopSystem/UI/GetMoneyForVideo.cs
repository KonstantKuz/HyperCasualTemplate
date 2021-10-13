using Templates.Ads;
using Templates.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.ItemSystems.ShopSystem.UI
{
    public class GetMoneyForVideo : MonoBehaviour
    {
        [SerializeField] private int _moneyAmount;
        [SerializeField] private Button button;
        [SerializeField] private Transform coinsMoveFrom;

        private void Awake()
        {
            button.onClick.AddListener(TryGetMoneyForVideo);
        }

        private void TryGetMoneyForVideo()
        {
            AdsManager.Instance.onRewardedAdRewarded += delegate
            {
                CoinAnimator.Instance.SpawnMovingCoins(coinsMoveFrom, CurrencyViewer.Instance.CoinImage);
                PlayerWallet.Instance.IncreaseCurrency(CurrencyType.Coin.ToString(), _moneyAmount);
            };
        
            AdsManager.Instance.ShowRewardedAd(RewardedVideoPlacement.Shop_Coins.ToString());
        }
    }
}
