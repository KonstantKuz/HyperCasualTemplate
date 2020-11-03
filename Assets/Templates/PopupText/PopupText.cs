using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupText : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private TextMeshProUGUI popupText = null;

    public void SetPopup(string text, Color color)
    {
        popupText.text = text;
        popupText.color = color;
        animator.CrossFadeInFixedTime("Popup", 0,0,0);
    }
}
