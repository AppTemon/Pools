using UnityEngine;
using System.Collections;

namespace PoolsManagement
{
    public abstract class BasePoolSettings : ScriptableObject
    {
#pragma warning disable CS0649
        [SerializeField]
        [Tooltip("Key for acces to new poolable object via MegaPool")]
        string _key;
        [SerializeField]
        [Tooltip("Initial pool size")]
        int _initialSize;
#pragma warning restore CS0649

        public string key => _key;
        public int initialSize => _initialSize;
        public Pool pool => _pool;

        protected Pool _pool;
        protected GameObject _prefabObject;

        public abstract IEnumerator PreparePrefab();
        public abstract GameObject GetPrefab();

        public IPoolable GetPoolableObject(Transform parentTrans)
        {
            IPoolable poolable = GetPoolableObject();
            poolable.transform.SetParent(parentTrans, false);
            return poolable;
        }

        public IPoolable GetPoolableObject()
        {
            Pool poolInst = GetOrCreatePool();
            return poolInst.GetPoolableObject();
        }

        public void ReturnToPool(IPoolable poolable)
        {
            Pool poolInst = GetOrCreatePool();
            poolInst.ReturnPoolable(poolable);
        }

        public void SetPool(Pool newPool)
        {
            if (newPool == _pool)
                return;

            if (_pool != null)
                _pool.OnPoolDestroyed -= OnPoolDestroyed;

            _pool = newPool;
            _pool.OnPoolDestroyed += OnPoolDestroyed;
        }

        protected virtual void OnPoolDestroyed(Pool destroyedPool)
        {
            destroyedPool.OnPoolDestroyed -= OnPoolDestroyed;
            _pool = null;
        }

        Pool GetOrCreatePool()
        {
            if (_pool != null)
                return _pool;

            var globalPoolManager = PoolManager.GetGlobalPoolManager();
            _pool = globalPoolManager.GetOrCreatePool(this);

            return _pool;
        }

        private void OnDisable()
        {
            if (_pool != null)
                _pool.OnPoolDestroyed -= OnPoolDestroyed;
            _pool = null;
        }
    }
}