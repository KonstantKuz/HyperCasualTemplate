using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum ScenesLoadType
{
    RandomAfterLinear,
    Linear,
    Random,
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ScenesLoadType scenesLoadType;
    [SerializeField] private int currentSceneIndex;
    public int CurrentSceneIndex => currentSceneIndex;

    private int maxLevelCount;
    private int firstSceneIndex = 1;

    private void OnEnable()
    {
        maxLevelCount = SceneManager.sceneCountInBuildSettings;
        Observer.Instance.OnLoadNextScene += LoadNextScene;
        Observer.Instance.OnRestartScene += RestartScene;
    }
    
    private void OnDestroy()
    {
        Observer.Instance.OnLoadNextScene -= LoadNextScene;
    }

    private void Start()
    {
        currentSceneIndex = PlayerPrefs.HasKey(GameConstants.PrefsCurrentScene)
            ? PlayerPrefs.GetInt(GameConstants.PrefsCurrentScene)
            : firstSceneIndex;

        if (SceneManager.GetActiveScene().buildIndex != currentSceneIndex)
        {
            SceneManager.LoadScene(currentSceneIndex);
        }
        
        // TinySauce.OnGameStarted($"{currentSceneIndex}");
    }

    private void LoadNextScene()
    {
        // TinySauce.OnGameFinished(currentSceneIndex);

        UpdateCurrentScene();
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void UpdateCurrentScene()
    {
        UpdateSceneIndexAsLinear();
        TryRandomizeSceneIndex();
        
        PlayerPrefs.SetInt(GameConstants.PrefsCurrentScene, currentSceneIndex);
        Debug.Log($"<color=red> Saved current scene prefs as currentSceneIndex=={currentSceneIndex} </color>");
    }

    private void UpdateSceneIndexAsLinear()
    {
        currentSceneIndex++;
        if (currentSceneIndex >= maxLevelCount)
        {
            currentSceneIndex = firstSceneIndex;
            PlayerPrefs.SetInt(GameConstants.PrefsIsLevelCirclePassed, 1);
        }
    }

    private void TryRandomizeSceneIndex()
    {
        if (scenesLoadType == ScenesLoadType.Random || RandomAfterLinear())
        {
            currentSceneIndex = RandomNotThisScene();
        }
    }

    private int RandomNotThisScene()
    {
        currentSceneIndex = Random.Range(firstSceneIndex, maxLevelCount);
        if (currentSceneIndex == SceneManager.GetActiveScene().buildIndex)
        {
            return RandomNotThisScene();
        }
        else
        {
            return currentSceneIndex;
        }
    }

    private bool RandomAfterLinear()
    {
        return scenesLoadType == ScenesLoadType.RandomAfterLinear && PlayerPrefs.GetInt(GameConstants.PrefsIsLevelCirclePassed) == 1;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CleanPrefs()
    {
        PlayerPrefs.DeleteKey(GameConstants.PrefsCurrentScene);
        PlayerPrefs.DeleteKey(GameConstants.PrefsIsLevelCirclePassed);
    }

    public void SetCurrentScene()
    {
        PlayerPrefs.SetInt(GameConstants.PrefsCurrentScene, currentSceneIndex);
    }
}