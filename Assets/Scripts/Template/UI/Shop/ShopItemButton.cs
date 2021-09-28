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

    private ProgressiveItemContainer _item;
    public ProgressiveItemContainer Item => _item;
    
    private int _unlockLevel;

    private void Awake()
    {
        button.onClick.AddListener(delegate { OnClicked.Invoke(this); });
    }

    public void Initialize(ProgressiveItemContainer item, Action<ShopItemButton> onClicked)
    {
        _item = item;
        
        gameObject.SetActive(true);
        gameObject.name = _item.Name();
        itemImage.sprite = _item.Icon();

        OnClicked += onClicked;
    }

    public void UpdateStatus(bool updateEquipStatus, int unlockLevel)
    {
        if (updateEquipStatus)
        {
            SetEquipped(_item.IsEquipped());
        }
        
        if (!_item.IsUnlockedToUseByDefault())
        {
            SetAsNew(_item.IsNewNotViewedInShop());
        }

        UpdateItemAvailableStatus(unlockLevel);
    }
    
    public void UpdateItemAvailableStatus(int unlockLevel)
    {
        _unlockLevel = unlockLevel;
        
        if (_item.IsUnlockedToShop() && _item.IsUnlockedToUse())
        {
            SetAvailableStatus(AvailableStatus.AvailableToUse);
        }
        else if (_item.IsUnlockedToShop() && !_item.IsUnlockedToUse())
        {
            SetAvailableByCostType(_item.PriceType());
        }
        else if (!_item.IsUnlockedToShop())
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
        lockedLevelText.text = $"UNLOCK AT LEVEL {_unlockLevel}";
        
        costText.gameObject.SetActive(status == AvailableStatus.UnlocksForCoins);
        costText.color = PlayerWallet.Instance.GetCurrentMoney() >= _item.Price() ? Color.white : Color.red;
        costText.text = $"{_item.Price()}";
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
