﻿using System;
using UnityEngine;

namespace Templates.Tools
{
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
            else if (paramType == typeof(bool))
            {
                int defaultValue = (bool) (object) _defaultValue ? 1 : 0;
                return (T) (object) (PlayerPrefs.GetInt(_key, defaultValue) == 1);
            }
            else if(paramType == typeof(string))
            {
                return (T)(object)PlayerPrefs.GetString(_key, (string)(object)_defaultValue);
            }
            else
            {
                throw new Exception($"Not implemented for type {typeof(T)}");
            }
        }

        private void Save(T value)
        {
            var paramType = typeof(T);
            if (paramType == typeof(int))
            {
                PlayerPrefs.SetInt(_key, (int)(object)value);    
            }
            else if (paramType == typeof(bool))
            {
                int saveValue = (bool) (object) value ? 1 : 0;
                PlayerPrefs.SetInt(_key, saveValue);
            }
            else if (paramType == typeof(string))
            {
                PlayerPrefs.SetString(_key, (string)(object)value);    
            }
            else
            {
                throw new Exception($"Not implemented for type {typeof(T)}");
            }
        }

    }
}