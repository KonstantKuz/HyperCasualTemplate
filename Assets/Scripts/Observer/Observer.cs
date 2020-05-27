using System;
using UnityEngine;
using Singleton;
public class Observer : Singleton<Observer>
{
    // Common
    public Action OnStartGame = delegate { Debug.Log("OnStartGame trigerred"); };
    public Action OnPlayerDie = delegate { Debug.Log("OnPlayerDie trigerred"); };
    public Action OnRestartGame = delegate { Debug.Log("OnRestart trigerred"); };
    public Action OnFinish = delegate { Debug.Log("OnFinish trigerred"); };
    public Action OnLoadMainMenu = delegate { Debug.Log("OnLoadMainMenu trigerred"); };

    public Action OnLoadNextLevel = delegate { Debug.Log("OnLoadNextLevel trigerred"); };
    public Action<int> OnAddingScore = delegate { Debug.Log("OnAddingScore trigerred"); };
    public Action OnLevelProgressChange = delegate { Debug.Log("OnLevelProgressChange trigerred"); };
    public Action<StimulType> OnGetStimulationText = delegate { Debug.Log("OnGetStimulationText triggered"); };
    public Action OnLeftMouseButtonDown;

    public Action<int> OnLevelManagerLoaded;
}