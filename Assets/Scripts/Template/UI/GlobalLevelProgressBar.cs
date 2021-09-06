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

    private float[] progressToInitValue = { 0f, 0.14f, 0.355f, 0.57f, 0.785f, 1f };
    private float[] progressToHalfValue = { 0f, 0.185f, 0.4f, 0.61f, 0.825f, 1f };
    
    
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
            GlobalLevelProgressBarBlock progressBlock = progressBlocks[i];
            
            int levelIndex = levelManager.GetLevelIndexFromLevelsCount(passedLevelsCount + i);
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
