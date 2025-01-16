using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UIElements;
using QFramework;
using TMPro;
using UnityEngine.UI;

namespace BattleCity
{
    public class RecordLoader : MonoBehaviour, IController
    {
        private Tanks mTanks;
        private Bullets mBullets;
        private Map CityMap;

        private JArray _recordArray;
        private string _recordFile;
        public RecordInfo _recordInfo;


        // Start is called before the first frame update
        void Start()
        {
            //model
            mTanks = this.GetModel<Tanks>();
            mBullets = this.GetModel<Bullets>();
            CityMap = this.GetModel<Map>();

            //UI

            //record
            _recordInfo = this.GetModel<RecordInfo>();

            FileLoaded fileLoaded = GameObject.Find("RecordReader").GetComponent<FileLoaded>();
            _recordFile = fileLoaded.File;
            if (_recordFile == null)
            {
                Debug.Log("Loading file error!");
                return;
            }
            _recordArray = LoadRecordData();
            _recordInfo.MaxTick = (int)_recordArray.Last["currentTicks"];
            GenerateMap(_recordArray);

        }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }

        private JArray LoadRecordData()
        {
            JObject recordJsonObject = JsonUtility.UnzipRecord(_recordFile);
            // Load the record array
            JArray recordArray = (JArray)recordJsonObject["records"];

            if (recordArray == null)
            {
                Debug.Log("Record file is empty!");
                return null;
            }
            return recordArray;
        }



        #region Event Definition

        private void GenerateMap(JArray recordArray)
        {
            if (recordArray == null || recordArray.Count == 0)
            {
                Debug.LogError("Record array is null or empty!");
                return;
            }

            // 遍历 recordArray
            foreach (var record in recordArray)
            {
                // 确保 record 中包含 walls 数据
                JArray wallsArray = (JArray)record["walls"];
                if (wallsArray == null)
                {
                    Debug.LogWarning("No walls data found in the record!");
                    continue;
                }

                // 遍历每个 wall 数据
                foreach (var wall in wallsArray)
                {
                    // 提取 wall 的属性数据
                    float x = wall["x"]?.Value<float>() ?? 0f;
                    float y = wall["y"]?.Value<float>() ?? 0f;
                    float angle = wall["angle"]?.Value<float>() ?? 0f;

                    Debug.Log($"Wall Position: x={x}, y={y}, angle={angle}");

                    // 创建 Position 对象并添加到 cityMap
                    Position position = new Position(x, y, angle);
                    CityMap.AddWall(position);
                }

                // 提取 mapSize（如果存在）
                var mapSizeToken = record["mapSize"];
                if (mapSizeToken != null)
                {
                    int mapSize = mapSizeToken.Value<int>();
                    CityMap.setSize(mapSize);
                }
            }
            this.SendCommand(new GenerateMapCommand());
        }

        private void UpdateTanks(JArray tanks)
        {
            if (tanks is null)
                return;

            foreach (JObject tank in tanks)
            {
                int tankId = tank["token"].ToObject<int>();
                int ammo = tank["ammo"].ToObject<int>();
                this.SendCommand(new AmmoChangeCommand(tankId, 0, ammo));
            }
        }

        private void TankAttackEvent(JObject eventJson)
        {
            int tankId = eventJson["data"]["tankId"].ToObject<int>();
            this.SendCommand(new AttackCommand(tankId));

        }



        #endregion

        private void UpdateTick()
        {
            if (_recordInfo.RecordSpeed < 0)
            {
                return;
            }

            int recordTick = _recordInfo.NowTick;
            while (recordTick == _recordInfo.NowTick)
            {
                if (_recordArray[_recordInfo.NowRecordNum].Value<string>("currentTicks") != null &&
                    _recordArray[_recordInfo.NowRecordNum]["messageType"].ToString() == "COMPETITION_UPDATE")
                {
                    UpdateTanks((JArray)_recordArray[_recordInfo.NowRecordNum]["data"]["tanks"]);
                    _recordInfo.NowTick = (int)(_recordArray[_recordInfo.NowRecordNum]["currentTicks"]);
                    JArray events = (JArray)_recordArray[_recordInfo.NowRecordNum]["data"]["events"];
                    if (events != null)
                    {
                        foreach (JObject eventJson in events)
                        {
                            JObject eventJsonInfo = (JObject)eventJson["Json"];
                            switch (eventJson["Json"]["eventType"].ToString())
                            {
                                case "PLAYER_ATTACK":
                                    TankAttackEvent(eventJsonInfo);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void FixedUpdate()
        {

            if (!(_recordInfo.NowPlayState == PlayState.Battle && _recordInfo.NowTick < _recordInfo.MaxTick))
            {
                return;
            }

            if ((float)(System.DateTime.Now.Ticks - _recordInfo.NowTime) / 1e7 > _recordInfo.NowFrameTime)
            {
                _recordInfo.NowTime = _recordInfo.NowTime + (long)(_recordInfo.NowFrameTime * 1e7);
                UpdateTick();
                _recordInfo.NowDeltaTime = 0;
            }
        }
    }

}
