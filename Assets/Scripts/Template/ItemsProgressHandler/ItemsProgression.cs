using UnityEngine;

[CreateAssetMenu(fileName = "ItemsProgression")]
public class ItemsProgression : ScriptableObject
{
    public string progressionName;
    public ProgressiveItemData[] itemsQueue;
}