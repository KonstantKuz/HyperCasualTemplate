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

    public void ShowPanel(Action onGetReward, Action onDiscardReward)
    {
        getRewardButton.onClick.RemoveAllListeners();
        noThanksButton.onClick.RemoveAllListeners();
        timerText.text = maxTime.ToString();
        gameObject.SetActive(true);
        
        getRewardButton.onClick.AddListener(delegate
        {
            StopNHide();
            onGetReward?.Invoke();
        });
        
        if (showNoThanksButton)
        {
            DelayHandler.Instance.DelayedCallCoroutineRealtime(showDelay, delegate
            {
                noThanksButton.gameObject.SetActive(true);
                noThanksButton.onClick.AddListener(delegate
                {
                    StopNHide();
                    onDiscardReward?.Invoke();
                });
            });
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