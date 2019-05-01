using UnityEngine;
using System.Collections;

namespace PoolsManagement
{
    public abstract class BasePoolSettings : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Key for acces to new poolable object via MegaPool")]
        string _key;
        [SerializeField]
        [Tooltip("Initial pool size")]
        int _initialSize;

        public string key => _key;
        public int initialSize => _initialSize;
        public Pool pool => _pool;

        private Pool _pool;
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
            _pool = newPool;
        }

        public virtual void OnPoolDestroyed()
        {
            _pool = null;
        }

        Pool GetOrCreatePool()
        {
            if (_pool != null)
                return _pool;

            var globalPoolManager = PoolManager.GetGlobalPoolManager();
            _pool = globalPoolManager.GetPool(this);

            return _pool;
        }

        private void OnDisable()
        {
            _pool = null;
        }
    }
}