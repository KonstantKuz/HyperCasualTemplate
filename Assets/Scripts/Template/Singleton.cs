using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static System.Object _lock = new System.Object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    T[] instances = FindObjectsOfType<T>();
                    if (instances.Length > 1)
                    {
                        Debug.LogError("В сцене найдено несколько синглтонов.");
                    }

                    if (instances[0] == null)
                    {
                        Debug.LogError("В сцене не найдено синглтона" + typeof(T));
                    }
                        
                    _instance = instances[0];
                }
                return _instance;
            }
        }
    }
}