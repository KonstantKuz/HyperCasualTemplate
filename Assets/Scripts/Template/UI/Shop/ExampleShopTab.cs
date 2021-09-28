using System.Collections.Generic;
using System.Linq;
using Template.ProgressiveItemsHandler;
using UnityEngine;
using UnityEngine.UI;

namespace Template.UI.Shop
{
    public class ExampleShopTab : UniversalShopTab
    {
        [SerializeField] private Image _itemView;

        public override void Awake()
        {
            base.Awake();

            List<ProgressiveItemContainer> itemContainers = ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary.Values.ToList();
            itemContainers[0].SetAsEquipped();
        }
    
        public override void OnItemSelected(string itemName)
        {
            _itemView.gameObject.SetActive(true);
            _itemView.sprite = ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemName].Icon();
            print($"OnItemSelected {itemName}");
        }

        public override void OnEquipItem(string itemName)
        {
            ProgressiveItemsHandler.ProgressiveItemsHandler.Instance.ItemsReadOnlyDictionary[itemName].SetAsEquipped();
            print($"OnEquipItem {itemName}");
        }

        public override void OnBuyItem(string itemName)
        {
            print($"OnBuyItem {itemName}");
        }
    }
}
