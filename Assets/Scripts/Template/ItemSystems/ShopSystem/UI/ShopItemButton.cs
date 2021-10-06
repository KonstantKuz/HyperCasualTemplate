using System;
using Template.ItemSystems.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Template.ItemSystems.ShopSystem.UI
{
    public enum AvailableStatus
    {
        Locked,
        AvailableToUse,
        AvailableToShopForCurrency
    }
    
    public class ShopItemButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image itemImage;

        [SerializeField] private GameObject availableImage;
        [SerializeField] private GameObject selectedImage;
        [SerializeField] private GameObject equippedImage;

        [SerializeField] private Image _lockedImage;

        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _priceAmountText;
        
        [SerializeField] private Image isNewItemImage;
    
        public Action<ShopItemButton> OnClicked;

        private ShopItem _shopItem;
        private InventoryItem _inventoryItem;

        public bool IsUnlockedToShop => _shopItem.IsUnlockedToShop();
        public bool IsUnlockedToUse => _inventoryItem.IsUnlockedToUse();
        
        private void Awake()
        {
            button.onClick.AddListener(delegate { OnClicked.Invoke(this); });
        }

        public void Initialize(ShopItemData itemData, Action<ShopItemButton> onClicked)
        {
            gameObject.SetActive(true);

            _shopItem = Shop.Instance.GetItem(itemData.InventoryData.Name);
            _inventoryItem = Inventory.Instance.GetItem(itemData.InventoryData.Name);
            
            gameObject.name = _shopItem.Name;
            itemImage.sprite = _shopItem.Icon;

            OnClicked += onClicked;
        }

        public void UpdateStatus(bool updateEquipStatus)
        {
            if (updateEquipStatus)
            {
                SetEquipped(_inventoryItem.IsEquipped());
            }
        
            if (!_shopItem.IsUnlockedToShopByDefault() && !_inventoryItem.IsUnlockedToUseByDefault())
            {
                SetViewedInShop(_shopItem.IsFreshInShop());
            }

            UpdateItemAvailableStatus();
        }
    
        public void UpdateItemAvailableStatus()
        {
            if (_inventoryItem.IsUnlockedToUse())
            {
                SetAvailableStatus(AvailableStatus.AvailableToUse);
            }
            else if (_shopItem.IsUnlockedToShop() && !_inventoryItem.IsUnlockedToUse())
            {
                SetAvailableStatus(AvailableStatus.AvailableToShopForCurrency);
            }
            else if (!_shopItem.IsUnlockedToShop())
            {
                SetAvailableStatus(AvailableStatus.Locked);
            }
        }

        private void SetAvailableStatus(AvailableStatus status)
        {
            availableImage.SetActive(status == AvailableStatus.AvailableToUse);
            _lockedImage.gameObject.SetActive(status == AvailableStatus.Locked);
        
            _currencyImage.gameObject.SetActive(status == AvailableStatus.AvailableToShopForCurrency);
            bool playerHasCurrency = PlayerWallet.Instance.HasCurrency(_shopItem.CurrencyType.ToString(), _shopItem.PriceAmount);
            _priceAmountText.color = playerHasCurrency ? Color.white : Color.red;
            _priceAmountText.SetText($"{_shopItem.PriceAmount}");
            _currencyImage.sprite = Shop.Instance.GetCurrencySprite(_shopItem.CurrencyType);
            
            HandleVideoPrice();
        }

        private void HandleVideoPrice()
        {
            bool priceIsOneVideo = _shopItem.PriceAmount == 1;
            _priceAmountText.gameObject.SetActive(!priceIsOneVideo);

            // if (priceIsOneVideo)
            // {
            //     return;
            // }
            
            // int leftVideoCount = _shopItem.PriceAmount -
            //                      PlayerWallet.Instance.GetCurrencyCurrentValue(_shopItem.Name + CurrencyType.Video);
            // _priceAmountText.SetText(leftVideoCount.ToString());
        }
    
        public void SetViewedInShop(bool value)
        {
            isNewItemImage.gameObject.SetActive(value);
        }

        public void SetEquipped(bool value)
        {
            equippedImage.SetActive(value);
        }

        public void SetSelected(bool value)
        {
            selectedImage.SetActive(value);
        }
    }
}
