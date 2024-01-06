using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // 地图行列
    public const int rowCount = 8;
    public const int colCount = 12;

    // 是否绘制地图格子网格
    public bool IsDrawGizmos = true;

    // 地图宽高
    private float mapWidth, mapHeight;
    // 地图格子块宽高
    private float tileWidth, tileHeight;
    // 格子列表
    private List<Tile> gridList = new();
    // 路径列表
    private List<Tile> roadList = new();
    // 背景图
    public string bgImage
    {
        set
        {
            SpriteRenderer sr = transform.Find("Background").GetComponentInChildren<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, sr));
        }
    }
    // 路径图
    public string roadImage
    {
        set
        {
            SpriteRenderer sr = transform.Find("Road").GetComponentInChildren<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, sr));
        }
    }
    // 格子列表
    public List<Tile> Grid
    {
        get { return gridList; }
    }
    // 路径列表
    public List<Tile> Road
    {
        get { return roadList; }
    }
    // 怪物行走路径位置
    public Vector3[] Path
    {
        get
        {
            List<Vector3> path = new();
            for (int i = 0; i < roadList.Count; i++)
            {
                Tile tile = roadList[i];
                Vector3 pos = GetTileWorldPos(tile);
                path.Add(pos);
            }
            return path.ToArray();
        }
    }
    // 关卡
    private Level level;

    // 初始化
    private void Awake()
    {
        // 初始化格子
        InitGrid();
    }

    // 加载关卡信息
    public void LoadLevel(Level level)
    {
        // 清除当前变量里面的关卡信息
        Clear();

        this.level = level;
        this.bgImage = "file://" + Consts.MapDir + level.Background;
        this.roadImage = "file://" + Consts.MapDir + level.Road;

        // 怪物路径
        for (int i = 0; i < level.Path.Count; i++)
        {
            Point p = level.Path[i];
            Tile tile = GetTile(p.X, p.Y);
            if (tile != null) roadList.Add(tile);
        }

        // 可放置炮塔位置
        for (int i = 0; i < level.Holder.Count; i++)
        {
            Point p = level.Holder[i];
            Tile tile = GetTile(p.X, p.Y);
            tile.canHole = true;
        }
    }

    // 绘制辅助网格,只在编辑器里面执行
    private void OnDrawGizmos()
    {
        // 是否绘制地图格子网格
        if (!IsDrawGizmos) return;

        // 计算地图宽高,格子数量大小
        CalculateSize();

        // 绘制网格
        Gizmos.color = Color.green;
        // 行
        for (int row = 0; row <= rowCount; row++)
        {
            Vector2 from = new(-mapWidth / 2, -mapHeight / 2 + row * tileHeight);
            Vector2 to = new(-mapWidth / 2 + mapWidth, -mapHeight / 2 + row * tileHeight);
            Gizmos.DrawLine(from, to);
        }
        // 列
        for (int col = 0; col <= colCount; col++)
        {
            Vector2 from = new(-mapWidth / 2 + col * tileWidth, -mapHeight / 2);
            Vector2 to = new(-mapWidth / 2 + col * tileWidth, mapHeight / 2);
            Gizmos.DrawLine(from, to);
        }

        // 绘制可放置炮塔的位置
        foreach (Tile item in gridList)
        {
            if (item.canHole)
            {
                Vector3 pos = GetTileWorldPos(item);
                Gizmos.DrawIcon(pos, "holder.png", true);
            }
        }

        // 绘制怪物行走路径
        Gizmos.color = Color.red;
        for (int i = 0; i < roadList.Count; i++)
        {
            // 开始点
            if (i == 0) Gizmos.DrawIcon(GetTileWorldPos(roadList[i]), "start.png", true);
            // 结束点
            else if (i == roadList.Count - 1) Gizmos.DrawIcon(GetTileWorldPos(roadList[i]), "end.png", true);
            // 中间路径
            else if (i > 0 && i < roadList.Count - 1)
            {
                Vector3 from = GetTileWorldPos(roadList[i - 1]);
                Vector3 to = GetTileWorldPos(roadList[i]);
                Gizmos.DrawLine(from, to);
            }
        }
    }

    // 清除地图所有可以放置炮塔的位置
    public void ClearHolder()
    {
        foreach (Tile item in gridList)
        {
            item.canHole = false;
        }
    }

    // 清除怪物行走路径 
    public void ClearRoad()
    {
        roadList.Clear();
    }

    // 清除地图所有
    public void Clear()
    {
        level = null;
        gridList.Clear();
        roadList.Clear();
    }

    // 计算地图，格子大小
    private void CalculateSize()
    {
        // 计算地图宽高
        Vector3 p1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // 左下角
        Vector3 p2 = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)); // 右上角

        mapWidth = p2.x - p1.x;
        mapHeight = p2.y - p1.y;

        tileWidth = mapWidth / colCount;
        tileHeight = mapHeight / rowCount;
    }

    // 获取格子块的中心点的世界坐标
    public Vector3 GetTileWorldPos(Tile tile)
    {
        return new Vector3(
            -mapWidth / 2 + (tile.X + 0.5f) * tileWidth,
            -mapHeight / 2 + (tile.Y + 0.5f) * tileHeight,
            0
            );
    }

    // 根据格子坐标号获取格子
    public Tile GetTile(int tileX, int tileY)
    {
        int idx = tileX + tileY * colCount;
        if (idx < 0 || idx >= gridList.Count)
        {
            return null;
        }

        return gridList[idx];
    }

    // 获取鼠标所在位置的格子坐标
    public Tile GetMouseTilePos()
    {
        Vector2 mousePos = GetMouseWorldPos();
        int tileX = (int)((mousePos.x + mapWidth / 2) / tileWidth);
        int tileY = (int)((mousePos.y + mapHeight / 2) / tileHeight);
        return GetTile(tileX, tileY);
    }

    // 获取鼠标所在位置的世界坐标
    public Vector3 GetMouseWorldPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // 初始化格子
    private void InitGrid()
    {
        // 计算地图宽高
        CalculateSize();

        // 初始化格子列表
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                gridList.Add(new Tile(j, i));
            }
        }
    }
}