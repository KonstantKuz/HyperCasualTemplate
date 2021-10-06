using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Template.ItemSystems.ShopSystem.UI
{
    public class BuyButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _buyForBlock;
        [SerializeField] private TextMeshProUGUI _priceAmountText;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private GameObject _oneVideoBlock;

        [SerializeField] private CanvasGroup buttonGroup;

        public Action OnClicked;

        private void Awake()
        {
            _button.onClick.AddListener(delegate { OnClicked?.Invoke(); });
        }

        public void Show(CurrencyType currencyType, int priceAmount)
        {
            gameObject.SetActive(true);

            bool playerHasCurrency = PlayerWallet.Instance.HasCurrency(currencyType.ToString(), priceAmount);
            
            UpdatePriceView(currencyType, priceAmount, playerHasCurrency);
            SetButtonClickable(playerHasCurrency);
        }

        private void UpdatePriceView(CurrencyType currencyType, int priceAmount, bool playerHasCurrency)
        {
            bool priceIsOneVideo = priceAmount == 1;
            _buyForBlock.SetActive(!priceIsOneVideo);
            _oneVideoBlock.SetActive(priceIsOneVideo);
            
            _priceAmountText.color = playerHasCurrency ? Color.white : Color.red;
            _priceAmountText.SetText(priceAmount.ToString());
            _currencyImage.sprite = Shop.Instance.GetCurrencySprite(currencyType);
        }

        public void SetButtonClickable(bool hasCurrency)
        {
            buttonGroup.alpha = hasCurrency ? 1f : 0.5f;
            buttonGroup.interactable = hasCurrency;
        }
    }
}
