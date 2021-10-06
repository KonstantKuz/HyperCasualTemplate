using System.Collections.Generic;
using UnityEngine;

namespace Template.ItemSystems.ShopSystem
{
    [CreateAssetMenu(fileName = "ShopItemsGroup", menuName = "Item Systems/ShopItemsGroup")]
    public class ShopItemsGroup : ScriptableObject
    {
        public string Name;
        public List<ShopItemData> Items;
    }
}