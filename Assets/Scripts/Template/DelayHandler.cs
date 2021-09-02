using System;
using System.Threading.Tasks;
using UnityEngine;

public class DelayHandler
{
    public static async void DelayedCallAsync(float delay, Action action)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        action.Invoke();
    }
}