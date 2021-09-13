using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private IEnumerator Start()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            ObjectPooler.Instance.SpawnObject("1", RndPosition(), Quaternion.identity);
            Debug.Log("Spawned concrete object 1!");
            yield return new WaitForSeconds(0.5f);
            ObjectPooler.Instance.SpawnObject("2", RndPosition(), Quaternion.identity);
            Debug.Log("Spawned concrete object 2!");

            yield return new WaitForSeconds(0.5f);
            ObjectPooler.Instance.SpawnObject("3", RndPosition(), Quaternion.identity);
            Debug.Log("Spawned concrete object 3!");

            yield return new WaitForSeconds(1f);
            ObjectPooler.Instance.SpawnRandomObject("cubes", RndPosition(), Quaternion.identity);
            ObjectPooler.Instance.SpawnRandomObject("cubes", RndPosition(), Quaternion.identity);
            ObjectPooler.Instance.SpawnRandomObject("cubes", RndPosition(), Quaternion.identity);
            Debug.Log("Spawned three random objects from group cubes!");
        }
    }

    private Vector3 RndPosition()
    {
        return transform.position + Vector3.up * Random.Range(-3, 3) + Vector3.up * Random.Range(-3, 3) +
               Vector3.right * Random.Range(-3, 3);
    }
}
