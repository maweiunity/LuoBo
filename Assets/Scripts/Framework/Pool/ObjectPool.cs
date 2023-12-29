using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    // 对象池
    public string ResourceDir = "";

    // 子对象池
    public Dictionary<string, SubPool> ObjectPoolDict = new();

    // 取对象
    public GameObject Spawn(string name)
    {
        SubPool subPool = null;

        if (!ObjectPoolDict.ContainsKey(name))
        {
            ObjectPoolDict.Add(name, subPool);
        }

        subPool = ObjectPoolDict[name];
        return subPool.Spawn();
    }

    // 回收
    public void Unspawn(GameObject gObj)
    {
        SubPool subPool = null;

        foreach (SubPool item in ObjectPoolDict.Values)
        {
            if (item.Prefab == gObj)
            {
                subPool = item;
                break;
            }
        }

        subPool.Unspawn(gObj);
    }

    // 回收所有
    public void UnspawnAll()
    {
        foreach (SubPool item in ObjectPoolDict.Values)
        {
            item.UnspawnAll();
        }
    }

    // 创建子对象池
    private void _createSubPool(string name)
    {
        // 路径 
        string path;
        if (string.IsNullOrEmpty(ResourceDir))
        {
            path = name;
        }
        else
        {
            path = ResourceDir + "/" + name;
        }

        // 加载资源
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.Log("没有找到资源：" + path);
        }

        // 创建
        ObjectPoolDict.Add(name, new SubPool(prefab));
    }
}
