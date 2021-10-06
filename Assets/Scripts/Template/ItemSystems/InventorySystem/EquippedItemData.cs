namespace Template.ItemSystems.InventorySystem
{
    public class EquippedItemData
    {
        public readonly string GroupName;
        public readonly string Name;

        public EquippedItemData(string groupName, string name)
        {
            GroupName = groupName;
            Name = name;
        }
    }
}