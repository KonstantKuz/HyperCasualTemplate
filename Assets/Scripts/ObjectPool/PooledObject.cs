using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPooler pooler;
    [HideInInspector] public string poolTag;

    public void DelayedReturnToPool(float delay)
    {
        StartCoroutine(DelayedReturn(delay));
    }

    private IEnumerator DelayedReturn(float delay)
    {
        yield return new WaitForSeconds(delay);
        pooler.ReturnObject(gameObject);
    }
}