using System.Collections.Generic;
using UnityEngine;

namespace PoolsManagement {
    public class Pool {
        public MegaPool megaPool;
        public Transform holder;
        public PoolSettingsSO poolSet;
        public Queue<IPoolable> poolables = new Queue<IPoolable>();

        public Pool(MegaPool _megaPool, PoolSettingsSO _poolSet, Transform _holder) {
            megaPool = _megaPool;
            holder = _holder;
            poolSet = _poolSet;
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
            IPoolable poolable = newObj.GetComponent<PoolableObject>();
            if (poolable == null) {
                poolable = newObj.GetComponent<IPoolable>();
                if (poolable == null) {
                    poolable = newObj.AddComponent<PoolableObject>();
                }
            }

            poolable.InitializePoolable(poolSet);
            return poolable;
        }

    }
}