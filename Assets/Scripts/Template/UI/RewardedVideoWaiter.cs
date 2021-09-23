using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;
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

        Action waitFunc = null;
        switch (waitVisual)
        {
            case WaitVisual.FadeWhileWait:
                waitFunc = delegate { UpdateFade(AdsManager.Instance.IsRewardedReady()); };
                break;
            case WaitVisual.RotateLoadImage:
                waitFunc = delegate { UpdateLoadImage(AdsManager.Instance.IsRewardedReady()); };
                break;
        }
        StartCoroutine(WaitForVideo(waitFunc));
    }

    private IEnumerator WaitForVideo(Action waitFunc)
    {
        while (true)
        {
            waitFunc?.Invoke();
            yield return updateStep;
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
