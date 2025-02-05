using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;

public class UpdateWeaponCommand : AbstractCommand
{
    TankModel player;
    JToken WeaponData;

    public UpdateWeaponCommand(TankModel tank, JToken weapon)
    {
        player = tank;
        WeaponData = weapon;
    }
    protected override void OnExecute()
    {
        if (WeaponData != null)
        {
            try
            {
                float attackSpeed = WeaponData["attackSpeed"].ToObject<float>();
                float bulletSpeed = WeaponData["bulletSpeed"].ToObject<float>();
                bool isLaser = WeaponData["isLaser"].ToObject<bool>();
                bool antiArmor = WeaponData["antiArmor"].ToObject<bool>();
                int damage = WeaponData["damage"].ToObject<int>();
                int maxBullets = WeaponData["maxBullets"].ToObject<int>();
                int currentBullets = WeaponData["currentBullets"].ToObject<int>();
                player.TankWeapon.UpdateWeapon(attackSpeed, bulletSpeed, isLaser, antiArmor, damage, maxBullets, currentBullets);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing weapon data for tank {player.Id}: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"No weapon data found for tank {player.Id}");
        }
    }
}
