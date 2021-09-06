using System;
using UnityEngine;

public class PlayerPrefsProperty<T>
{
    private readonly string _key;
    private readonly T _defaultValue;
    
    public PlayerPrefsProperty(string key, T defaultValue)
    {
        _key = key;
        _defaultValue = defaultValue;
    }

    public T Value
    {
        get => Load();
        set => Save(value);
    }

    private T Load()
    {
        var paramType = typeof(T);
        if (paramType == typeof(int))
        {
            return (T)(object)PlayerPrefs.GetInt(_key, (int)(object)_defaultValue);
        }
        if (paramType == typeof(bool))
        {
            return (T) (object) (PlayerPrefs.GetInt(_key, (int) (object) _defaultValue) == 1);
        }

        throw new Exception($"Not implemented for type {typeof(T)}");
    }

    private void Save(T value)
    {
        var paramType = typeof(T);
        if (paramType == typeof(int))
        {
            PlayerPrefs.SetInt(_key, (int)(object)value);    
        }
        else
        {
            throw new Exception($"Not implemented for type {typeof(T)}");
        }
    }

}