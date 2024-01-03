using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Model
{
    // 模型名
    public abstract string Name { get; }

    // 发送消息
    public void SendEvent(string name, object data = null)
    {
        MVC.SendEvent(name, data);
    }
}