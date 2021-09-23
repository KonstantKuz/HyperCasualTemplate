using System;
using System.Collections;
using System.Security.Cryptography;
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
    
    private int time;
    public int Time => time;
    private Coroutine timeCounter;

    private Action _onGetReward;
    private Action _onDiscardReward;

    public void ShowPanel(Action onGetReward, Action onDiscardReward)
    {
        // getRewardButton.onClick.RemoveAllListeners();
        // noThanksButton.onClick.RemoveAllListeners();
        gameObject.SetActive(true);

        _onGetReward = onGetReward;
        _onDiscardReward = onDiscardReward;
        
        getRewardButton.onClick.AddListener(OnGetRewardButtonClicked);
        
        if (showNoThanksButton)
        {
            DelayHandler.Instance.DelayedCallCoroutineRealtime(showDelay, ShowNoThanksButton);
        }

        StartCountTime();
    }

    private void OnGetRewardButtonClicked()
    {
        StopNHide();
        _onGetReward?.Invoke();
        
        getRewardButton.onClick.RemoveListener(OnGetRewardButtonClicked);
    }

    private void ShowNoThanksButton()
    {
        noThanksButton.gameObject.SetActive(true);
        noThanksButton.onClick.AddListener(OnNoThanksButtonClicked);
    }
    
    private void OnNoThanksButtonClicked()
    {
        StopNHide();
        _onDiscardReward?.Invoke();
        
        noThanksButton.onClick.RemoveListener(OnNoThanksButtonClicked);
    }

    private void StartCountTime()
    {
        timerText.text = maxTime.ToString();
        time = maxTime + 1;
        timeCounter = StartCoroutine(TimeCounter());
    }

    private IEnumerator TimeCounter()
    {
        while (time > minTime)
        {
            time--;
            timerText.text = time.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        if (hideOnTimerEnds && !showNoThanksButton)
        {
            _onDiscardReward?.Invoke();
            HidePanel();
        }
    }
    
    private void StopNHide()
    {
        StopCounter();
        HidePanel();
    }

    private void StopCounter()
    {
        if (timeCounter != null)
        {
            StopCoroutine(timeCounter);
            timeCounter = null;
        }
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}