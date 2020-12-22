using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defeat : MonoBehaviour
{
    [SerializeField] private bool callDefeat = true;
    [SerializeField] private float delay = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!callDefeat)
        {
            return;
        }
        
        if (other.CompareTag(GameConstants.TagPlayer))
        {
            Observer.Instance.CallOnLoseLevel(delay);
        }
    }
}
