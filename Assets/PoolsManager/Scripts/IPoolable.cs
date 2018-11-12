using UnityEngine;
namespace PoolsManagement {
    public interface IPoolable {

        /// <summary>
        /// Use it to return object to pool instead destroying
        /// </summary>
        void ReturnToPool();
        /// <summary>
        /// Called when iPoolable was taken from pool
        /// </summary>
        void OnPoolableTaken();
        /// <summary>
        /// Called when new iPoolable was created
        /// </summary>
        /// <param name="poolSet"></param>
        void InitializePoolable(PoolSettingsSO poolSet);
        /// <summary>
        /// Same as GetComponent of GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetComponent<T>();
        /// <summary>
        /// Return reference to gameObject
        /// </summary>
        /// <returns></returns>
        GameObject GetGameObject();
    }
}

