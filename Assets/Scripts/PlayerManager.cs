using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Service, ServiceRequester, ObserverSubscriber
{
    private bool isGameStarted;

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
        observer.OnStartGame += StartGame;
    }

    private void StartGame()
    {
        isGameStarted = true;
    }
}
