using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class DelayHandler : Singleton<DelayHandler>
{
    public void DelayedCallCoroutine(float delay, Action action)
    {
        StartCoroutine(DelayedCall());
        IEnumerator DelayedCall()
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
    
    public void DelayedCallCoroutineRealtime(float delay, Action action)
    {
        StartCoroutine(DelayedCall());
        IEnumerator DelayedCall()
        {
            yield return new WaitForSecondsRealtime(delay);
            action?.Invoke();
        }
    }
    
    public async void DelayedCallAsync(float delay, Action action)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        action.Invoke();
    }
}