using UnityEngine;
namespace PoolsManagement
{
    public interface IPoolable
    {
        void ReturnToPool();
        void OnPoolableTaken();
        void InitializePoolable(BasePoolSettings poolSet);
        T GetComponent<T>();
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}

