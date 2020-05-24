using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject mainMenuPanel;
    [SerializeField] public GameObject losePanel;
    [SerializeField] public GameObject winPanel;
    
    private List<GameObject> allPanels = new List<GameObject>();
    
    private void OnEnable()
    {
        SubscribeToNecessaryEvets();
    }

    public void SubscribeToNecessaryEvets()
    {
        Observer.Instance.OnLoadMainMenu += delegate { ActivatePanel(mainMenuPanel); };
        Observer.Instance.OnPlayerDie += delegate { ActivatePanel(losePanel); };
        Observer.Instance.OnFinish += delegate { ActivatePanel(winPanel); };
    }

    private void Start()
    {
        SetAllPanels();
    }
    /// <summary>
    /// Метод для вызова с кнопки UI
    /// </summary>
    public void StartGame()
    {
        Observer.Instance.OnStartGame();
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
        Observer.Instance.OnLoadNextLevel();
    }

    public void RestartGame()
    {
        Observer.Instance.OnRestartGame();
    }
}
