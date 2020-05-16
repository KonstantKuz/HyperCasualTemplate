using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Service, ServiceRequester, ObserverSubscriber
{
    [SerializeField] private int currentLevel;

    public int CurrentLevel => currentLevel;


    public Observer observer { get; private set; }
    
    public override void RegisterService()
    {
        ServiceLocator.Instance.Register(this);
    }

    private void OnEnable()
    {
        CacheNecessaryService();
        SubscribeToNecessaryEvets();
    }

    public void CacheNecessaryService()
    {
        observer = ServiceLocator.Instance.Get<Observer>();
    }
    
    public void SubscribeToNecessaryEvets()
    {
        observer.OnLoadNextLevel += LoadNextLevel;
        observer.OnRestartGame += RestartLevel;
    }

    private void OnDestroy()
    {
        observer.OnLoadNextLevel -= LoadNextLevel;
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
