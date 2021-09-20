using System;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        button.onClick.AddListener(delegate { OnClicked?.Invoke(this); });
    }

    public void SetItemSprite(Sprite itemSprite)
    {
        itemImage.sprite = itemSprite;
    }

    public void SetItemAvailable()
    {
        availableImage.SetActive(true);
        
        videoImage.SetActive(false);
        lockedImage.SetActive(false);
        costText.gameObject.SetActive(false);
    }
    public void SetItemAvailableForVideo()
    {
        videoImage.SetActive(true);
        
        lockedImage.SetActive(false);
    }
    public void SetItemAvailableForLevel(int level)
    {
        lockedImage.SetActive(true);
        lockedLevelText.text = $"UNLOCK AT LEVEL {level}";
        // lockedLevelText.text = LocalizationManager.Localize(GameConstants.LK_StoreUnlockAtLevel);
        // lockedLevelText.text = lockedLevelText.text.Replace("[Number]", $"{level}");
    }
    public void SetItemAvailableForCoins(int cost, bool canBeBought)
    {
        costText.gameObject.SetActive(true);
        costText.color = canBeBought ? Color.white : Color.red;
        costText.text = $"{cost}";
    }
    
    public void SetAsNewUncheckedItem(bool value)
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
