using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    // 预设
    public GameObject Prefab;

    // 子对象池
    public List<GameObject> ObjectList = new();

    // 名称
    public string Name
    {
        get { return Prefab.name; }
    }
    // 构造
    public SubPool(GameObject prefab)
    {
        this.Prefab = prefab;
    }

    // 取对象
    public GameObject Spawn()
    {
        GameObject gObj = null;

        // 提取对象
        if (ObjectList.Count > 0)
        {
            foreach (GameObject item in ObjectList)
            {
                if (!item.activeSelf)
                {
                    gObj = item;
                    break;
                }
            }
        }
        else
        {
            // 没有对象，创建一个
            gObj = GameObject.Instantiate(Prefab);
            // 添加到对象池
            ObjectList.Add(gObj);
        }

        // 激活
        gObj.SetActive(true);
        // 发送消息,让对象自己去做
        gObj.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

        return gObj;
    }

    // 回收
    public void Unspawn(GameObject gObj)
    {
        if (ObjectList.Contains(gObj))
        {
            // 发送消息,回收
            gObj.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);
            // 禁用
            gObj.SetActive(false);
        }
    }

    // 回收所有
    public void UnspawnAll()
    {
        foreach (GameObject item in ObjectList)
        {
            if (item.activeSelf)
            {
                Unspawn(item);
            }
        }
    }
}
