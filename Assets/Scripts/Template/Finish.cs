using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private bool callWin = true;
    [SerializeField] private float delay = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!callWin)
        {
            return;
        }
        
        if (other.CompareTag(GameConstants.TagPlayer))
        {
            DelayHandler.DelayedCallAsync(delay, Observer.Instance.CallOnWinLevel);
        }
    }
}
