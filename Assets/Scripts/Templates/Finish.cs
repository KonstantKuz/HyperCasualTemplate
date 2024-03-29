﻿using Templates.Constants;
using UnityEngine;

namespace Templates
{
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
        
            if (other.CompareTag(Tags.Player))
            {
                DelayHandler.Instance.DelayedCallAsync(delay, Observer.Instance.CallOnWinLevel);
            }
        }
    }
}
