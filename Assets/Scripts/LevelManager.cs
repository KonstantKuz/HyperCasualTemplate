using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentLevel;

    public int CurrentLevel => currentLevel;

    private void OnEnable()
    {
        SubscribeToNecessaryEvets();
    }

    public void SubscribeToNecessaryEvets()
    {
        Observer.Instance.OnLoadNextLevel += LoadNextLevel;
        Observer.Instance.OnRestartGame += RestartLevel;
    }

    private void OnDestroy()
    {
        Observer.Instance.OnLoadNextLevel -= LoadNextLevel;
    }

    private void Start()
    {
        currentLevel = PlayerPrefs.HasKey(GameConstants.PrefsCurrentLevel)
            ? PlayerPrefs.GetInt(GameConstants.PrefsCurrentLevel)
            : 1;
    }
    
    private void LoadNextLevel()
    {
        UpdateCurrentLevel();
        SceneManager.LoadScene(0);
    }

    private void UpdateCurrentLevel()
    {
        Debug.Log("UpdateActiveLevel");
        Debug.Log($"<color=red> Setted active level prefs to currentlevel = {currentLevel} </color>");
        currentLevel++;
        PlayerPrefs.SetInt(GameConstants.PrefsCurrentLevel, currentLevel);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
