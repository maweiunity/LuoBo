using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    // 地图
    [HideInInspector]
    public Map map;

    // 关卡文件列表
    List<FileInfo> levelFiles = new();

    // 当前选择的文件序号
    int curIdx = 0;

    // 绘制地图编辑组件
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // 转换为Map
        Map map = (Map)target;

        if (EditorApplication.isPlaying)
        {
            // 添加读取关卡文件-组件
            EditorGUILayout.BeginHorizontal();
            int idx = EditorGUILayout.Popup(curIdx, GetLevelFilesName(levelFiles));
            if (idx != curIdx) // 切换地图文件
            {
                curIdx = idx;
                LoadLevelFiles();
            }
            if (GUILayout.Button("读取列表"))
            {
                LoadLevel();
            }
            EditorGUILayout.EndHorizontal();

            // 清除地图
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("清除塔点"))
            {
                map.ClearHolder();
            }
            if (GUILayout.Button("清除路径"))
            {
                map.ClearRoad();
            }
            EditorGUILayout.EndHorizontal();

            // 保存数据
            if (GUILayout.Button("保存数据"))
            {
                SaveLevel();
            }
        }

        // 重载组件 
        if (GUI.changed)
        {
            EditorUtility.SetDirty(map);
        }
    }

    // 加载关卡文件
    public void LoadLevel()
    {
        // 读取关卡文件
        FileInfo file = levelFiles[curIdx];
        Level level = new();
        Tools.ReadLevelFile(file.FullName, ref level);
        // 加载关卡
        map.LoadLevel(level);
    }

    // 获取关卡文件列表
    private void LoadLevelFiles()
    {
        // 清除数据
        Clear();
        // 获取关卡文件列表
        levelFiles = Tools.GetLevelFiles();
    }

    // 保存数据
    private void SaveLevel()
    {

    }

    // 消除数据
    private void Clear()
    {
        curIdx = -1;
        levelFiles.Clear();
    }

    // 返回关卡文件列表名称
    private string[] GetLevelFilesName(List<FileInfo> levelFiles)
    {
        List<string> levelFilesName = new();
        foreach (FileInfo file in levelFiles)
        {
            levelFilesName.Add(file.Name);
        }
        return levelFilesName.ToArray();
    }
}
