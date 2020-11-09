using System;
using UnityEngine;

public class Observer : Singleton<Observer>
{
    public Action OnStartGame = delegate { Debug.Log("OnStartGame"); };

    public Action OnLoseLevel = delegate { Debug.Log("OnLoseLevel"); };
    public Action OnWinLevel = delegate { Debug.Log("OnWinLevel"); };
   
    public Action OnLoadNextScene = delegate { Debug.Log("OnLoadNextLevel"); };
    public Action OnRestartScene = delegate { Debug.Log("OnRestart"); };
    
    private bool isGameOver = false;
    
    public void CallOnWinLevel()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            OnWinLevel();
        }
    }
    
    public void CallOnLoseLevel()
    {
        if(!isGameOver)
        {
            isGameOver = true;
            OnLoseLevel();
        }
    }
}