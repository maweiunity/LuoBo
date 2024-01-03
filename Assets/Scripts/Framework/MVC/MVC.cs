using System;
using System.Collections;
using System.Collections.Generic;

public static class MVC
{
    // 存储MVC
    public static Dictionary<string, Model> ModelsDict = new Dictionary<string, Model>(); // 名字--模型
    public static Dictionary<string, View> ViewDict = new Dictionary<string, View>(); // 名字--视图
    public static Dictionary<string, Type> CommandDict = new Dictionary<string, Type>(); // 事件名字--控制器类型

    // 注册模型
    public static void RegisterModel(Model model)
    {
        ModelsDict[model.Name] = model;
    }
    // 注册视图
    public static void RegisterView(View view)
    {
        ViewDict[view.Name] = view;
    }
    // 注册控制器
    public static void RegisterController(string name, Type type)
    {
        CommandDict[name] = type;
    }

    // 获取
    public static Model GetModel<T>()
    where T : Model
    {
        foreach (Model item in ModelsDict.Values)
        {
            if (item is T) return item;
        }
        return null;
    }
    public static View GetView<T>()
    where T : View
    {
        foreach (View item in ViewDict.Values)
        {
            if (item is T) return item;
        }
        return null;
    }

    // 发送事件
    public static void SendEvent(string name, object data = null)
    {
        // 控制器响应事件
        if (CommandDict.ContainsKey(name))
        {
            Type t = CommandDict[name];
            Controller c = Activator.CreateInstance(t) as Controller;
            // 控制器执行
            c.Execute(data);
        }

        // 视图响应事件
        foreach (View item in ViewDict.Values)
        {
            if (item.AttationEvents.Contains(name))
            {
                // 视图执行
                item.HandleEvent(name, data);
            }
        }
    }
}