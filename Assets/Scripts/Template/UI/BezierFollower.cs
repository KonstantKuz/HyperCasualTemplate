using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BezierFollower : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform[] routes;
    
    private int currentRouteIndex = 0;
    private float tParam = 0;

    private void OnEnable()
    {
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            yield return StartCoroutine(MoveOnLoopSegment());

            currentRouteIndex++;
            if (currentRouteIndex >= routes.Length)
            {
                currentRouteIndex = 0;
            }
        }
    }

    private IEnumerator MoveOnLoopSegment()
    {
        Vector2 p0 = routes[currentRouteIndex].GetChild(0).position;
        Vector2 p1 = routes[currentRouteIndex].GetChild(1).position;
        Vector2 p2 = routes[currentRouteIndex].GetChild(2).position;
        Vector2 p3 = routes[currentRouteIndex].GetChild(3).position;
       
        tParam = 0f;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speed;

            Vector2 finalPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                                    3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                                    3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                                    Mathf.Pow(tParam, 3) * p3;

            transform.position = finalPosition;
            yield return null;
        }
    }
}
