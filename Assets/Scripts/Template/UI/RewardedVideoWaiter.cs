using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WaitVisual
{
    FadeWhileWait,
    RotateLoadImage,
}

public class RewardedVideoWaiter : MonoBehaviour
{
    
    [SerializeField] private WaitVisual waitVisual;
    [SerializeField] private Image loadImage;
    [SerializeField] private CanvasGroup disableWhileWait;
    private WaitForSecondsRealtime updateStep;

    private void OnEnable()
    {
        updateStep = new WaitForSecondsRealtime(0.2f);
        StartCoroutine(WaitForVideo());
    }

    private IEnumerator WaitForVideo()
    {
        while (true)
        {
            UpdateVisual(ADManager.Instance.IsRewardedReady());
            yield return updateStep;
        }
    }
    
    private void UpdateVisual(bool videoAvailable)
    {
        switch (waitVisual)
        {
            case WaitVisual.FadeWhileWait:
                UpdateFade(videoAvailable);
                break;
            case WaitVisual.RotateLoadImage:
                UpdateLoadImage(videoAvailable);
                break;
        }
    }

    private void UpdateFade(bool videoAvailable)
    {
        disableWhileWait.alpha = videoAvailable ? 1f : 0.5f;
        disableWhileWait.interactable = videoAvailable;
    }

    private void UpdateLoadImage(bool videoAvailable)
    {
        disableWhileWait.gameObject.SetActive(videoAvailable);
        loadImage.gameObject.SetActive(!videoAvailable);
        
        loadImage.transform.rotation *= Quaternion.Euler(0,0,-20);
    }
}
