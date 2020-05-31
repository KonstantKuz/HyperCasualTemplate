using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private string car_1PoolTag;
    [SerializeField] private string car_2PoolTag;
    [SerializeField] private string carsGroupTag;

    private void Start()
    {
        StartCoroutine(spawn());
    }

    private IEnumerator spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1f,1.5f));
            Debug.Log("Spawn Car_1");
            ObjectPooler.Instance.SpawnObject(car_1PoolTag);
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
            Debug.Log("Spawn Car_2");
            ObjectPooler.Instance.SpawnObject(car_2PoolTag);
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
            Debug.Log("Spawn Random Car = " +
            ObjectPooler.Instance.SpawnRandomObject(carsGroupTag).name);
        }
    }
}
