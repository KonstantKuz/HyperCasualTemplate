using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private ShopTab tabToOpen;

    public Action<ShopTab> OnClickedOpenTab;
    
    private void Awake()
    {
        button.onClick.AddListener(delegate { OnClickedOpenTab(tabToOpen); });
    }

    public void SetSelected(bool value)
    {
        selectedImage.SetActive(value);
    }
}
