using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requester : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(spawn());
    }

    private IEnumerator spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Spawn Car_1");
            ObjectPooler.Instance.SpawnObject("Car_1");
            yield return new WaitForSeconds(1f);
            Debug.Log("Spawn Car_2");
            ObjectPooler.Instance.SpawnObject("Car_2");
            yield return new WaitForSeconds(1f);
            Debug.Log("Spawn Random Car");
            ObjectPooler.Instance.SpawnRandomObject("Cars");
        }
    }
}
