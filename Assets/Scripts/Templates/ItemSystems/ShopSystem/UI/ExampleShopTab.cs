using Templates.ItemSystems.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.ItemSystems.ShopSystem.UI
{
    public class ExampleShopTab : UniversalShopTab
    {
        [SerializeField] private Image _itemView;

        public override void Awake()
        {
            base.Awake();

            // List<InventoryItem> itemContainers = Inventory.Instance.GetItem();
            // itemContainers[0].SetAsEquipped();
        }
    
        public override void OnItemSelected(string itemName)
        {
            _itemView.gameObject.SetActive(true);
            _itemView.sprite = Inventory.Instance.GetItem(itemName).Icon;
            
            
            print($"OnItemSelected {itemName}");
        }

        public override void OnEquipItem(string itemName)
        {
            Inventory.Instance.GetItem(itemName).SetAsEquipped();
            print($"OnEquipItem {itemName}");
        }

        public override void OnBuyItem(string itemName)
        {
            print($"OnBuyItem {itemName}");
        }
    }
}
