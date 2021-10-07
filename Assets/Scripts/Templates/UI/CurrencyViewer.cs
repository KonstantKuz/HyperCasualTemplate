using TMPro;
using UnityEngine;

namespace Templates.UI
{
    public class CurrencyViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;

        private void Start()
        {
            PlayerWallet.Instance.OnCurrencyChanged += UpdateCoinsTextInstantly;
            UpdateCoinsTextInstantly();
        }

        public void UpdateCoinsTextInstantly()
        {
            string currentCoins = PlayerWallet.Instance.GetCurrencyCurrentValue(CurrencyType.Coin.ToString()).ToString();
            coinsText.SetText(currentCoins);
        }
    }
}
