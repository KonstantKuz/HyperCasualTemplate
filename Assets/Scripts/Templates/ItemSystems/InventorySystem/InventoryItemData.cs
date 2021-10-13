using UnityEngine;

namespace Templates.ItemSystems.InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryItemData", menuName = "Item Systems/InventoryItemData")]
    public class InventoryItemData : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public bool UnlockedToUseByDefault;
    }
}