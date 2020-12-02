using System;
using System.Collections;
using UnityEngine;

public class Observer : Singleton<Observer>
{
    public Action OnStartGame = delegate { Debug.Log("OnStartGame"); };

    public Action OnLoseLevel = delegate { Debug.Log("OnLoseLevel"); };
    public Action OnWinLevel = delegate { Debug.Log("OnWinLevel"); };
   
    public Action OnLoadNextScene = delegate { Debug.Log("OnLoadNextLevel"); };
    public Action OnRestartScene = delegate { Debug.Log("OnRestart"); };
    
    public bool IsGameLaunched { get; private set; }
    
    private void Start()
    {
        OnStartGame += delegate { IsGameLaunched = true; };
    }

    public void CallOnWinLevel()
    {
        if (IsGameLaunched)
        {
            IsGameLaunched = false;
            OnWinLevel();
        }
    }

    public void CallOnWinLevel(float delay)
    {
        StartCoroutine(DelayedCallFinish());
        IEnumerator DelayedCallFinish()
        {
            yield return new WaitForSecondsRealtime(delay);
            CallOnWinLevel();
        }
    }
    
    public void CallOnLoseLevel()
    {
        if (IsGameLaunched)
        {
            IsGameLaunched = false;
            OnLoseLevel();
        }
    }

    public void CallOnLoseLevel(float delay)
    {
        StartCoroutine(DelayedCallLose());
        IEnumerator DelayedCallLose()
        {
            yield return new WaitForSecondsRealtime(delay);
            CallOnLoseLevel();
        }
    }
}