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
    public int currentLevelIndexToSet;
    
    private PlayableLevel[] PlayableLevels => levelsQueue.levels;
    private int MaxLevelsCount => PlayableLevels.Length;

    private const string PrefsCurrentLevelIndex = "CurrentLevelIndex";

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
        int lastLevelIndex = GetCurrentLevelIndex();
        SceneManager.LoadScene(PlayableLevels[lastLevelIndex].sceneName);
    }

    public static int GetCurrentLevelIndex()
    {
        int currentLevelIndex = PlayerPrefs.HasKey(PrefsCurrentLevelIndex)
            ? PlayerPrefs.GetInt(PrefsCurrentLevelIndex)
            : 0;
        return currentLevelIndex;
    }

    public void SetCurrentLevelIndex(int index)
    {
        PlayerPrefs.SetInt(PrefsCurrentLevelIndex, index);
    }
    
    private void LoadNextLevel()
    {
        int nextLevelIndex = UpdateLevelIndex();
        SceneManager.LoadScene(PlayableLevels[nextLevelIndex].sceneName);
    }

    private int UpdateLevelIndex()
    {
        int currentLevelIndex = GetCurrentLevelIndex();
        
        if (IsRandomSceneSelection())
        {
            currentLevelIndex = RandomNotThisScene(currentLevelIndex);
        }
        else
        {
            currentLevelIndex = UpdateSceneIndexAsLinear(currentLevelIndex);
        }
        
        SetCurrentLevelIndex(currentLevelIndex);
        
        Debug.Log($"<color=red> Saved current level index == {currentLevelIndex} </color>");

        return GetCurrentLevelIndex();
    }

    private bool IsRandomSceneSelection()
    {
        return scenesLoadType == ScenesLoadType.Random || 
               scenesLoadType == ScenesLoadType.RandomAfterLinear && PlayerPrefs.GetInt(GameConstants.PrefsIsLevelCirclePassed) == 1;
    }

    private int RandomNotThisScene(int currentIndex)
    {
        int rndSceneIndex = Random.Range(0, MaxLevelsCount - 1);
        if (rndSceneIndex >= currentIndex)
        {
            rndSceneIndex++;
        }
        return rndSceneIndex;
    }

    private int UpdateSceneIndexAsLinear(int currentIndex)
    {
        currentIndex++;
        
        if (currentIndex >= MaxLevelsCount)
        {
            currentIndex = 0;
            PlayerPrefs.SetInt(GameConstants.PrefsIsLevelCirclePassed, 1);
        }

        if (PlayerPrefs.GetInt(GameConstants.PrefsIsLevelCirclePassed) == 1 
        && !PlayableLevels[currentIndex].isRepeatable)
        {
            return UpdateSceneIndexAsLinear(currentIndex);
        }

        return currentIndex;
    }
    
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public int GetClampedNextLevelIndex()
    {
        bool levelCirclePassed = false;
        int nextLevelIndex = GetCurrentLevelIndex();
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
    
    public int GetLevelIndexFromUILevel(int uiLevel)
    {
        bool levelCirclePassed = false;
        int sceneIndex = -1;
        for (int i = 0; i < uiLevel; i++)
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
}