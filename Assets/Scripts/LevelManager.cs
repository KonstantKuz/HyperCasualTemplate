using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentSceneIndex;
    private int maxLevelCount;

    public int CurrentSceneIndex => currentSceneIndex;

    private void OnEnable()
    {
        maxLevelCount = SceneManager.sceneCountInBuildSettings;
        SubscribeToNecessaryEvets();
    }

    public void SubscribeToNecessaryEvets()
    {
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
            : 0;

        if (SceneManager.GetActiveScene().buildIndex != currentSceneIndex)
        {
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    private void LoadNextScene()
    {
        UpdateCurrentScene();
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void UpdateCurrentScene()
    {
        currentSceneIndex++;

        if (currentSceneIndex >= maxLevelCount)
        {
            currentSceneIndex = 0;
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