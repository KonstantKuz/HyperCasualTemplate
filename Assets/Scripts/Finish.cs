using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private bool callFinish = true;
    [SerializeField] private float delay = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!callFinish)
        {
            return;
        }
        
        if (other.CompareTag(GameConstants.TagPlayer))
        {
            Observer.Instance.CallOnWinLevel(delay);
        }
    }
}
