using System.Collections.Generic;
using UnityEngine;

namespace Template.ProgressiveItemsHandler
{
    [CreateAssetMenu(fileName = "ItemsProgression")]
    public class ItemsProgression : ScriptableObject
    {
        [Header("Обновлять ли прогресс айтемов вручную")]
        public bool manualUpdate;
        [Header("Обновлять ли прогресс айтемов параллельно (если нет - разблокировка айтемов будет происходить последовательно)")]
        public bool parallelUpdate;
        public string progressionName;
        public List<ProgressiveItemData> itemsQueue;
    }
}