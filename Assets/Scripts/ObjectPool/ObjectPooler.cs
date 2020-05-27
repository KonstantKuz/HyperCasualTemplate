using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
[System.Serializable]
public class Pool
{
    public Transform parent = null;
    public GameObject prefab = null;
    public string poolTag = "";
    public bool useDefaultReturnToPool = true;
    public float defaultReturnDelay = 5f;
    public Queue<PooledObject> pool;
}

public class ObjectPooler : Singleton<ObjectPooler>
{
    public List<Pool> pools;

    private Dictionary<string, Pool> poolDictionary;

    private void Awake()
    {
        InitializePooler();
    }

    private void InitializePooler()
    {
        poolDictionary = new Dictionary<string, Pool>();

        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].pool = new Queue<PooledObject>();
            poolDictionary.Add(pools[i].poolTag, pools[i]);
        }
    }

    public PooledObject SpawnObject(string poolTag)
    {
        PooledObject objToReturn;

        if(!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogError($"Threse is no pools with name {poolTag}");
        }

        if(poolDictionary[poolTag].pool.Count > 0)
        {
            objToReturn = poolDictionary[poolTag].pool.Dequeue();
            objToReturn.gameObject.SetActive(true);
        }
        else
        {
            objToReturn = Instantiate(poolDictionary[poolTag].prefab, poolDictionary[poolTag].parent).AddComponent<PooledObject>();
            objToReturn.pooler = this;
            objToReturn.poolTag = poolDictionary[poolTag].poolTag;
        }

        if(poolDictionary[poolTag].useDefaultReturnToPool)
        {
            objToReturn.DelayedReturnToPool(poolDictionary[poolTag].defaultReturnDelay);
        }
        return objToReturn;
    }
    public PooledObject SpawnObject(string poolTag, Vector3 position)
    {
        PooledObject objToReturn;

        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogError($"Threse is no pools with name {poolTag}");
        }

        objToReturn = SpawnObject(poolTag);
        objToReturn.transform.position = position;

        return objToReturn;
    }
    public PooledObject SpawnObject(string poolTag, Vector3 position, Quaternion rotation)
    {
        PooledObject objToReturn;

        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogError($"Threse is no pools with name {poolTag}");
        }

        objToReturn = SpawnObject(poolTag);
        objToReturn.transform.position = position;
        objToReturn.transform.rotation = rotation;
        return objToReturn;
    }

    public void ReturnObject(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        if(!poolDictionary.ContainsKey(pooledObject.poolTag))
        {
            Debug.LogError($"Threse is no pools with tag {pooledObject.poolTag}");
        }
        if(!poolDictionary[pooledObject.poolTag].pool.Contains(pooledObject))
        {
            toReturn.SetActive(false);
            poolDictionary[pooledObject.poolTag].pool.Enqueue(pooledObject);
        }
    }
}