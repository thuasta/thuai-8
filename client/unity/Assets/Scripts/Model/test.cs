using BattleCity;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class test : MonoBehaviour,IController
{
    Map cityMap;

    [System.Serializable]
    public class WallData
    {
        public int x;
        public int y;
        public int angle;
    }

    private string jsonFilePath;

    [System.Serializable]
    public class WallsData
    {
        public WallData[] walls;
        public int mapSize;
    }

    private void Start()
    {
        cityMap = this.GetModel<Map>();
        // JSON 文件路径
        jsonFilePath = Path.Combine("Assets/Scripts/", "MapInfo.json");

        // 读取和解析 JSON 数据
        ReadWallsFromJson();
        //this.SendCommand(new GenerateMapCommand());
    }
    public IArchitecture GetArchitecture()
    {
        return GameApp.Interface;
    }

    private void ReadWallsFromJson()
    {
        if (File.Exists(jsonFilePath))
        {
            // 读取文件内容
            string jsonContent = File.ReadAllText(jsonFilePath);

            // 解析 JSON 数据
            WallsData wallsData = UnityEngine.JsonUtility.FromJson<WallsData>(jsonContent);

            // 输出解析的墙壁数据
            foreach (var wall in wallsData.walls)
            {
                Debug.Log($"{wall.x}, {wall.y}, {wall.angle}");
                Position position = new(wall.x,wall.y,wall.angle);
                cityMap.AddWall(position);
            }

            cityMap.setSize(wallsData.mapSize);
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }
}
