using System;
using System.Collections;
using Template.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace Template.UI
{
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

            Action<bool> waitFunc = waitVisual switch 
            {
                WaitVisual.FadeWhileWait => UpdateFade,
                WaitVisual.RotateLoadImage => UpdateLoadImage
            };
            StartCoroutine(WaitForVideo(waitFunc));
        }

        private IEnumerator WaitForVideo(Action<bool> waitFunc)
        {
            while (true)
            {
                waitFunc?.Invoke(AdsManager.Instance.IsRewardedReady());
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
}
