using System.Collections.Generic;
using UnityEngine;
using System;

namespace PoolsManagement
{
    public class Pool
    {
        public event Action<Pool> OnPoolDestroyed;

        Transform _holder;
        string _key;
        GameObject _prefab;
        Stack<IPoolable> _poolledObjects = new Stack<IPoolable>();

        private readonly bool _prefabHasIPoolable;

        public Pool(PoolManager poolManager, string newKey, GameObject newPrefab, int initialSize, Transform newHolder)
        {
            _holder = newHolder;
            _key = newKey;
            _prefab = newPrefab;
            _prefabHasIPoolable = _prefab.GetComponent<IPoolable>() != null;
            poolManager.OnPoolManagerDestroyed += OnPoolManagerDestroyed;
            for (int i = 0; i < initialSize; i++)
                _poolledObjects.Push(CreateNewPoolableObject());
        }

        public IPoolable GetPoolableObject()
        {
            IPoolable poolable;
            if (_poolledObjects.Count > 0)
                poolable = _poolledObjects.Pop();
            else 
                poolable = CreateNewPoolableObject();

            poolable.OnPoolableTaken();
            return poolable;
        }

        public void ReturnPoolable(IPoolable poolable)
        {
            _poolledObjects.Push(poolable);
            ReturnToPoolHolder(poolable.transform);
        }

        void ReturnToPoolHolder(Transform trans)
        {
            if (trans.parent != _holder)
                trans.SetParent(_holder, false);
        }

        IPoolable CreateNewPoolableObject()
        {
            GameObject newObj = GameObject.Instantiate(_prefab, _holder, false);
            IPoolable poolable = null;
            if (_prefabHasIPoolable)
            {
                poolable = newObj.GetComponent<IPoolable>();
                if (poolable == null)
                    poolable = newObj.AddComponent<PoolableObject>();
            }
            else
            {
                poolable = newObj.AddComponent<PoolableObject>();
            }
            poolable.InitializePoolable(_key, this);
            return poolable;
        }

        void OnPoolManagerDestroyed(PoolManager destroyedPoolManager)
        {
            _prefab = null;
            destroyedPoolManager.OnPoolManagerDestroyed -= OnPoolManagerDestroyed;
            OnPoolDestroyed?.Invoke(this);
        }
    }
}
