﻿using Templates.LevelManagement;
using TMPro;
using UnityEngine;

namespace Templates.UI
{
    public class LevelProgressBar : SceneLineProgressBar<LevelProgressBar>
    {
        [SerializeField] private TextMeshProUGUI levelNumber;
    
        // Для использования смотреть примеры в папке Templates -> Progress Bars Templates

        private InitialData<LevelProgressBar> initialData;
        private UpdateData<LevelProgressBar> updateData;
    
        private void Awake()
        {
            levelNumber.SetText(LevelManager.Instance.CurrentDisplayLevelNumber.ToString());
        
            Observer.Instance.OnLoseLevel += delegate { gameObject.SetActive(false); };
            Observer.Instance.OnWinLevel += delegate { gameObject.SetActive(false); };
        }

        public void InitializeProgress(float minValue, float maxValue, float currentValue)
        {
            initialData.MinValue = minValue;
            initialData.MaxValue = maxValue;
            initialData.CurrentValue = currentValue;

            Initialize(initialData);
        }

        public void UpdateProgress(float currentValue)
        {
            updateData.CurrentValue = currentValue;
            UpdateCurrentProgress(updateData);
        }
    }
}
