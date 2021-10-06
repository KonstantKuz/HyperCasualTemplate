using System;
using System.Collections;
using System.Collections.Generic;
using Template.ItemSystems.GiftSystem.UI;
using UnityEngine;

public class giftpaneltest : MonoBehaviour
{
    [SerializeField] private GiftPanel _giftPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _giftPanel.ShowPanel(null);
            _giftPanel.ShowBoostOrUnlockButton();
        }
    }
}
