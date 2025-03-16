using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using QFramework;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleCity
{
    public class RecordLoader : MonoBehaviour, IController
    {
        private Tanks mTanks;
        private Bullets mBullets;
        private Map mMap;

        private JArray _recordArray;
        public string _recordFile;
        public RecordInfo _recordInfo;

        private bool BattleStart;

        private Coroutine updateTickCoroutine; // 协程引用存储
        public Button exitButton;
        public GameObject BuffSeclectPanel;

        // Start is called before the first frame update
        void Start()
        {
            //model
            mTanks = this.GetModel<Tanks>();
            mBullets = this.GetModel<Bullets>();
            mMap = this.GetModel<Map>();

            //UI
            exitButton = GameObject.Find("Canvas/Exit").GetComponent<Button>();
            exitButton.onClick.AddListener(() => OnExitButtonClicked());

            //record
            BattleStart = true;
            _recordInfo = this.GetModel<RecordInfo>();
            if (!string.IsNullOrEmpty(SceneData.FilePath))
            {
                _recordFile = SceneData.FilePath;
            }
            else
            {
                Debug.LogError("文件路径未提供！");
            }
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
            updateTickCoroutine = StartCoroutine(UpdateTick());
        }

        public void OnExitButtonClicked()
        {
            // 停止正在运行的协程
            if (updateTickCoroutine != null)
            {
                StopCoroutine(updateTickCoroutine);
                updateTickCoroutine = null;
            }

            // 加载开始场景
            SceneManager.LoadScene("Start"); // 替换为你的开始场景名称
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

        private IEnumerator BuffSelect(JObject buffInfo)
        {
            JArray details = (JArray)buffInfo["details"];
            foreach (JObject info in details)
            {
                int id = info["token"].ToObject<int>();
                string buff = info["buff"].ToString();
                //TODO
            }
            
            BuffSeclectPanel.SetActive(true);
            yield return new WaitForSeconds(3);
            BuffSeclectPanel.SetActive(false);
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
                        case "STAGE_INFO":
                            UpdateStage(message);
                            break;
                        case "BATTLE_UPDATE":
                            UpdateBattle(message);
                            break;
                        case "BUFF_SELECT":
                            yield return BuffSelect(message);
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
