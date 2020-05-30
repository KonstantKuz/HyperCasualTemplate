using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ObjectPooler/Pool", fileName = "NewPool")]
[System.Serializable]
public class PoolData : ScriptableObject
{
    public GameObject prefab = null;
    public string poolTag = "";
    public bool useDefaultReturn;
    public float defaultReturnDelay = 5f;
}