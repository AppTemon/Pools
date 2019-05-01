using UnityEngine;
using System;

namespace PoolsManagement
{
    public class PoolableObject : MonoBehaviour, IPoolable
    {
        public event Action OnTakenFromPool;

        public string poolKey => _poolKey;
        public Pool pool => _pool;
        public bool isInPool => _isInPool;

        bool _isInPool;
        string _poolKey;
        Pool _pool;

        public void ReturnToPool()
        {
            _isInPool = true;
            gameObject.SetActive(false);
            if (_pool != null)
                _pool.ReturnPoolable(this);
            else
                Destroy(gameObject);
        }

        public virtual void OnPoolableTaken()
        {
            _isInPool = false;
            gameObject.SetActive(true);
            OnTakenFromPool?.Invoke();
        }

        public virtual void InitializePoolable(string key, Pool newPool)
        {
            _isInPool = true;
            _poolKey = key;
            _pool = newPool;
            gameObject.SetActive(false);
        }
    }
}