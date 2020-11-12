using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Observer.Instance.OnStartGame();
            gameObject.SetActive(false);
        }
    }
}
