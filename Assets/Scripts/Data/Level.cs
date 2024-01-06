using System;
using System.Collections.Generic;

public class Level
{
    // 关卡名称
    public string Name;
    // 背景
    public string Background;
    // 路径
    public string Road;
    // 金币
    public int InitScore;
    // 可放置炮塔位置
    public List<Point> Holder = new List<Point>();
    // 怪物行走线路
    public List<Point> Path = new List<Point>();
    // 出怪回合信息
    public List<Round> Rounds = new List<Round>();
}