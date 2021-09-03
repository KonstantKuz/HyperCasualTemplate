using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalLevelProgressBar : SceneLineProgressBar<GlobalLevelProgressBar>
{
    [SerializeField] private GlobalLevelProgressBarBlock[] progressBlocks;

    private InitialData<GlobalLevelProgressBar> initialData;
    private static UpdateData<GlobalLevelProgressBar> updateData;

    private const string PrefsUpdateBarProgress = "UpdateBarProgress";
    private const string PrefsPassedLevelsCount = "PassedLevelsCount";
    private const string PrefsFirstBlockLevelNumber = "FirstDisplayLevelNumber";
    
    private void Start()
    {
        LevelManager levelManager = LevelManager.Instance;

        int updateBarProgress = PlayerPrefs.GetInt(PrefsUpdateBarProgress, 1);
        int passedLevelsCount = PlayerPrefs.GetInt(PrefsPassedLevelsCount, 1);
        int firstBlockLevelNumber = PlayerPrefs.GetInt(PrefsFirstBlockLevelNumber, 1);
        
        if (updateBarProgress >= 6)
        {
            // Не сбрасывается до 1 для случая "перескока" через один или более уровней
            // Например при отказе проходить бонус уровень
            PlayerPrefs.SetInt(PrefsUpdateBarProgress, updateBarProgress - 5);
            PlayerPrefs.SetInt(PrefsPassedLevelsCount, passedLevelsCount + 5);
            PlayerPrefs.SetInt(PrefsFirstBlockLevelNumber, LevelManager.CurrentLevelIndex);

            updateBarProgress = PlayerPrefs.GetInt(PrefsUpdateBarProgress);
            firstBlockLevelNumber = PlayerPrefs.GetInt(PrefsFirstBlockLevelNumber);
            passedLevelsCount = PlayerPrefs.GetInt(PrefsPassedLevelsCount);
        }

        int blockLevelNumber = firstBlockLevelNumber;
        for (int i = 0; i < progressBlocks.Length; i++)
        {
            int levelIndex = levelManager.GetLevelIndexFromLevelsCount(passedLevelsCount + i);
            if (levelManager.IsBossLevel(levelIndex))
            {
                progressBlocks[i].SwitchDisplayType(BlockDisplayType.DisplayBoss);
            }
            else if (levelManager.IsBonusLevel(levelIndex))
            {
                progressBlocks[i].SwitchDisplayType(BlockDisplayType.DisplayBonus);
                
                blockLevelNumber--;
            }
            else
            {
                progressBlocks[i].SwitchDisplayType(BlockDisplayType.DisplayLevel);
                progressBlocks[i].levelText.SetText(blockLevelNumber.ToString());
            }

            blockLevelNumber++;
        }

        InitializeProgress(updateBarProgress);
        
        Observer.Instance.OnStartGame += delegate
        {
            SetHalfProgress(updateBarProgress);
        };
        
        Observer.Instance.OnLoseLevel += delegate { gameObject.SetActive(false); };
        Observer.Instance.OnWinLevel += delegate { gameObject.SetActive(false); };

        Observer.Instance.OnLoadNextScene += IncreaseGlobalProgress;
    }

    private void InitializeProgress(int globalProgress)
    {
        float currentValue = 0f;
        if (globalProgress == 1)
        {
            currentValue = 0.14f;
        }
        if (globalProgress == 2)
        {
            currentValue = 0.355f;
        }
        if (globalProgress == 3)
        {
            currentValue = 0.57f;
        }
        if (globalProgress == 4)
        {
            currentValue = 0.785f;
        }
        if (globalProgress == 5)
        {
            currentValue = 1f;
        }
        
        initialData.CurrentValue = currentValue;
        initialData.MinValue = 0;
        initialData.MaxValue = 1f;
        Initialize(initialData);   
    }

    private void SetHalfProgress(int globalProgress)
    {
        float currentValue = 0f;
        if (globalProgress == 1)
        {
            currentValue = 0.185f;
        }
        if (globalProgress == 2)
        {
            currentValue = 0.4f;
        }
        if (globalProgress == 3)
        {
            currentValue = 0.61f;
        }
        if (globalProgress == 4)
        {
            currentValue = 0.825f;
        }
        if (globalProgress == 5)
        {
            currentValue = 1f;
        }
        
        updateData.CurrentValue = currentValue;
        UpdateProgress(updateData);
    }
    
    public static void IncreaseGlobalProgress()
    {
        int currentProgress = PlayerPrefs.GetInt(PrefsUpdateBarProgress, 1);
        currentProgress++;
        PlayerPrefs.SetInt(PrefsUpdateBarProgress, currentProgress);
    }
    
    [Serializable]
    private class GlobalLevelProgressBarBlock
    {
        public TextMeshProUGUI levelText;
        public Image bossImage;
        public Image bonusImage;

        public void SwitchDisplayType(BlockDisplayType displayType)
        {
            switch (displayType)
            {
                case BlockDisplayType.DisplayLevel:
                    levelText.gameObject.SetActive(true);
                    bossImage.gameObject.SetActive(false);
                    bonusImage.gameObject.SetActive(false);
                    break;
                case BlockDisplayType.DisplayBoss:
                    levelText.gameObject.SetActive(false);
                    bossImage.gameObject.SetActive(true);
                    bonusImage.gameObject.SetActive(false);
                    break;
                case BlockDisplayType.DisplayBonus:
                    levelText.gameObject.SetActive(false);
                    bossImage.gameObject.SetActive(false);
                    bonusImage.gameObject.SetActive(true);
                    break;
            }
        }
    }
    
    private enum BlockDisplayType
    {
        DisplayLevel,
        DisplayBoss,
        DisplayBonus,
    }
}
