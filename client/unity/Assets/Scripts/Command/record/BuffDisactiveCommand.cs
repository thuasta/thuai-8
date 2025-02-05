using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;

public class BuffDisactiveCommand : AbstractCommand
{
    TankModel player;
    string buffName;

    public BuffDisactiveCommand(TankModel tank, string buff)
    {
        player = tank;
        buffName = buff;
    }
    protected override void OnExecute()
    {
        switch (buffName)
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
            case "KAMUI":
                break;
            default:
                break;

        }
       
    }
}