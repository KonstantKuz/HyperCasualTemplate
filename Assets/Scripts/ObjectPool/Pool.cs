using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ObjectPooler/Pool", fileName = "NewPool")]
[System.Serializable]
public class Pool : ScriptableObject
{
    public GameObject prefab = null;
    //public GameObject parentPrefab;
    public string poolTag = "";
    public bool autoReturn;
    public float autoReturnDelay = 5f;

    [HideInInspector] public Queue<GameObject> poolQueue;
    [HideInInspector] public Transform parent;
}