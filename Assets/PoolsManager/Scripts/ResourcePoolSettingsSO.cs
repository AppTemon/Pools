using System.Collections;
using UnityEngine;

namespace PoolsManagement
{
    [CreateAssetMenu(fileName = "PoolSetting", menuName = "Pools/ResourcePoolSetting")]
    public class ResourcePoolSettingsSO : BasePoolSettings
    {
        [SerializeField]
        [Tooltip("Path to poolable object's prefab")]
        string _prefabPath;
        [SerializeField]
        [Tooltip("Poolable object's prefab name")]
        string _prefabName;

        public override GameObject GetPrefab()
        {
            if (_prefabObject == null)
                _prefabObject = Resources.Load<GameObject>(_prefabPath + _prefabName);
            return _prefabObject;
        }

        public override IEnumerator PreparePrefab()
        {
            if (_prefabObject != null)
                yield return true;
            ResourceRequest request = Resources.LoadAsync<GameObject>(_prefabPath + _prefabName);
            yield return new WaitUntil(() => _prefabObject != null || request.isDone);
            if (_prefabObject == null)
                _prefabObject = request.asset as GameObject;
        }

        public override void OnPoolDestroyed()
        {
            _prefabObject = null;
            base.OnPoolDestroyed();
        }
    }
}
