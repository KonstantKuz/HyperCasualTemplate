using System;
using System.Collections;
using System.Collections.Generic;
using Template.ItemSystems.GiftSystem.UI;
using UnityEngine;

public class giftpaneltest : MonoBehaviour
{
    [SerializeField] private GiftProgressUpdatePanel giftProgressUpdatePanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            giftProgressUpdatePanel.ShowPanel();
        }
    }
}
