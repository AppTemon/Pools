using UnityEngine;
namespace PoolsManagement
{
    public interface IPoolable
    {
        void ReturnToPool();
        void OnPoolableTaken();
        void InitializePoolable(string key, Pool pool);
        T GetComponent<T>();
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}

