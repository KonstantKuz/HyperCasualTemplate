using System;
using UnityEngine;

namespace ObjectPoolerSingleton
{
    public class PoolerSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    FindInstance();
                }
            
                return _instance;
            }
        }

        private static void FindInstance()
        {
            T[] instances = FindObjectsOfType<T>();
            if (instances.Length > 1)
            {
                Debug.LogError("В сцене найдено несколько синглтонов.");
            }

            if (instances.Length == 0)
            {
                throw new Exception("В сцене не найдено синглтона " + typeof(T));
            }
        
            Debug.Log($"Set component {typeof(T)} in {instances[0].gameObject.name} GameObject as {typeof(T)} Singleton.");
        
            _instance = instances[0];
        }
    }
}