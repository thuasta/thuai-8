using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;

public class UpdateArmorCommand : AbstractCommand
{
    TankModel player;
    JToken ArmorData;

    public UpdateArmorCommand(TankModel tank, JToken armor)
    {
        player = tank;
        ArmorData = armor;
    }
    protected override void OnExecute()
    {
        if (ArmorData != null)
        {
            try
            {
                bool canReflect = ArmorData["canReflect"].ToObject<bool>();
                int armorValue = ArmorData["armorValue"].ToObject<int>();
                int health = ArmorData["health"].ToObject<int>();
                bool gravityField = ArmorData["gravityField"].ToObject<bool>();
                string knife = ArmorData["knife"].ToString();
                float dodgeRate = ArmorData["dodgeRate"].ToObject<float>();
                player.TankArmor.UpdateArmor(canReflect, armorValue, health, gravityField, knife, dodgeRate, player.TankObject);

                this.SendCommand(new HealthChangeCommand(player.Id, health));
                this.SendCommand(new ArmorValueChangeCommand(player.Id, armorValue));
                this.SendCommand(new ArmorTypeChangeCommand(player.Id, canReflect));
            }
            catch
            {
                Debug.LogError($"No armor data found for tank {player.Id}");
            }
        }
        else
        {
            Debug.LogWarning($"No armor data found for tank {player.Id}");
        }
    }
}