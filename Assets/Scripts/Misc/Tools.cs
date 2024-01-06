using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Tools
{
    // 获取关卡文件列表
    public static List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Consts.LevelDir, "*.xml");

        List<FileInfo> levelFiles = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            levelFiles.Add(file);
        }

        return levelFiles;
    }

    // 读取关卡文件
    public static string ReadLevelFile(string fileName, ref Level level)
    {
        FileInfo file = new FileInfo(fileName);
        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(sr.ReadToEnd());
        // 关卡名称
        level.Name = xmlDoc.SelectSingleNode("Level/Name").InnerText;
        // 背景
        level.Background = xmlDoc.SelectSingleNode("Level/Background").InnerText;
        // 路径
        level.Road = xmlDoc.SelectSingleNode("Level/Road").InnerText;
        // 金币
        level.InitScore = int.Parse(xmlDoc.SelectSingleNode("Level/InitScore").InnerText);

        // 可放置炮塔位置
        XmlNodeList nodes = xmlDoc.SelectNodes("Level/Holder/Point");
        level.Holder = new List<Point>();
        foreach (XmlNode node in nodes)
        {
            Point p = new Point(
            int.Parse(node.Attributes["X"].Value),
            int.Parse(node.Attributes["Y"].Value)
            );
            level.Holder.Add(p);
        }

        // 怪物行走线路
        nodes = xmlDoc.SelectNodes("Level/Path/Point");
        level.Path = new List<Point>();
        foreach (XmlNode node in nodes)
        {
            Point p = new Point(
            int.Parse(node.Attributes["X"].Value),
            int.Parse(node.Attributes["Y"].Value)
            );
            level.Path.Add(p);
        }

        // 出怪回合信息
        nodes = xmlDoc.SelectNodes("Level/Rounds/Point");
        level.Rounds = new List<Round>();
        foreach (XmlNode node in nodes)
        {
            Round p = new Round(
             int.Parse(node.Attributes["X"].Value),
             int.Parse(node.Attributes["Y"].Value)
            );
            level.Rounds.Add(p);
        }

        return level.Name;
    }

    public static IEnumerator LoadImage(string path, SpriteRenderer sr)
    {
        UnityWebRequest web = new(path);

        while (!web.isDone)
            yield return web.SendWebRequest();

        Texture2D texture = ((DownloadHandlerTexture)web.downloadHandler).texture;
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
            );
        sr.sprite = sprite;
    }
}