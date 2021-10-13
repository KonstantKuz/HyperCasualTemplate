using TMPro;
using UnityEngine;

namespace Templates.UI
{
    public class CurrencyViewer : Singleton<CurrencyViewer>
    {
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private Transform _coinImage;
        public Transform CoinImage => _coinImage;
        
        private void Start()
        {
            PlayerWallet.Instance.OnCurrencyChanged += UpdateCoinsTextInstantly;
            UpdateCoinsTextInstantly();
        }

        public void UpdateCoinsTextInstantly()
        {
            string currentCoins = PlayerWallet.Instance.GetCurrencyCurrentValue(CurrencyType.Coin.ToString()).ToString();
            _coinsText.SetText(currentCoins);
        }
    }
}
