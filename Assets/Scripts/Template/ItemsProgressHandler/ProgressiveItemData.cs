using UnityEngine;

[System.Serializable]
public class ProgressiveItemData
{
    public string _itemName;
    public bool _unlockedByDefault;
    public int _levelsCountToGet;
    public bool _unlockOnlyForLevels;
    public int _cost;
    public Sprite _icon;
}