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

        private bool BattleStart;


        // Start is called before the first frame update
        void Start()
        {
            //model
            mTanks = this.GetModel<Tanks>();
            mBullets = this.GetModel<Bullets>();
            CityMap = this.GetModel<Map>();

            //UI

            //record
            BattleStart = true;
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

        private void UpdateMap(JObject MapData)
        {
            if(BattleStart)
            {
                if (MapData == null || MapData.Count == 0)
                {
                    Debug.LogError("MapData is null or empty!");
                    return;
                }

                JArray wallsArray = (JArray)MapData["walls"];
                if (wallsArray == null)
                {
                    Debug.LogWarning("No walls data found in the record!");
                }

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

                int? mapSize = MapData["mapSize"].ToObject<int>();
                CityMap.setSize(mapSize);

                this.SendCommand(new GenerateMapCommand());
                BattleStart = false;
            }
            else
            {
                this.SendCommand(new UpdateMapCommand(MapData));
            }
            
            
        }

        private void UpdateTanks(JArray tanks)
        {
            if (tanks is null)
                return;

            foreach (JObject tempTank in tanks)
            {
                int tankId = tempTank["token"].ToObject<int>();
                TankModel tank = mTanks.GetTank(tankId);

                JToken WeaponData = tempTank["weapon"];
                JToken ArmorData = tempTank["armor"];
                JToken SkillsData = tempTank["skills"];
                JToken PositionData = tempTank["position"];
                this.SendCommand(new UpdateWeaponCommand(tank, WeaponData));
                this.SendCommand(new UpdateArmorCommand(tank, ArmorData));
                this.SendCommand(new UpdateSkillsCommand(tank, SkillsData));
                this.SendCommand(new UpdatePositionCommand(tank, PositionData));
            }
        }

        private void UpdateBullets(JArray bullets)
        {
            if (bullets is null)
                return;
            HashSet<int> currentBulletIds = new HashSet<int>();

            foreach (JObject BulletData in bullets)
            {
                int bulletId = BulletData["no"].ToObject<int>();
                currentBulletIds.Add(bulletId);
                BulletModel bullet = mBullets.GetBullet(bulletId);
                JToken PositionData = BulletData["position"];
                if (bullet != null)
                {                    
                    this.SendCommand(new UpdatePositionCommand(bullet, PositionData));
                }
                else
                {
                    this.SendCommand(new AddBulletCommand(mBullets, BulletData));
                }
            }

            foreach(int id in mBullets.GetBulletIds())
            {
                if (!currentBulletIds.Contains(id)) 
                {
                    mBullets.DelBulletModel(id); 
                }
            }
        }

        private void BuffActive(JObject ActiveData)
        {
            if (ActiveData.TryGetValue("playerToken", out JToken tokenObj) && tokenObj.ToString() is string tokenString)
            {
                if (int.TryParse(tokenString, out int playerId))
                {
                    string buffName = ActiveData["buffName"].ToObject<string>();
                    this.SendCommand(new BuffActiveCommand(mTanks.GetTank(playerId),buffName));
                }
                else
                {
                    Debug.LogError("cannot turn string playerToken into int !");
                }
            }
            else
            {
                Debug.LogWarning("No actived buff data found in the record!");
            }
        }

        private void BuffDisactive(JObject ActiveData)
        {
            if (ActiveData.TryGetValue("playerToken", out JToken tokenObj) && tokenObj.ToString() is string tokenString)
            {
                if (int.TryParse(tokenString, out int playerId))
                {
                    string buffName = ActiveData["buffName"].ToObject<string>();
                    this.SendCommand(new BuffDisactiveCommand(mTanks.GetTank(playerId), buffName));
                }
                else
                {
                    Debug.LogError("cannot turn string playerToken into int !");
                }
            }
            else
            {
                Debug.LogWarning("No actived buff data found in the record!");
            }
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
                    //UpdateTanks((JArray)_recordArray[_recordInfo.NowRecordNum]["data"]["tanks"]);
                    _recordInfo.NowTick = (int)(_recordArray[_recordInfo.NowRecordNum]["currentTicks"]);
                    JArray events = (JArray)_recordArray[_recordInfo.NowRecordNum]["data"]["events"];
                    if (events != null)
                    {
                        foreach (JObject eventJson in events)
                        {
                            JObject eventJsonInfo = (JObject)eventJson["Json"];
                            switch (eventJson["Json"]["eventType"].ToString())
                            {
                                case "BULLETS_UPDATE_EVENT":
                                    UpdateBullets((JArray)eventJsonInfo["bullets"]);
                                    break;
                                case "PLAYER_UPDATE_EVENT":
                                    UpdateTanks((JArray)eventJsonInfo["player"]);
                                    break;
                                case "MAP_UPDATE_EVENT":
                                    UpdateMap(eventJsonInfo);
                                    break;
                                case "BUFF_ACTIVE_EVENT":
                                    BuffActive(eventJsonInfo);
                                    break;
                                case "BUFF_DISACTIVE_EVENT":
                                    BuffDisactive(eventJsonInfo);
                                    break;
                                default:
                                    Console.WriteLine("Unknown event type: " + eventJson["Json"]["eventType"].ToString());
                                    break;
                            }
                        }
                    }
                }
                _recordInfo.NowRecordNum++;
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
