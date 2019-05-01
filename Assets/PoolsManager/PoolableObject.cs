using UnityEngine;
using System;

namespace PoolsManagement
{
    public class PoolableObject : MonoBehaviour, IPoolable
    {
        public event Action OnTakenFromPool;

        public BasePoolSettings poolSettings => _poolSettings;
        public bool isInPool => _isInPool;

        protected bool _isInPool;
        protected BasePoolSettings _poolSettings;

        public void ReturnToPool()
        {
            _isInPool = true;
            gameObject.SetActive(false);
            if (_poolSettings.pool != null)
                _poolSettings.ReturnToPool(this);
            else
                Destroy(gameObject);
        }

        public virtual void OnPoolableTaken()
        {
            _isInPool = false;
            gameObject.SetActive(true);
            OnTakenFromPool?.Invoke();
        }

        public virtual void InitializePoolable(BasePoolSettings poolSettings)
        {
            _isInPool = true;
            _poolSettings = poolSettings;
            gameObject.SetActive(false);
        }
    }
}