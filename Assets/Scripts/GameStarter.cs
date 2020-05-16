using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour, ServiceRequester
{
    private Observer observer;

    private void OnEnable()
    {
        CacheNecessaryService();
    }

    public void CacheNecessaryService()
    {
        observer = ServiceLocator.Instance.Get<Observer>();
    }

    private void Start()
    {
        observer.OnLoadMainMenu();
    }
}
