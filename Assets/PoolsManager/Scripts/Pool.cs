using System.Collections.Generic;
using UnityEngine;

namespace PoolsManagement
{
    public class Pool
    {
        PoolManager _poolManager;
        Transform _holder;
        BasePoolSettings _poolSet;
        Stack<IPoolable> _poolledObjects = new Stack<IPoolable>();

        private readonly bool _prefabHasIPoolable;

        public Pool(PoolManager poolManager, BasePoolSettings poolSet, Transform holder)
        {
            _poolManager = poolManager;
            _holder = holder;
            _poolSet = poolSet;
            _prefabHasIPoolable = _poolSet.GetPrefab().GetComponent<IPoolable>() != null;
            _poolManager.OnPoolManagerDestroyed += OnPoolManagerDestroyed;
            for (int i = 0; i < _poolSet.initialSize; i++) 
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
            GameObject newObj = GameObject.Instantiate(_poolSet.GetPrefab(), _holder, false);
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
            poolable.InitializePoolable(_poolSet);
            return poolable;
        }

        void OnPoolManagerDestroyed(PoolManager poolManager)
        {
            if (poolManager == _poolManager)
                if (_poolSet.pool == this)
                    _poolSet.OnPoolDestroyed();
        }
    }
}
