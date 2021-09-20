using System.Collections;
using System.Collections.Generic;
// using DG.Tweening;
using UnityEngine;
using DG.Tweening;

public class CoinAnimator : Singleton<CoinAnimator>
{
    private const string PoolCoinUI = "CoinUI";
    
    private int defaultCoinsCount = 15;
    public void SpawnMovingCoins(Transform fromTransform, Transform toTransform, float duration)
    {
        StartCoroutine(SpawnCoins());
        IEnumerator SpawnCoins()
        {
            for (int i = 0; i < defaultCoinsCount; i++)
            {
                Transform coin = ObjectPooler.Instance.SpawnObject(PoolCoinUI).transform;
                coin.SetParent(transform);
                coin.transform.position = fromTransform.position;
                coin.transform.DOMove(toTransform.position, duration / (defaultCoinsCount / 2));
                ObjectPooler.Instance.DelayedReturnObject(coin.gameObject, coin.gameObject.name, duration / (defaultCoinsCount / 2));
                yield return new WaitForSeconds(duration / defaultCoinsCount);
            }
        }
    }
    
    public void SpawnMovingCoins(Transform fromTransform, Transform toTransform, int coinsCount, float duration)
    {
        StartCoroutine(SpawnCoins());
        IEnumerator SpawnCoins()
        {
            for (int i = 0; i < coinsCount; i++)
            {
                Transform coin = ObjectPooler.Instance.SpawnObject(PoolCoinUI).transform;
                coin.SetParent(transform);
                coin.transform.position = fromTransform.position;
                coin.transform.DOMove(toTransform.position, duration / (coinsCount / 2));
                ObjectPooler.Instance.DelayedReturnObject(coin.gameObject, coin.gameObject.name, duration / (coinsCount / 2));
                yield return new WaitForSeconds(duration / coinsCount);
            }
        }
    }
}