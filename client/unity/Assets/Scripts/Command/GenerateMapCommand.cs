using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;

public class GenerateMapCommand : AbstractCommand
{
    private static readonly string[] floorPrefabNames =
        {
            "Floor00",
            "Floor01",
            "Floor02",
            "Floor03",
            "Floor04",
            "Floor05",
            "Floor06",
            "Floor07",
            "Floor08"
        };
    private Map map;
    

    protected override void OnExecute()
    {
        map = this.GetModel<Map>();
        GenerateFloor();
        foreach (Wall wall in map.CityWall)
        {
            wall.CreateWallObject();
        }
    }

    private void GenerateFloor()
    {
        GameObject wallController = GameObject.Find("WallController");
        for (int i = 0; i <= map.MapSize; i++)
        {
            for (int j = 0; j <= map.MapSize; j++)
            {
                Vector3 position = new Vector3((float)i * Constants.FLOOR_LEN + Constants.POS_BIAS, (float)(Constants.YPOS + Constants.Y_BIAS), (float)j * Constants.FLOOR_LEN + Constants.POS_BIAS);
                int randomIndex = Random.Range(0, floorPrefabNames.Length);
                // 根据索引加载对应的预制体
                string prefabPath = "Prefabs/Floor/" + floorPrefabNames[randomIndex];
                GameObject floor = Resources.Load<GameObject>(prefabPath);

                if (floor != null)
                {
                   GameObject floorObject =  Object.Instantiate(floor, position, Quaternion.identity);
                   floorObject.transform.SetParent(wallController.transform);
                   floorObject.transform.localScale *= 20f;

                   map.CityFloors.Add(floorObject);
                }
                else
                {
                    Debug.LogError("Failed to load prefab: " + prefabPath);
                }
                
            }
        }
    }
}
