using System;
using UnityEngine;
using UnityEngine.UI;

public enum AvailableStatus
{
    AvailableToUse,
    UnlocksAtLevel,
    UnlocksForVideo,
    UnlocksForCoins,
}

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject availableImage;
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private GameObject equipedImage;
    [SerializeField] private GameObject videoImage;
    [SerializeField] private GameObject lockedImage;
    [SerializeField] private Text lockedLevelText;
    [SerializeField] private Text costText;
    [SerializeField] private GameObject isNewItemImage;
    
    public Action<ShopItemButton> OnClicked;

    private int _unlocksAtLevel;
    private int _price;

    private void Awake()
    {
        button.onClick.AddListener(delegate { OnClicked?.Invoke(this); });
    }

    public void Initialize(ProgressiveItemData itemData)
    {
        gameObject.SetActive(true);
        gameObject.name = itemData.itemName;

        itemImage.sprite = itemData.icon;
    }

    public void UpdateItemAvailableStatus(ProgressiveItemContainer item, int unlocksAtLevel)
    {
        _price = item.Price();
        _unlocksAtLevel = unlocksAtLevel;
        
        if (item.IsUnlockedToShop() && item.IsUnlockedToUse())
        {
            SetAvailableStatus(AvailableStatus.AvailableToUse);
        }
        else if (item.IsUnlockedToShop() && !item.IsUnlockedToUse())
        {
            SetAvailableByCostType(item.PriceType());
        }
        else if (!item.IsUnlockedToShop())
        {
            SetAvailableStatus(AvailableStatus.UnlocksAtLevel);
        }
    }

    private void SetAvailableByCostType(ItemPriceType priceType)
    {
        switch (priceType)
        {
            case ItemPriceType.Video:
                SetAvailableStatus(AvailableStatus.UnlocksForVideo);
                break;
            case ItemPriceType.Coins:
                SetAvailableStatus(AvailableStatus.UnlocksForCoins);
                break;
        }
    }
    
    private void SetAvailableStatus(AvailableStatus status)
    {
        availableImage.SetActive(status == AvailableStatus.AvailableToUse);
        
        videoImage.SetActive(status == AvailableStatus.UnlocksForVideo);
        
        lockedImage.SetActive(status == AvailableStatus.UnlocksAtLevel);
        lockedLevelText.gameObject.SetActive(status == AvailableStatus.UnlocksAtLevel);
        lockedLevelText.text = $"UNLOCK AT LEVEL {_unlocksAtLevel}";
        
        costText.gameObject.SetActive(status == AvailableStatus.UnlocksForCoins);
        costText.color = PlayerWallet.Instance.GetCurrentMoney() >= _price ? Color.white : Color.red;
        costText.text = $"{_price}";
    }
    
    public void SetAsNew(bool value)
    {
        isNewItemImage.SetActive(value);
    }

    public void SetEquipped(bool value)
    {
        equipedImage.SetActive(value);
    }

    public void SetSelected(bool value)
    {
        selectedImage.SetActive(value);
    }
}
