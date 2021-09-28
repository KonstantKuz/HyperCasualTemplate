﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Template.UI.Shop
{
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
}
