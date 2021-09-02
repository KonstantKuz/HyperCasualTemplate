using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    private void Start()
    {
        levelManager.LoadLastLevel();
    }
}
