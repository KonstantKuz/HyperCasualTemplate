using System;
using Templates.Tools;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierIndicator_Slider : MonoBehaviour
{
    [SerializeField] private int _maxAngle;
    [SerializeField] private int _angleStep;
    [SerializeField] private Slider _angleSlider;
    
    private bool _isActive;
    private int _multiplierValue = 1;
    private int _prevMultiplierValue;

    public Action<int> OnMultiplierValueChanged;

    public void StartCount()
    {
        _isActive = true;
    }

    public void StopCount()
    {
        _isActive = false;
    }
    
    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        
        UpdateSliderValue();
        UpdateMultiplierValue();
    }

    private void UpdateSliderValue()
    {
        _angleSlider.value += _angleStep;
        
        if (Mathf.Abs(_angleSlider.value) >= _maxAngle)
        {
            _angleStep = -_angleStep;
        }
    }
    
    private void UpdateMultiplierValue()
    {
        float angleValue = _angleSlider.value;
        if (angleValue.IsInRange(-50, 50))
        {
            _multiplierValue = 5;
        }
        else if (angleValue.IsInRange(-100, -50) || angleValue.IsInRange(50, 100))
        {
            _multiplierValue = 4;
        }
        else if (angleValue.IsInRange(-140, -100) || angleValue.IsInRange(100, 140))
        {
            _multiplierValue = 3;
        }
        else
        {
            _multiplierValue = 2;
        }

        if (_multiplierValue != _prevMultiplierValue)
        {
            OnMultiplierValueChanged?.Invoke(_multiplierValue);
        }

        _prevMultiplierValue = _multiplierValue;
    }
}
