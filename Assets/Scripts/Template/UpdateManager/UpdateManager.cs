using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpdateManager : Singleton<UpdateManager>
{
    private List<Action> customUpdates = new List<Action>(1000);
    private List<Action> customFixedUpdates = new List<Action>(1000);
    private List<Action> customLateUpdates = new List<Action>(1000);
    
    private void Awake()
    {
       Application.targetFrameRate = 60;
    }

    public void StartUpdate(Action obj)
    {
        customUpdates.Add(obj);
    }

    public void StartFixedUpdate(Action obj)
    {
        customFixedUpdates.Add(obj);
    }

    public void StartLateUpdate(Action obj)
    {
        customLateUpdates.Add(obj);
    }
    
    public void StopUpdate(Action obj)
    {
        customUpdates.Remove(obj);
    }

    public void StopFixedUpdate(Action obj)
    {
        customFixedUpdates.Remove(obj);
    }

    public void StopLateUpdate(Action obj)
    {
        customLateUpdates.Remove(obj);
    }
    
    private void Update()
    {
        foreach (Action obj in customUpdates)
        {
            obj.Invoke();
        }
    }

    private void FixedUpdate()
    {
        foreach (Action obj in customFixedUpdates)
        {
            obj.Invoke();
        }
    }

    private void LateUpdate()
    {
        foreach (Action obj in customFixedUpdates)
        {
            obj.Invoke();
        }
    }
}
