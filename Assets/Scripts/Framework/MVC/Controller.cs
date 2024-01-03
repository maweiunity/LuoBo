using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Controller
{
    // 事件处理函数
    public abstract void Execute(object data);

    // 获取模型
    public Model GetModel<T>() where T : Model
    {
        return MVC.GetModel<T>();
    }

    // 获取视图
    public View GetView<T>() where T : View
    {
        return MVC.GetView<T>();
    }

    // 注册模型
    public void RegisterModel(Model model)
    {
        MVC.RegisterModel(model);
    }

    // 注册视图
    public void RegisterView(View view)
    {
        MVC.RegisterView(view);
    }

    // 注册控制器
    public void RegisterController(string name, Type type)
    {
        MVC.RegisterController(name, type);
    }
}