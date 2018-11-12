using UnityEngine;

namespace PoolsManagement {
    /// <summary>
    /// Simplest poolable object
    /// </summary>
    public class PoolableObject : MonoBehaviour, IPoolable {

        public delegate void PoolableEvent();

        /// <summary>
        /// Invoked when poolable taken from pool
        /// </summary>
        public event PoolableEvent OnTaken;

        /// <summary>
        /// True if object in pool
        /// </summary>
        [SerializeField]
        public bool IsInPool { get; protected set; }
        protected PoolSettingsSO poolSet;

        /// <summary>
        /// Use it to return object to pool instead destroying
        /// </summary>
        public void ReturnToPool() {
            IsInPool = true;
            gameObject.SetActive(false);
            Pool poolInst = poolSet.GetPool();
            if (poolInst != null) {
                if (transform.parent != poolInst.holder) {
                    transform.SetParent(poolInst.holder);
                }
            }
            poolSet.ReturnToPool(this);
        }

        /// <summary>
        /// Called when iPoolable was taken from pool
        /// </summary>
        public virtual void OnPoolableTaken() {
            IsInPool = false;
            gameObject.SetActive(true);
            if (OnTaken != null) {
                OnTaken.Invoke();
            }
        }

        /// <summary>
        /// Called when new iPoolable was created
        /// </summary>
        public virtual void InitializePoolable(PoolSettingsSO _poolSet) {
            IsInPool = true;
            poolSet = _poolSet;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Just return gameObject
        /// </summary>
        /// <returns></returns>
        public GameObject GetGameObject() {
            return gameObject;
        }
    }
}