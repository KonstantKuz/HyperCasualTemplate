using UnityEngine;

[CreateAssetMenu(fileName = "ItemsProgression")]
public class ItemsProgression : ScriptableObject
{
    [Header("Обновлять ли прогресс айтемов вручную")]
    public bool _manualUpdate;
    [Header("Обновлять ли прогресс айтемов параллельно (если нет - разблокировка айтемов будет происходить последовательно)")]
    public bool _parallelUpdate;
    public string _progressionName;
    public ProgressiveItemData[] _itemsQueue;
}