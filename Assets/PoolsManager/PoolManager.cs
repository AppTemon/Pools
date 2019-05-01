using System.Collections;
using UnityEngine;
using System;

namespace PoolsManagement
{
    public class PoolManager : MonoBehaviour
    {

        static PoolManager _globalPoolManager;

        public event Action<PoolManager> OnPoolManagerDestroyed;

        [SerializeField]
        BasePoolSettings[] _initialPools;

        Hashtable _pools;

        public static PoolManager GetGlobalPoolManager()
        {
            if (_globalPoolManager == null)
                _globalPoolManager = new GameObject("_GlobalPoolManager", typeof(PoolManager)).GetComponent<PoolManager>();
            return _globalPoolManager;
        }

        void OnEnable()
        {
            StartCoroutine(InitCoroutine());
        }

        IEnumerator InitCoroutine()
        {
            if (_pools != null)
                yield break;

            _pools = new Hashtable();

            if (_initialPools != null)
            {
                foreach (BasePoolSettings poolSet in _initialPools)
                {
                    yield return poolSet.PreparePrefab();
                    GetPool(poolSet);
                }
            }
        }

        public Pool GetPool(BasePoolSettings poolSettings)
        {
            Pool pool = GetPool(poolSettings.key);
            if (pool != null)
                return pool;
            return CreateNewPool(poolSettings);
        }

        public Pool GetPool(string key)
        {
            return _pools[key] as Pool;
        }

        Pool CreateNewPool(BasePoolSettings poolSettings)
        {
            string key = poolSettings.key;
            Transform newHolder = new GameObject("Holder_" + key).transform;
            newHolder.SetParent(transform);
            var newPool = new Pool(this, poolSettings, newHolder);
            _pools.Add(key, newPool);
            if (poolSettings.pool == null)
                poolSettings.SetPool(newPool);
            return newPool;
        }

        void OnDestroy()
        {
            OnPoolManagerDestroyed?.Invoke(this);

            if (_globalPoolManager == this)
                _globalPoolManager = null;
        }
    }
}