using System;
using UnityEngine;

namespace Templates.UI.RewardMultiplier
{
    public class RewardedMultiplierIndicator : MonoBehaviour
    {
        [SerializeField] private MultiplierIndicator_Slider indicatorSlider;

        public void StartCount(Action<int> onMultiplierValueChanged)
        {
            indicatorSlider.StartCount();
            indicatorSlider.OnMultiplierValueChanged += onMultiplierValueChanged;
        }

        public void StopCount()
        {
            indicatorSlider.StopCount();
        }
    }
}
