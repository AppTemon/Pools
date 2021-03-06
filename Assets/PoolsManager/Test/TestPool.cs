using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolsManagement;

public class TestPool : MonoBehaviour
{
    public bool getFromPool;
    public bool returnToPool;

    public BasePoolSettings poolSet;
    public List<PoolableObject> testlist = new List<PoolableObject>();
    public GameObject testComp;

    public void Get()
    {
        IPoolable poolable = poolSet.GetPoolableObject(transform);
        testlist.Add(poolable.GetComponent<PoolableObject>());
        testComp = poolable.gameObject;
    }

    public void Return()
    {
        foreach (var item in testlist)
            item.ReturnToPool();
        testlist.Clear();
    }


    private void Update()
    {
        if (getFromPool)
        {
            Get();
            getFromPool = false;
        }
        if (returnToPool)
        {
            Return();
            returnToPool = false;
        }
    }
}
