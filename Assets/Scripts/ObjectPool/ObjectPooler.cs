using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
[System.Serializable]
public class Pool
{
    public Transform parent;
    public Queue<GameObject> poolQueue;
    public PoolData poolData;
}

public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] private List<Pool> pools;
    [SerializeField] private List<PoolGroup> poolGroups;

    private Dictionary<string, Pool> poolsDictionary;
    private Dictionary<string, List<string>> groupTagToPoolTagDictionary;

    private void Awake()
    {
        InitializePooler();
    }
    private void InitializePooler()
    {
        InitializeSingleObjectPools();
        InitializeObjectPoolGroups();
    }
    private void InitializeSingleObjectPools()
    {
        poolsDictionary = new Dictionary<string, Pool>();

        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].poolQueue = new Queue<GameObject>();
            poolsDictionary.Add(pools[i].poolData.poolTag, pools[i]);
        }
    }
    private void InitializeObjectPoolGroups()
    {
        groupTagToPoolTagDictionary = new Dictionary<string, List<string>>();

        for (int groupIndex = 0; groupIndex < poolGroups.Count; groupIndex++)
        {
            List<string> singlePoolTags = new List<string>();
            for (int singleIndex = 0; singleIndex < poolGroups[groupIndex].group.Length; singleIndex++)
            {
                singlePoolTags.Add(poolGroups[groupIndex].group[singleIndex].poolTag);
            }
            groupTagToPoolTagDictionary.Add(poolGroups[groupIndex].groupTag, singlePoolTags);
        }
    }

    #region Spawning from concrete object pool
    public GameObject SpawnObject(string poolTag)
    {
        GameObject objToReturn;

        TryFindInSinglePoolsDictionary(poolTag);

        if (poolsDictionary[poolTag].poolQueue.Count > 0)
        {
            objToReturn = poolsDictionary[poolTag].poolQueue.Dequeue();
            objToReturn.gameObject.SetActive(true);
        }
        else
        {
            objToReturn = Instantiate(poolsDictionary[poolTag].poolData.prefab, poolsDictionary[poolTag].parent);
        }

        if(poolsDictionary[poolTag].poolData.useDefaultReturn)
        {
            DelayedReturnObject(objToReturn, poolTag, poolsDictionary[poolTag].poolData.defaultReturnDelay);
        }

        return objToReturn;
    }
    public void TryFindInSinglePoolsDictionary(string poolTag)
    {

        if (!poolsDictionary.ContainsKey(poolTag))
        {
            Debug.LogError($"Threse is no pool with pooltag == {poolTag}");
        }
    }
    public GameObject SpawnObject(string poolTag, Vector3 position)
    {
        GameObject objToReturn;

        objToReturn = SpawnObject(poolTag);
        objToReturn.transform.position = position;

        return objToReturn;
    }
    public GameObject SpawnObject(string poolTag, Vector3 position, Quaternion rotation)
    {
        GameObject objToReturn;

        objToReturn = SpawnObject(poolTag);
        objToReturn.transform.position = position;
        objToReturn.transform.rotation = rotation;

        return objToReturn;
    }
    #endregion

    #region Spawning from object pool group
    // just using a group tag that points to an array of Concrete object pools tags

    public GameObject SpawnRandomObject(string groupTag)
    {
        GameObject objToReturn;

        if (!groupTagToPoolTagDictionary.ContainsKey(groupTag))
        {
            Debug.LogError($"Threse is no poolgroup with grouptag == {groupTag}");
        }

        int rndSingleObjectPoolTagIndex = Random.Range(0, groupTagToPoolTagDictionary[groupTag].Count);
        string rndSingleObjectPoolTag = groupTagToPoolTagDictionary[groupTag][rndSingleObjectPoolTagIndex];

        objToReturn = SpawnObject(rndSingleObjectPoolTag);

        return objToReturn;
    }
    public GameObject SpawnRandomObject(string groupTag, Vector3 position)
    {
        GameObject objToReturn;

        objToReturn = SpawnRandomObject(groupTag);
        objToReturn.transform.position = position;

        return objToReturn;
    }
    public GameObject SpawnRandomObject(string groupTag, Vector3 position, Quaternion rotation)
    {
        GameObject objToReturn;

        objToReturn = SpawnRandomObject(groupTag);
        objToReturn.transform.position = position;
        objToReturn.transform.rotation = rotation;

        return objToReturn;
    }

    #endregion

    public void ReturnObject(GameObject toReturn, string poolTag)
    {
        TryFindInSinglePoolsDictionary(poolTag);

        if (!poolsDictionary[poolTag].poolQueue.Contains(toReturn))
        {
            toReturn.SetActive(false);
            poolsDictionary[poolTag].poolQueue.Enqueue(toReturn);
        }
    }

    public void DelayedReturnObject(GameObject toReturn, string poolTag, float delay)
    {
        TryFindInSinglePoolsDictionary(poolTag);

        StartCoroutine(DelayedReturn(toReturn, poolTag, delay));
    }

    private IEnumerator DelayedReturn(GameObject toReturn, string poolTag, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnObject(toReturn, poolTag);
    }
}