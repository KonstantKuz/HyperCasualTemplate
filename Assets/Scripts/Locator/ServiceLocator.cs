using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System;

public interface IService
{
    void RegisterService();
}

public interface ServiceRequester
{
    void CacheNecessaryService();
}

public class ServiceLocator : Singleton<ServiceLocator>
{
    private readonly Dictionary<string, IService> services = new Dictionary<string, IService>();

    private void Awake()
    {
        CallRegistrationOnServices();
    }

    private void CallRegistrationOnServices()
    {
        Service[] services = FindObjectsOfType<Service>();
        for (int i = 0; i < services.Length; i++)
        {
            services[i].RegisterService();
        }
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        Debug.LogWarning($"<color=red> {key} has been requested from ServiceLocator. </color>");
        return (T)services[key];
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        
        if (services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
            return;
        }

        Debug.LogWarning($"<color=green> Added new service : {key} </color>");
        services.Add(key, service);
    }
    
    public void Unregister<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
            return;
        }

        services.Remove(key);
    }
}
