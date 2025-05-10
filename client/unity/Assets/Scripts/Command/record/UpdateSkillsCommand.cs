using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;

public class UpdateSkillsCommand : AbstractCommand
{
    TankModel player;
    JToken SkilllsData;

    public UpdateSkillsCommand(TankModel tank, JToken skills)
    {
        player = tank;
        SkilllsData = skills;
    }
    protected override void OnExecute()
    { 
        if (SkilllsData != null)
        {
            foreach (JObject skill in SkilllsData)
            {
                try
                {
                    string skillName = skill["name"].ToString();
                    int maxCooldown = skill["maxCooldown"].ToObject<int>();
                    int currentCooldown = skill["currentCooldown"].ToObject<int>();
                    bool isActive = skill["isActive"].ToObject<bool>();
                    if (currentCooldown != 0)
                    {
                        Debug.LogWarning($"A skill is used");
                    }
                    if (!player.TankSkills.UpdateSkill(skillName, maxCooldown, currentCooldown, isActive))
                    {
                        this.SendCommand(new SkillsAddCommand(player.Id, skillName));
                    }
                    else
                    {
                        this.SendCommand(new SkillsCDChangeCommand(player.Id, skillName, currentCooldown, maxCooldown));
                    }
                }
                catch
                {
                    Debug.LogWarning($"No skill data found for tank {player.Id}");
                }
            }
        }
    }
}