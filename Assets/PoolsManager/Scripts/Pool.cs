using System.Collections.Generic;
using UnityEngine;

namespace PoolsManagement {
    public class Pool{
        public MegaPool megaPool;
        public Transform holder;
        public PoolSettingsSO poolSet;
        public Queue<IPoolable> poolables = new Queue<IPoolable>();

        private readonly bool prefabHasIPoolable;

        public Pool(MegaPool _megaPool, PoolSettingsSO _poolSet, Transform _holder) {
            megaPool = _megaPool;
            holder = _holder;
            poolSet = _poolSet;
            prefabHasIPoolable = (poolSet.prefab.GetComponent<IPoolable>() != null);
            for (int i = 0; i < poolSet.initialSize; i++) {
                poolables.Enqueue(CreateNewPoolable());
            }
        }

        public IPoolable GetPoolable() {
            IPoolable poolable;
            if (poolables.Count > 0) {
                poolable = poolables.Dequeue();
            } else {
                poolable = CreateNewPoolable();
            }
            poolable.OnPoolableTaken();
            return poolable;
        }

        public void ReturnPoolable(IPoolable poolable) {
            poolables.Enqueue(poolable);
        }

        IPoolable CreateNewPoolable() {
            GameObject newObj = GameObject.Instantiate(poolSet.prefab, holder);
            IPoolable poolable = null;
            if (prefabHasIPoolable) {
                poolable = newObj.GetComponent<IPoolable>();
                if (poolable == null) {
                    poolable = newObj.AddComponent<PoolableObject>();
                }
            } else {
                poolable = newObj.AddComponent<PoolableObject>();
            }
            poolable.InitializePoolable(poolSet);
            return poolable;
        }

    }
}