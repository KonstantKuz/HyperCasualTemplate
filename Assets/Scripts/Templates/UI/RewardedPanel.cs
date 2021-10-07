﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Templates.UI
{
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
            gameObject.SetActive(true);

            _onGetReward = onGetReward;
            _onDiscardReward = onDiscardReward;
        
            getRewardButton.onClick.AddListener(OnGetRewardButtonClicked);

            TryShowNoThanksButton();
            StartCountTime();
        }

        private void OnGetRewardButtonClicked()
        {
            Close();
            _onGetReward?.Invoke();
        
            getRewardButton.onClick.RemoveListener(OnGetRewardButtonClicked);
        }

        private void TryShowNoThanksButton()
        {
            if (!showNoThanksButton)
            {
                return;
            }
        
            DelayHandler.Instance.DelayedCallCoroutineRealtime(showDelay, ShowNoThanksButton);
        }

        private void ShowNoThanksButton()
        {
            noThanksButton.gameObject.SetActive(true);
            noThanksButton.onClick.AddListener(OnNoThanksButtonClicked);
        }
    
        private void OnNoThanksButtonClicked()
        {
            Close();
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
    
        private void Close()
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
}