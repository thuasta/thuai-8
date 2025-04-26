using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BattleCity;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

public class UpdateMapCommand : AbstractCommand
{

    private Map map;
    private JArray walls;
    private JArray fences;
    private JArray traps;
    private JArray laser;

    public UpdateMapCommand(JArray walls, JArray fences, JArray traps, JArray laser)
    {
        this.walls = walls;
        this.fences = fences;
        this.traps = traps;
        this.laser = laser;
    }

    public UpdateMapCommand(JObject mapData)
    {
        walls = (JArray)mapData["walls"];
        fences = (JArray)mapData["fences"];
        traps = (JArray)mapData["traps"];
        laser = (JArray)mapData["laser"];
    }

    protected override void OnExecute()
    {
        map = this.GetModel<Map>();
        UpdateWalls();
        UpdateFences();
        UpdateTraps();
        ShowLaser();
    }

    private void UpdateWalls()
    {
        HashSet<Position> currentWalls = new HashSet<Position>();
        // add wall
        foreach (var wall in walls)
        {
            float x = wall["x"]?.Value<float>() ?? 0f;
            float y = wall["y"]?.Value<float>() ?? 0f;
            float angle = wall["angle"]?.Value<float>() ?? 0f;
                      
            Position position = new Position(x, y, angle);
            currentWalls.Add(position);

            var existingWall = map.CityWall.FirstOrDefault(w => w.wallPos == position);

            if (existingWall == null)
            { 
                map.UpdateWall(position);
            }            
        }

        // delete wall
        List<Wall> toDelete = new();
        foreach (Wall wall in map.CityWall)
        {
            if (!currentWalls.Contains(wall.wallPos))
            {
                toDelete.Add(wall);
            }
        }
        foreach (var wall in toDelete)
        {
            map.RemoveWall(wall);
        }
    }

    private void UpdateFences()
    {
        HashSet<Position> currentFences = new HashSet<Position>();

        // add fence
        foreach (var wall in fences)
        {
            float x = wall["x"]?.Value<float>() ?? 0f;
            float y = wall["y"]?.Value<float>() ?? 0f;
            float angle = wall["angle"]?.Value<float>() ?? 0f;

            Position position = new Position(x, y, angle);
            currentFences.Add(position);

            var existingFence = map.CityFence.FirstOrDefault(w => w.wallPos == position);

            if (existingFence == null)
            {
                map.UpdateFence(position);
            }
        }

        // delete the fence
        List<Wall> toDelete = new();
        foreach (Wall wall in map.CityFence)
        {
            if (!currentFences.Contains(wall.wallPos))
            {
                toDelete.Add(wall);
            }
        }
        foreach (Wall wall in toDelete)
        {
            map.RemoveFence(wall);
        }
    }

    private void UpdateTraps()
    {
        HashSet<Position> currentTraps = new HashSet<Position>();

        // add fence
        foreach (var trap in traps)
        {
            JToken posData = trap["position"];
            bool isActive = trap["isActive"].ToObject<bool>();
            float x = posData["x"]?.Value<float>() ?? 0f;
            float y = posData["y"]?.Value<float>() ?? 0f;
            float angle = posData["angle"]?.Value<float>() ?? 0f;

            Position position = new Position(x, y, angle);
            currentTraps.Add(position);

            var existingTrap = map.Traps.FirstOrDefault(w => w.trapPos == position);

            if (existingTrap == null)
            {
                map.AddTrap(position, isActive);
            }
            else
            {
                map.UpdateTrap(existingTrap, isActive);
            }
        }

        // delete the fence
        List<Trap> toDelete = new();
        foreach (Trap trap in map.Traps)
        {
            if (!currentTraps.Contains(trap.trapPos))
            {
                toDelete.Add(trap);
            }
        }
        foreach (Trap trap in toDelete)
        {
            map.RemoveTrap(trap);
        }
    }

    private void ShowLaser()
    {
        foreach (JToken laserData in laser)
        {
            JToken startData = laserData["start"];
            JToken endData = laserData["end"];
            float startX = startData["x"]?.Value<float>() ?? 0f;
            float startY = startData["y"]?.Value<float>() ?? 0f;
            float startAngle = startData["angle"]?.Value<float>() ?? 0f;
            float endX = endData["x"]?.Value<float>() ?? 0f;
            float endY = endData["y"]?.Value<float>() ?? 0f;
            float endAngle = endData["angle"]?.Value<float>() ?? 0f;
            Position startPos = new(startX, startY, startAngle);
            Position endPos = new(endX, endY, endAngle);
            new Laser(startPos, endPos);
        }
    }
}
