using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Service, ServiceRequester, ObserverSubscriber
{
    [SerializeField] public GameObject mainMenuPanel;
    [SerializeField] public GameObject losePanel;
    [SerializeField] public GameObject winPanel;
    
    private List<GameObject> allPanels = new List<GameObject>();
    
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

    private void Start()
    {
        SetAllPanels();
    }

    public void CacheNecessaryService()
    {
        observer = ServiceLocator.Instance.Get<Observer>();
    }

    public void SubscribeToNecessaryEvets()
    {
        observer.OnLoadMainMenu += delegate { ActivatePanel(mainMenuPanel); };
        observer.OnPlayerDie += delegate { ActivatePanel(losePanel); };
        observer.OnFinish += delegate { ActivatePanel(winPanel); };
    }

    /// <summary>
    /// Метод для вызова с кнопки UI
    /// </summary>
    public void StartGame()
    {
        observer.OnStartGame();
    }
    
    private void ActivatePanel(params GameObject[] panels)
    {
        DeactivateAllPanels();
        foreach (var panel in panels)
        {
            panel.SetActive(true);
        }
    }
    
    private void DeactivateAllPanels()
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].SetActive(false);
        }
    }
    
    private void SetAllPanels()
    {
        allPanels.Add(mainMenuPanel);
        allPanels.Add(losePanel);
        allPanels.Add(winPanel);
    }
    
    /// <summary>
    /// Метод для вызова с кнопки UI (загрузить следующий уровень)
    /// </summary>
    public void LoadNextLevel()
    {
        observer.OnLoadNextLevel();
    }

    public void RestartGame()
    {
        observer.OnRestartGame();
    }
}
