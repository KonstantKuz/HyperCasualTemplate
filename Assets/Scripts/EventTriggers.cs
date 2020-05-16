using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggers : MonoBehaviour, ServiceRequester
{
    private int stimulationTypeCounter = 1;

    private Observer observer;

    private void OnEnable()
    {
        CacheNecessaryService();
    }

    public void CacheNecessaryService()
    {
        observer = ServiceLocator.Instance.Get<Observer>();
    }

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
			observer.OnFinish();
		if (Input.GetKeyDown(KeyCode.S))
			observer.OnPlayerDie();
		if (Input.GetKey(KeyCode.D))
			observer.OnLevelProgressChange();

        if(Input.GetKeyDown(KeyCode.W))
        {
            stimulationTypeCounter++;
            if (stimulationTypeCounter > 3)
                stimulationTypeCounter = 1;

            observer.OnGetStimulationText((StimulType)stimulationTypeCounter);
        }
    }
}
