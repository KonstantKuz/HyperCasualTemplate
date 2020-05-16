using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Service, ServiceRequester, ObserverSubscriber
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

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
        observer.OnAddingScore += AddScore;
    }

    private void OnDestroy()
    {
        observer.OnAddingScore -= AddScore;
    }

    private void AddScore(int value)
    {
        score += value;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.SetText(score.ToString());
    }
}
