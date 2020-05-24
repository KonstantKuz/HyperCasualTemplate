using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    private int stimulationTypeCounter = 1;

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
            Observer.Instance.OnFinish();
		if (Input.GetKeyDown(KeyCode.S))
            Observer.Instance.OnPlayerDie();
		if (Input.GetKey(KeyCode.D))
            Observer.Instance.OnLevelProgressChange();

        if(Input.GetKeyDown(KeyCode.W))
        {
            stimulationTypeCounter++;
            if (stimulationTypeCounter > 3)
                stimulationTypeCounter = 1;

            Observer.Instance.OnGetStimulationText((StimulType)stimulationTypeCounter);
        }
    }
}
