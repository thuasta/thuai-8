using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Unity.VisualScripting;

public class BuffActiveCommand : AbstractCommand
{
    TankModel player;
    string buffName;

    public BuffActiveCommand(TankModel tank, string buff)
    {
        player = tank;
        buffName = buff;
    }
    protected override void OnExecute()
    {
        if (buffName == "BLACK_OUT" || buffName == "SPEED_UP" || buffName == "FLASH" || buffName == "KAMUI" || buffName == "MISS" || buffName == "MISSILE")
        {
            GameObject effectPrefab = null;

            // 加载特效预制件
            effectPrefab = Resources.Load<GameObject>($"Effects/{buffName}");

            if (effectPrefab != null)
            {
                // 实例化特效并将其放置在 player's TankObject 上
                GameObject effectInstance = GameObject.Instantiate(effectPrefab, player.TankObject.transform.position + new Vector3(0,0.2f,0), player.TankObject.transform.rotation, player.TankObject.transform);

                // 可选：设置特效实例的生命周期，假设特效在3秒后销毁
                //GameObject.Destroy(effectInstance, 3f);
            }
            else
            {
                Debug.LogWarning($"特效 {buffName} 未找到!");
            }
        }

        
        /*switch (buffName)
        {
            case "LASER":                
                break;
            case "REFLECT":
                break;
            case "DODGE":
                break;
            case "KNIFE":
                break;
            case "GRAVITY":
                break;
            case "BLACK_OUT":
                break;
            case "SPEED_UP":
                break;
            case "FLASH":
                break;
            case "DESTROY":
                break;
            case "CONSTRUCT":
                break;
            case "TRAP":
                break;
            case "MISSILE":
                break;
            case "KAMUI":
                break;
            default:
                break;

    }*/
       
    }
}