using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Templates.UI
{
    public class CoinAnimator : Singleton<CoinAnimator>
    {
        [SerializeField] private float _spawnPeriod;
        [SerializeField] private float _lifeTime;
        [SerializeField] private int _coinsCount;
    
        private const string PoolCoinUI = "CoinUI";
    
        public void SpawnMovingCoins(Transform moveFrom, Transform moveTo)
        {
            StartCoroutine(SpawnCoins(moveFrom, moveTo));
        }
    
        private IEnumerator SpawnCoins(Transform moveFrom, Transform moveTo)
        {
            for (int i = 0; i < _coinsCount; i++)
            {
                Transform coin = ObjectPooler.Instance.SpawnObject(PoolCoinUI).transform;
                coin.SetParent(transform);
                coin.transform.position = moveFrom.position;

                coin.transform.DOMove(moveTo.position, _lifeTime);
                ObjectPooler.Instance.DelayedReturnObject(coin.gameObject, coin.gameObject.name, _lifeTime);
                yield return new WaitForSeconds(_spawnPeriod);
            }
        }
    }
}