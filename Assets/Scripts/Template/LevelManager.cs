using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum ScenesLoadType
{
    Linear,
    Random,
    RandomAfterLinear,
}

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private ScenesLoadType scenesLoadType;
    [SerializeField] private LevelsQueue levelsQueue;
    [Tooltip("Определяет будет ли увеличиваться счетчик уровней при прохождении бонус уровней.")]
    [SerializeField] private bool countBonusLevels;
    
    [Header("Editor Only")]
    public int currentLevelIndexToSet;
    
    private const string PrefsIsCircleOfLevelsPassed = "IsLevelCirclePassed";
    private const string PrefsCurrentLevelIndex = "CurrentLevelIndex";
    private const string PrefsCurrentDisplayLevelNumber = "CurrentDisplayLevelNumber";

    private const string BossLevelsNameContainingPart = "Boss";
    private const string BonusLevelsNameContainingPart = "Bonus";

    public PlayableLevel[] PlayableLevels => levelsQueue.levels;
    public int MaxLevelsCount => PlayableLevels.Length;
    public static int CurrentLevelIndex => PlayerPrefs.GetInt(PrefsCurrentLevelIndex);
    public static int CurrentDisplayLevelNumber => PlayerPrefs.GetInt(PrefsCurrentDisplayLevelNumber, 1);
    
    private void OnEnable()
    {
        Observer.Instance.OnLoadNextScene += LoadNextLevel;
        Observer.Instance.OnRestartScene += RestartScene;
    }
    
    private void OnDisable()
    {
        Observer.Instance.OnLoadNextScene -= LoadNextLevel;
        Observer.Instance.OnRestartScene -= RestartScene;
    }

    public void LoadLastLevel()
    {
        SceneManager.LoadScene(PlayableLevels[CurrentLevelIndex].sceneName);
    }
    
    public void SetCurrentLevelIndex(int index)
    {
        PlayerPrefs.SetInt(PrefsCurrentLevelIndex, index);
    }

    private void SetCurrentDisplayLevelNumber(int currentDisplayNumber)
    {
        PlayerPrefs.SetInt(PrefsCurrentDisplayLevelNumber, currentDisplayNumber);
    }

    private bool IsCircleOfLevelsPassed()
    {
        return PlayerPrefs.GetInt(PrefsIsCircleOfLevelsPassed, 0) == 1;
    }

    private void MarkCircleOfLevelsAsPassed()
    {
        PlayerPrefs.SetInt(PrefsIsCircleOfLevelsPassed, 1);
    }
    
    private void LoadNextLevel()
    {
        IncreaseDisplayLevelNumber();
        
        int nextLevelIndex = UpdateLevelIndex();
        SceneManager.LoadScene(PlayableLevels[nextLevelIndex].sceneName);
    }

    private void IncreaseDisplayLevelNumber()
    {
        if (!countBonusLevels && IsBonusLevel(CurrentLevelIndex))
        {
            return;
        }
        
        int currentNumber = CurrentDisplayLevelNumber;
        currentNumber++;
        SetCurrentDisplayLevelNumber(currentNumber);
    }

    private int UpdateLevelIndex()
    {
        int currentLevelIndex = CurrentLevelIndex;
        
        if (IsRandomSceneSelection())
        {
            currentLevelIndex = RandomNotThisLevel(currentLevelIndex);
        }
        else
        {
            currentLevelIndex = UpdateIndexAsLinear(currentLevelIndex);
        }
        
        SetCurrentLevelIndex(currentLevelIndex);
        
        Debug.Log($"<color=red> Saved current level index == {currentLevelIndex} </color>");

        return CurrentLevelIndex;
    }

    private bool IsRandomSceneSelection()
    {
        return scenesLoadType == ScenesLoadType.Random || 
               scenesLoadType == ScenesLoadType.RandomAfterLinear && IsCircleOfLevelsPassed();
    }

    private int RandomNotThisLevel(int currentIndex)
    {
        int rndSceneIndex = Random.Range(0, MaxLevelsCount - 1);
        if (rndSceneIndex >= currentIndex)
        {
            rndSceneIndex++;
        }
        return rndSceneIndex;
    }

    private int UpdateIndexAsLinear(int currentIndex)
    {
        currentIndex++;
        
        if (currentIndex >= MaxLevelsCount)
        {
            currentIndex = 0;
            MarkCircleOfLevelsAsPassed();
        }

        if ( IsCircleOfLevelsPassed() && !PlayableLevels[currentIndex].isRepeatable)
        {
            return UpdateIndexAsLinear(currentIndex);
        }

        return currentIndex;
    }
    
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public int GetNextLevelIndex()
    {
        bool levelCirclePassed = false;
        int nextLevelIndex = CurrentLevelIndex;
        nextLevelIndex++;
        
        if (nextLevelIndex >= MaxLevelsCount)
        {
            nextLevelIndex = 0;
            levelCirclePassed = true;
        }
        
        if (levelCirclePassed)
        {
            while (!PlayableLevels[nextLevelIndex].isRepeatable)
            {
                nextLevelIndex++;
            }
        }

        return nextLevelIndex;
    }
    
    public int GetLevelIndexFromLevelsCount(int levelsCount)
    {
        bool levelCirclePassed = false;
        int sceneIndex = -1;
        for (int i = 0; i < levelsCount; i++)
        {
            sceneIndex++;
            if (sceneIndex >= PlayableLevels.Length)
            {
                sceneIndex = 0;
                levelCirclePassed = true;
            }
            
            if (levelCirclePassed)
            {
                while (!PlayableLevels[sceneIndex].isRepeatable)
                {
                    sceneIndex++;
                }
            }
        }
        
        return sceneIndex;
    }

    public bool IsBossLevel(int levelIndex)
    {
        return PlayableLevels[levelIndex].sceneName.Contains(BossLevelsNameContainingPart);
    }

    public bool IsBonusLevel(int levelIndex)
    {
        return PlayableLevels[levelIndex].sceneName.Contains(BonusLevelsNameContainingPart);
    }
}
