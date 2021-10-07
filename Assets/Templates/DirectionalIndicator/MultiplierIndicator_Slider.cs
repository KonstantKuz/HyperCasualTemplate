using System;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierIndicator_Slider : MonoBehaviour
{
    [SerializeField] private int _maxAngle;
    [SerializeField] private int _angleStep;
    [SerializeField] private Slider _angleSlider;
    
    private bool _isActive;
    private int _finalMultiplier = 1;

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
        
        _angleSlider.value += _angleSlider.value >= _maxAngle ? _angleStep : -_angleStep;

        CheckMultiplier();
    }
    
    private void CheckMultiplier()
    {
        float val = _angleSlider.value;
        if (val >= -50 && val <= 50)
        {
            _finalMultiplier = 5;
        }
        else if ((val >= -100 && val <= -50) || (val <= 100 && val >= 50))
        {
            _finalMultiplier = 4;
        }
        else if ((val >= -140 && val <= -100) || (val <= 140 && val >= 100))
        {
            _finalMultiplier = 3;
        }
        else
        {
            _finalMultiplier = 2;
        }
        
        OnMultiplierValueChanged?.Invoke(_finalMultiplier);
    }
}
