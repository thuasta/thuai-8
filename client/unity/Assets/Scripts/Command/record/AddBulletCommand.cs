using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;

public class AddBulletCommand : AbstractCommand
{
    Bullets bullets;
    JToken bulletData;

    public AddBulletCommand(JToken bullet)
    {
        bulletData = bullet;
    }
    protected override void OnExecute()
    {
        bullets = this.GetModel<Bullets>();
        if (bulletData != null)
        {
            try
            {
                int no = bulletData["no"].ToObject<int>();
                bool isMissile = bulletData["isMissile"].ToObject<bool>();
                bool isAntiArmor = bulletData["isAntiArmor"].ToObject<bool>();

                JToken positionData = bulletData["position"];
                float x = positionData["x"].ToObject<float>();
                float y = positionData["y"].ToObject<float>();
                float angle = positionData["angle"].ToObject<float>();
                Position position = new Position(x, y, angle);

                int speed = bulletData["speed"].ToObject<int>();
                int damage = bulletData["damage"].ToObject<int>();
                float traveledDistance = bulletData["traveledDistance"].ToObject<int>();
                BulletModel newBullet = new BulletModel(no, position, speed, damage, isMissile, isAntiArmor, traveledDistance);
                bullets.AddBulletModel(newBullet);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing bullet data: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"No bullet data");
        }
    }
}
