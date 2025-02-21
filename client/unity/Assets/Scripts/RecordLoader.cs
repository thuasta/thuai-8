using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using QFramework;
using System.IO;
using System.Linq;

namespace BattleCity
{
    public class RecordLoader : MonoBehaviour, IController
    {
        private Tanks mTanks;
        private Bullets mBullets;
        private Map mMap;

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
            mMap = this.GetModel<Map>();

            //UI

            //record
            BattleStart = true;
            _recordInfo = this.GetModel<RecordInfo>();
            _recordFile = Path.Combine("Assets/Scripts/", "BattleInfo.json");
            if (_recordFile == null)
            {
                Debug.Log("Loading file error!");
                return;
            }
            _recordArray = LoadRecordData();
            PlayRecord();

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

        public void PlayRecord()
        {
            StartCoroutine(UpdateTick());
        }



        #region Event Definition

        private void UpdateMap(JObject MapData)
        {
            if(BattleStart)
            {
                this.SendCommand(new GenerateMapCommand(MapData, mMap));
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
                Debug.LogError("Tanks is null!");
            foreach (JObject tempTank in tanks)
            {
                int tankId = tempTank.Value<int>("token");
                TankModel tank = mTanks.GetTank(tankId);
                if (tank == null)
                {
                    mTanks.AddTankModel(tankId);
                    tank = mTanks.GetTank(tankId);
                }

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
            // if (bullets is null)
            //     return;
            HashSet<int> currentBulletIds = new HashSet<int>();

            foreach (JObject BulletData in bullets)
            {
                int bulletId = BulletData.Value<int>("no");
                currentBulletIds.Add(bulletId);
                BulletModel bullet = mBullets.GetBullet(bulletId);
                JToken PositionData = BulletData["position"];
                if (bullet != null)
                {                    
                    this.SendCommand(new UpdatePositionCommand(bullet, PositionData));
                }
                else
                {
                    this.SendCommand(new AddBulletCommand(BulletData));
                }
            }

            var bulletIds = mBullets.GetBulletIds().ToList();
            foreach (int id in bulletIds)
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
                    string buffName = ActiveData["buffName"].ToString();
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

        private void UpdateStage(JObject stageInfo)
        {
            string targetStage = stageInfo["targetStage"].ToString();
            if (!Enum.TryParse(typeof(PlayState), targetStage, true, out var result))
            {
                Debug.LogError("this stage is invaild: " + targetStage);
                result = PlayState.Rest;
            }
            _recordInfo.NowPlayState = (PlayState)result;
            int currentTick = stageInfo["currentTicks"].ToObject<int>();
            _recordInfo.NowTick = currentTick;
        }

        private void UpdateBattle(JObject battleInfo)
        {
            JArray events = (JArray)battleInfo["events"];
            if (events != null)
            {
                foreach (JObject eventJson in events)
                {
                    switch (eventJson["eventType"].ToString())
                    {
                        case "BULLETS_UPDATE_EVENT":
                            UpdateBullets((JArray)eventJson["bullets"]);
                            break;
                        case "PLAYER_UPDATE_EVENT":
                            UpdateTanks((JArray)eventJson["players"]);
                            break;
                        case "MAP_UPDATE_EVENT":
                            UpdateMap(eventJson);
                            break;
                        case "BUFF_ACTIVE_EVENT":
                            BuffActive(eventJson);
                            break;
                        case "BUFF_DISACTIVE_EVENT":
                            BuffDisactive(eventJson);
                            break;
                        default:
                            Debug.LogWarning("Unknown event type: " + eventJson["eventType"].ToString());
                            break;
                    }
                }
            }

        }

        private void BuffSelect(JObject buffInfo)
        {
            int id = buffInfo["token"].ToObject<int>();
            string buff = buffInfo["buff"].ToString();
            
        }


        #endregion

        IEnumerator UpdateTick()
        {                
            foreach (JObject recordObj in _recordArray)
            {
                Debug.Log("NowRecordNum：" + _recordInfo.NowRecordNum);
                JArray record = (JArray)recordObj["record"];
                foreach (JObject message in record)
                {
                    string messageType = message["messageType"].ToString();
                    switch (messageType)
                    {
                        case "STAGE":
                            UpdateStage(message);
                            break;
                        case "BATTLE_UPDATE":
                            UpdateBattle(message);
                            break;
                        case "BUFF_SELECT":
                            BuffSelect(message);
                            break;
                        default:
                            Debug.LogWarning("Unknown message type: " + messageType);
                            break;

                    }
                }
                _recordInfo.NowRecordNum++;
                yield return new WaitForSeconds(RecordInfo.FrameTime);

            }
                       
        }
    }

}
