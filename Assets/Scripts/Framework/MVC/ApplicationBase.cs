using System;
using UnityEngine;

public abstract class ApplicationBase<T> : Singleton<T> where T : MonoBehaviour
{
    // 注册控制器
    protected void RegisterController(string name, Type type)
    {
        MVC.RegisterController(name, type);
    }

    // 执行消息
    protected void SendEvent(string name, object data = null)
    {
        MVC.SendEvent(name, data);
    }
}
