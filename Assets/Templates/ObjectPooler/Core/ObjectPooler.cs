using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPoolerSingleton;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

public class ObjectPooler : PoolerSingleton<ObjectPooler>
{
    [SerializeField] private List<Pool> pools;
    [SerializeField] private List<PoolGroup> poolGroups;

    private Dictionary<string, Pool> _poolsDictionary;
    private Dictionary<string, PoolGroup> _poolGroupsDictionary;
    private Dictionary<string, List<string>> _groupTagToHisPoolTagsDictionary;

    private Dictionary<string, Pool> PoolsDictionary
    {
        get
        {
            if (_poolsDictionary == null)
            {
                InitializePooler();
            }

            return _poolsDictionary;
        }
    }

    private Dictionary<string, PoolGroup> PoolGroupsDictionary
    {
        get
        {
            if (_poolGroupsDictionary == null)
            {
                InitializePooler();
            }

            return _poolGroupsDictionary;
        }
    }
    
    private Dictionary<string, List<string>> GroupTagToHisPoolTagsDictionary
    {
        get
        {
            if (_groupTagToHisPoolTagsDictionary == null)
            {
                InitializePooler();
            }

            return _groupTagToHisPoolTagsDictionary;
        }
    }
    
    #region Initialization

    private void InitializePooler()
    {
        //TryResolveUnassignedPools();
        InitializeSingleObjectPools();
        InitializeObjectPoolGroups();
    }

    private void InitializeSingleObjectPools()
    {
        _poolsDictionary = new Dictionary<string, Pool>();

        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].poolQueue = new Queue<GameObject>();

            if (pools[i].nameAsTag)
            {
                pools[i].poolTag = pools[i].prefab.name;
            }

            _poolsDictionary.Add(pools[i].poolTag, pools[i]);
        }
    }
    private void InitializeObjectPoolGroups()
    {
        _poolGroupsDictionary = new Dictionary<string, PoolGroup>();
        _groupTagToHisPoolTagsDictionary = new Dictionary<string, List<string>>();

        for (int groupIndex = 0; groupIndex < poolGroups.Count; groupIndex++)
        {
            poolGroups[groupIndex].CalculateTotalWeight();
            _poolGroupsDictionary.Add(poolGroups[groupIndex].groupTag, poolGroups[groupIndex]);
            
            List<string> poolTagsInGroup = new List<string>();
            for (int singleIndex = 0; singleIndex < poolGroups[groupIndex].poolsInGroup.Count; singleIndex++)
            {
                poolTagsInGroup.Add(poolGroups[groupIndex].poolsInGroup[singleIndex].pool.poolTag);
            }
            _groupTagToHisPoolTagsDictionary.Add(poolGroups[groupIndex].groupTag, poolTagsInGroup);
        }
    }
    #endregion

    #region Spawning from concrete object pool (General)
    public GameObject SpawnObject(string poolTag)
    {
        GameObject objToReturn;

        TryFindPoolTag(poolTag);

        if (PoolsDictionary[poolTag].poolQueue.Count > 0)
        {
            objToReturn = PoolsDictionary[poolTag].poolQueue.Dequeue();
            objToReturn.gameObject.SetActive(true);
        }
        else
        {
            if(PoolsDictionary[poolTag].parent == null)
            {
                PoolsDictionary[poolTag].parent = new GameObject(poolTag + "Pool").transform;
                PoolsDictionary[poolTag].parent.parent = transform;
            }
            objToReturn = Instantiate(PoolsDictionary[poolTag].prefab, PoolsDictionary[poolTag].parent);
            objToReturn.name = PoolsDictionary[poolTag].prefab.name;
        }

        if(PoolsDictionary[poolTag].autoReturn)
        {
            DelayedReturnObject(objToReturn, poolTag, PoolsDictionary[poolTag].autoReturnDelay);
        }

        return objToReturn;
    }
    private void TryFindPoolTag(string poolTag)
    {
        if (!PoolsDictionary.ContainsKey(poolTag))
        {
            Debug.LogError($"Threse is no pool with pooltag == {poolTag}");
        }
    }
    public GameObject SpawnObject(string poolTag, Vector3 position, Quaternion rotation)
    {
        GameObject objToReturn = SpawnObject(poolTag);
        objToReturn.transform.SetPositionAndRotation(position, rotation);

        return objToReturn;
    }
    #endregion

    #region Spawning from object pool group
    // just using a group tag that points to an array of Concrete object pools tags
    public GameObject SpawnRandomObject(string groupTag)
    {
        TryFindGroupTag(groupTag);

        int rndSingleObjectPoolTagIndex = Random.Range(0, GroupTagToHisPoolTagsDictionary[groupTag].Count);
        string rndSingleObjectPoolTag = GroupTagToHisPoolTagsDictionary[groupTag][rndSingleObjectPoolTagIndex];

        return SpawnObject(rndSingleObjectPoolTag);
    }
    private void TryFindGroupTag(string groupTag)
    {
        if (!GroupTagToHisPoolTagsDictionary.ContainsKey(groupTag))
        {
            Debug.LogError($"Threse is no poolgroup with grouptag == {groupTag}");
        }
    }
    public GameObject SpawnRandomObject(string groupTag, Vector3 position, Quaternion rotation)
    {
        GameObject objToReturn = SpawnRandomObject(groupTag);
        objToReturn.transform.SetPositionAndRotation(position, rotation);

        return objToReturn;
    }
    
    public GameObject SpawnWeightedRandomObject(string groupTag)
    {
        TryFindGroupTag(groupTag);
        
        int randomValue = Random.Range(0, PoolGroupsDictionary[groupTag].totalWeight + 1);

        for (int i = 0; i < PoolGroupsDictionary[groupTag].poolsInGroup.Count; i++)
        {
            WeightedPool weightedPool = PoolGroupsDictionary[groupTag].poolsInGroup[i];
            if (randomValue <=  weightedPool.weight)
            {
                return SpawnObject(weightedPool.pool.poolTag);
            }

            randomValue -= weightedPool.weight;
        }
        
        Debug.LogError($"Return null with {randomValue} weight. Maybe weight are not assigned.");
        return null;
    }
    
    public GameObject SpawnWeightedRandomObject(string groupTag, Vector3 position, Quaternion rotation)
    {
        GameObject objToReturn = SpawnWeightedRandomObject(groupTag);
        objToReturn.transform.SetPositionAndRotation(position, rotation);

        return objToReturn;
    }
    
    #endregion

    #region Return object
    public void ReturnObject(GameObject toReturn, string poolTag)
    {
        TryFindPoolTag(poolTag);

        if (!PoolsDictionary[poolTag].poolQueue.Contains(toReturn))
        {
            toReturn.SetActive(false);
            PoolsDictionary[poolTag].poolQueue.Enqueue(toReturn);
        }
    }
    public async void DelayedReturnObject(GameObject toReturn, string poolTag, float delay)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        
        if (toReturn == null)
        {
            return;
        }
        TryFindPoolTag(poolTag);
        ReturnObject(toReturn, poolTag);
    }
    #endregion
    
#if UNITY_EDITOR
    public void TryResolveUnassignedPools()
    {
        if (poolGroups == null)
        {
            return;
        }
        
        for (int i = 0; i < poolGroups.Count; i++)
        {
            for (int j = 0; j < poolGroups[i].poolsInGroup.Count; j++)
            {
                Pool poolInGroup = poolGroups[i].poolsInGroup[j].pool;

                if (pools == null)
                {
                    pools = new List<Pool> {poolInGroup};
                    continue;
                }
                
                if(!pools.Contains(poolInGroup))
                {
                    Debug.LogWarning($"There is {poolInGroup.poolTag} pool in {poolGroups[i].groupTag} pool group, " +
                                     $"but not in Pools! Added automatically.");
                    pools.Add(poolInGroup);
                }
            }
        }
    }
    
    #region EditorOnly
    public List<Pool> EditorOnlyPools
    {
        get { return pools; }
        set { pools = value; }
    }

    public List<PoolGroup> EditorOnlyPoolGroups
    {
        get { return poolGroups; }
        set { poolGroups = value; }
    }
    #endregion
#endif
}