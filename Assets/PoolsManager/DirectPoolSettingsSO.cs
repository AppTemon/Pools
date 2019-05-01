using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PoolsManagement
{
    [CreateAssetMenu(fileName = "PoolSetting", menuName = "Pools/DirectPoolSetting")]
    public class DirectPoolSettingsSO : BasePoolSettings
    {
#pragma warning disable CS0649
        [SerializeField]
        [Tooltip("Path to poolable object's prefab")]
        GameObject _prefab;
#pragma warning restore CS0649

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