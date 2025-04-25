using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;

public class UpdatePositionCommand : AbstractCommand
{
    TankModel player;
    BulletModel bullet;
    JToken positionDate;


    public UpdatePositionCommand(TankModel tank, JToken position)
    {
        player = tank ?? throw new ArgumentNullException(nameof(tank)); ;
        bullet = null;
        positionDate = position;

    }

    public UpdatePositionCommand(BulletModel bullet, JToken position)
    {
        this.bullet = bullet ?? throw new ArgumentNullException(nameof(bullet)); ;
        player = null;
        positionDate = position;
    }
    protected override void OnExecute()
    {
        if (positionDate != null)
        {
            try
            {
                float x = positionDate["x"].ToObject<float>();
                float y = positionDate["y"].ToObject<float>();
                float angle = positionDate["angle"].ToObject<float>();
                if (player != null)
                {
                    
                    player.UpdateTankPosition(x, y, angle);
                }
                else if(bullet != null)
                {                  
                    bullet.UpdateBulletPosition(x, y, angle);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing position data for tank or bullet: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"No position data found for tank or bullet");
        }
    }
}
