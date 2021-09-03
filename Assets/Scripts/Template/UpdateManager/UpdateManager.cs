using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UpdateManager : Singleton<UpdateManager>
{
    private List<MonoCached> customUpdates = new List<MonoCached>(1000);
    private List<MonoCached> customFixedUpdates = new List<MonoCached>(1000);
    private List<MonoCached> customLateUpdates = new List<MonoCached>(1000);
    
    private void Awake()
    {
       Application.targetFrameRate = 60;
    }

    public void StartUpdate(MonoCached obj)
    {
        customUpdates.Add(obj);
    }

    public void StartFixedUpdate(MonoCached obj)
    {
        customFixedUpdates.Add(obj);
    }

    public void StartLateUpdate(MonoCached obj)
    {
        customLateUpdates.Add(obj);
    }
    
    public void StopUpdate(MonoCached obj)
    {
        customUpdates.Add(obj);
    }

    public void StopFixedUpdate(MonoCached obj)
    {
        customFixedUpdates.Add(obj);
    }

    public void StopLateUpdate(MonoCached obj)
    {
        customLateUpdates.Add(obj);
    }
    
    private void Update()
    {
        foreach (MonoCached obj in customUpdates)
        {
            obj.CustomUpdate();
        }
    }

    private void FixedUpdate()
    {
        foreach (MonoCached obj in customFixedUpdates)
        {
            obj.CustomFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        foreach (MonoCached obj in customFixedUpdates)
        {
            obj.CustomFixedUpdate();
        }
    }
}
