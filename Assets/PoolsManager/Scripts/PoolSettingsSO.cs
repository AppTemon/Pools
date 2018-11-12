using UnityEngine;

namespace PoolsManagement {
    [CreateAssetMenu(fileName = "PoolSetting", menuName = "Settings/PoolSetting")]
    public class PoolSettingsSO : ScriptableObject {

        /// <summary>
        /// Key of poolable object to access via MegaPool
        /// </summary>
        [Tooltip("Key for acces to new poolable object via MegaPool")]
        public string key;
        /// <summary>
        /// Reference to poolable prefab
        /// </summary>
        [Tooltip("Prefab of poolable object")]
        public GameObject prefab;
        /// <summary>
        /// Initial pool size
        /// </summary>
        [Tooltip("Initial pool size")]
        public int initialSize = 0;

        /// <summary>
        /// Hash of key field
        /// </summary>
        public int KeyHash { get; private set; }

        private Pool pool;

        /// <summary>
        /// Reference to MegaPool with this poolable object
        /// </summary>
        public static MegaPool generalMegaPool;

        /// <summary>
        /// Get poolable object from pool (pool will be created automatically or get from existing)
        /// </summary>
        /// <param name="parentTrans"></param>
        /// <returns></returns>
        public IPoolable GetNewObject(Transform parentTrans) {
            IPoolable poolable = GetNewObject();
            poolable.GetComponent<Transform>().SetParent(parentTrans);
            return poolable;
        }

        /// <summary>
        /// Get poolable object from pool (pool will be created automatically or get from existing)
        /// </summary>
        public IPoolable GetNewObject() {
            Pool poolInst = GetOrCreatePool();
            return poolInst.GetPoolable();
        }

        /// <summary>
        /// Return poolable object to pool
        /// </summary>
        /// <param name="poolable"></param>
        public void ReturnToPool(IPoolable poolable) {
            Pool poolInst = GetOrCreatePool();
            poolInst.ReturnPoolable(poolable);
        }

        /// <summary>
        /// Set new pool for this type of poolable objects
        /// </summary>
        /// <param name="_pool"></param>
        public void SetPool(Pool _pool) {
            pool = _pool;
        }

        /// <summary>
        /// Reference to pool
        /// </summary>
        /// <returns></returns>
        public Pool GetPool() {
            return pool;
        }

        Pool GetOrCreatePool() {
            if (pool == null) {
                if (generalMegaPool == null) {
                    CreateMegaPool();
                }
                pool = generalMegaPool.GetPoolPart(this);
            }
            return pool;
        }

        static void CreateMegaPool() {
            generalMegaPool = new GameObject("_InstMegaPool", typeof(MegaPool)).GetComponent<MegaPool>();
        }

        /// <summary>
        /// Set reference to instatiated MegaPool (for internal using)
        /// </summary>
        /// <param name="megaPool"></param>
        public static void SetGeneralMegaPool(MegaPool megaPool) {
            generalMegaPool = megaPool;
        }

        private void OnDisable() {
            pool = null;
        }

        private void OnValidate() {
            if (!string.IsNullOrEmpty(key)) {
                KeyHash = key.GetHashCode();
            } else {
                KeyHash = 0;
            }
        }
    }
}