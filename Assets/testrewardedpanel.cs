using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrewardedpanel : MonoBehaviour
{
    [SerializeField] private RewardedPanel _rewardedPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShowPanel();
        }
    }

    private void ShowPanel()
    {
        _rewardedPanel.ShowPanel(IncreaseItemsProgress, Hide);
    }

    private void IncreaseItemsProgress()
    {
        ItemsProgression itemsProgression = ItemsProgressHandler.Instance.ProgressionsDictionary["ItemsProgression"];
        ItemsProgressHandler.Instance.IncreaseItemsProgress(itemsProgression);
      
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
