using System;
using Templates.LevelManagement;
using Templates.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.UI
{
    public class GlobalLevelProgressBar : SceneLineProgressBar<GlobalLevelProgressBar>
    {
        [SerializeField] private GlobalLevelProgressBarBlock[] progressBlocks;

        private InitialData<GlobalLevelProgressBar> initialData;
        private static UpdateData<GlobalLevelProgressBar> updateData;

        private PlayerPrefsProperty<int> updateBarProgress = new PlayerPrefsProperty<int>("UpdateBarProgress", 1);
        private PlayerPrefsProperty<int> passedLevelsCount = new PlayerPrefsProperty<int>("PassedLevelsCount", 1);
        private PlayerPrefsProperty<int> firstBlockLevelNumber = new PlayerPrefsProperty<int>("FirstDisplayLevelNumber", 1);
    
        private float[] progressToInitValue = { 0f, 0.14f, 0.355f, 0.57f, 0.785f, 1f };
        private float[] progressToHalfValue = { 0f, 0.185f, 0.4f, 0.61f, 0.825f, 1f };
    
        private void Start()
        {
            if (updateBarProgress.Value >= 6)
            {
                // Не сбрасывается до 1, а сдвигается на 5 для случая "перескока" через один или более уровней
                // Например при отказе проходить бонус уровень
                updateBarProgress.Value -= 5;
                passedLevelsCount.Value += 5;
                firstBlockLevelNumber.Value = LevelManager.Instance.CurrentDisplayLevelNumber;
            }

            LevelManager levelManager = LevelManager.Instance;
            int blockLevelNumber = firstBlockLevelNumber.Value;
            for (int i = 0; i < progressBlocks.Length; i++)
            {
                GlobalLevelProgressBarBlock progressBlock = progressBlocks[i];
            
                int levelIndex = levelManager.GetLevelIndexFromLevelsCount(passedLevelsCount.Value + i);
                if (levelManager.IsBossLevel(levelIndex))
                {
                    progressBlock.SwitchDisplayType(BlockDisplayType.DisplayBoss);
                }
                else if (levelManager.IsBonusLevel(levelIndex))
                {
                    progressBlock.SwitchDisplayType(BlockDisplayType.DisplayBonus);
                
                    blockLevelNumber--;
                }
                else
                {
                    progressBlock.SwitchDisplayType(BlockDisplayType.DisplayLevel);
                    progressBlock.levelText.SetText(blockLevelNumber.ToString());
                }

                blockLevelNumber++;
            }

            InitializeProgress(updateBarProgress.Value);
        
            Observer.Instance.OnStartGame += delegate
            {
                SetHalfProgress(updateBarProgress.Value);
            };
        
            Observer.Instance.OnLoseLevel += delegate { gameObject.SetActive(false); };
            Observer.Instance.OnWinLevel += delegate { gameObject.SetActive(false); };

            Observer.Instance.OnLoadNextScene += IncreaseGlobalProgress;
        }

        private void InitializeProgress(int globalProgress)
        {
            initialData.CurrentValue = progressToInitValue[globalProgress];
            initialData.MinValue = 0;
            initialData.MaxValue = 1f;
            Initialize(initialData);   
        }

        private void SetHalfProgress(int globalProgress)
        {
            updateData.CurrentValue = progressToHalfValue[globalProgress];
            UpdateProgress(updateData);
        }
    
        public void IncreaseGlobalProgress()
        {
            updateBarProgress.Value++;
        }
    
        [Serializable]
        private class GlobalLevelProgressBarBlock
        {
            public TextMeshProUGUI levelText;
            public Image bossImage;
            public Image bonusImage;

            public void SwitchDisplayType(BlockDisplayType displayType)
            {
                levelText.gameObject.SetActive(displayType == BlockDisplayType.DisplayLevel);
                bossImage.gameObject.SetActive(displayType == BlockDisplayType.DisplayBoss);
                bonusImage.gameObject.SetActive(displayType == BlockDisplayType.DisplayBonus);
            }
        }
    
        private enum BlockDisplayType
        {
            DisplayLevel,
            DisplayBoss,
            DisplayBonus,
        }
    }
}
