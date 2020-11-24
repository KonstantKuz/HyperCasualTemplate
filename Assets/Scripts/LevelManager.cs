using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
        currentSceneIndex++;

        if (currentSceneIndex >= maxLevelCount)
        {
            currentSceneIndex = firstSceneIndex;
        }
        PlayerPrefs.SetInt(GameConstants.PrefsCurrentScene, currentSceneIndex);
        
        Debug.Log($"<color=red> Saved current scene prefs as {currentSceneIndex} </color>");
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CleanPrefs()
    {
        PlayerPrefs.DeleteKey(GameConstants.PrefsCurrentScene);
    }

    public void SetCurrentScene()
    {
        PlayerPrefs.SetInt(GameConstants.PrefsCurrentScene, currentSceneIndex);
    }
}