using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    // 视图名
    public abstract string Name { get; }
    // 关心的事件列表
    public List<string> AttationEvents = new List<string>();
    // 响应事件处理
    public abstract void HandleEvent(string eventName, object data);

    // 获取模型
    public Model GetModel<T>() where T : Model
    {
        return MVC.GetModel<T>();
    }

    // 接收消息
    public void SendEvent(string name, object data = null)
    {
        MVC.SendEvent(name, data);
    }
}