using UnityEngine;

[System.Serializable]
public class ProgressiveItemData
{
    public string _itemName;
    public bool _unlockedByDefault;
    public bool _unlockCompletely;
    public int _progressToUnlock;
    public int _cost;
    public Sprite _icon;
}