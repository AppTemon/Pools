using System.Collections;
using UnityEngine;
using System;

namespace PoolsManagement
{
    public class PoolManager : MonoBehaviour
    {
        static PoolManager _globalPoolManager;

        public event Action<PoolManager> OnPoolManagerDestroyed;

#pragma warning disable CS0649
        [SerializeField]
        [Tooltip("Initial pool setting")]
        BasePoolSettings[] _initialPools;
#pragma warning restore CS0649

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
                    GetOrCreatePool(poolSet);
                }
            }
        }

        public Pool GetPool(string key)
        {
            return _pools[key] as Pool;
        }

        public Pool GetOrCreatePool(BasePoolSettings poolSettings)
        {
            Pool pool = GetPool(poolSettings.key);
            if (pool != null)
                return pool;
            return CreateNewPool(poolSettings);
        }

        public Pool GetOrCreatePool(string key, GameObject prefab, int initialSize = 0)
        {
            Pool pool = GetPool(key);
            if (pool != null)
                return pool;
            return CreateNewPool(key, prefab, initialSize);
        }

        Pool CreateNewPool(BasePoolSettings poolSettings)
        {
            Pool newPool = CreateNewPool(poolSettings.key, poolSettings.GetPrefab(), poolSettings.initialSize);
            poolSettings.SetPool(newPool);
            return newPool;
        }

        Pool CreateNewPool(string key, GameObject prefab, int initialSize)
        {
            Transform newHolder = new GameObject("Holder_" + key).transform;
            newHolder.SetParent(transform);
            var newPool = new Pool(this, key, prefab, initialSize, newHolder);
            _pools.Add(key, newPool);
            return newPool;
        }

        void OnDestroy()
        {
            _pools.Clear();

            OnPoolManagerDestroyed?.Invoke(this);

            if (_globalPoolManager == this)
                _globalPoolManager = null;
        }
    }
}