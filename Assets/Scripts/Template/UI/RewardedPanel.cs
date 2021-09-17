using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardedPanel : MonoBehaviour
{
    [SerializeField] private int maxTime;
    [SerializeField] private int minTime;
    [SerializeField] private bool hideOnTimerEnds;
    [SerializeField] private Text timerText;
    [SerializeField] private Button getRewardButton;

    [SerializeField] private bool showNoThanksButton;
    [SerializeField] private float showDelay;
    [SerializeField] private Button noThanksButton;
    
    private int timer;
    public int Timer => timer;
    private Coroutine counter;

    public bool AlreadyShown()
    {
        return counter != null;
    }

    public void ShowPanel()
    {
        getRewardButton.onClick.RemoveAllListeners();
        noThanksButton.onClick.RemoveAllListeners();
        
        gameObject.SetActive(true);
        timerText.text = maxTime.ToString();
    }

    public void StartCount(Action onGetReward, Action onDiscardReward)
    {
        getRewardButton.onClick.AddListener(delegate
        {
            StopNHide();
            onGetReward?.Invoke();
        });
        
        if (showNoThanksButton)
        {
            // DelayHandler.Instance.DelayedCallRealtime(showDelay, delegate
            // {
            //     noThanksButton.gameObject.SetActive(true);
            //     noThanksButton.onClick.AddListener(delegate
            //     {
            //         StopNHide();
            //         onDiscardReward?.Invoke();
            //     });
            // });
        }
        
        timer = maxTime + 1;
        counter = StartCoroutine(Counter());
        IEnumerator Counter()
        {
            while (timer > minTime)
            {
                timer--;
                timerText.text = timer.ToString();
                yield return new WaitForSecondsRealtime(1);
            }
            if (hideOnTimerEnds && !showNoThanksButton)
            {
                onDiscardReward?.Invoke();
                HidePanel();
            }
        }
    }

    private void StopNHide()
    {
        StopCounter();
        HidePanel();
    }

    private void StopCounter()
    {
        if (counter != null)
        {
            StopCoroutine(counter);
            counter = null;
        }
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}