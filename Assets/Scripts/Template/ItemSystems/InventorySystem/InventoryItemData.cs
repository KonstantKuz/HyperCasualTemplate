using UnityEngine;

namespace Template.ItemSystems.InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryItemData", menuName = "Item Systems/InventoryItemData")]
    public class InventoryItemData : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public bool UnlockedToUseByDefault;
    }
}