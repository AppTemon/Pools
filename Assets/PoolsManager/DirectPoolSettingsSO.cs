using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PoolsManagement
{
    [CreateAssetMenu(fileName = "PoolSetting", menuName = "Pools/DirectPoolSetting")]
    public class DirectPoolSettingsSO : BasePoolSettings
    {
        [SerializeField]
        [Tooltip("Path to poolable object's prefab")]
        GameObject _prefab;

        public override GameObject GetPrefab()
        {
            return _prefab;
        }

        public override IEnumerator PreparePrefab()
        {
            yield return true;
        }
    }
}