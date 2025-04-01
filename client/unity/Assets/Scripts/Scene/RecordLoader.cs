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
        private bool isPaused = false;

        private Coroutine updateTickCoroutine; // 协程引用存储
        public Button exitButton;
        public GameObject BuffSeclectPanel;
        public GameObject StartCanvas;
        public GameObject GameCanvas;

        private List<List<JObject>> _gameRounds = new List<List<JObject>>();
        public Button[] Episodes;
        public Button Fast;
        public Button Slow;
        public Button Pause;

        // Start is called before the first frame update
        void Start()
        {
            TypeEventSystem.Global.Register<BattleStageEvent>(e =>
            {
                PlayRecord();
            });
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
                _recordFile = Path.Combine("Assets/Scripts/", "BattleInfo.json");
                Debug.LogError("文件路径未提供！");
            }
            if (_recordFile == null)
            {
                Debug.Log("Loading file error!");
                return;
            }
            _recordArray = LoadRecordData();
            PreprocessGameRounds();
            Fast.onClick.AddListener(() =>
            {
                _recordInfo.FastSpeed();
                TypeEventSystem.Global.Send(new SpeedUpEvent());
            });
            Slow.onClick.AddListener(() =>
            {
                _recordInfo.SlowSpeed();
                TypeEventSystem.Global.Send(new SpeedDownEvent());
            });
            Pause.onClick.AddListener(() =>
            {
                TogglePause();
                if (isPaused)
                    TypeEventSystem.Global.Send(new StopEvent());
                else
                    TypeEventSystem.Global.Send(new ResumeEvent());

            });
            updateTickCoroutine = StartCoroutine(UpdateTick());
        }

        public void PlaySpecificRound(int roundIndex)
        {
            if (roundIndex < 0 || roundIndex >= _gameRounds.Count)
            {
                Debug.LogError($"无效的局索引：{roundIndex}");
                return;
            }

            this.SendCommand(new BuffShowCommand(1, roundIndex - 1));
            this.SendCommand(new BuffShowCommand(2, roundIndex - 1));

            // 停止当前播放
            if (updateTickCoroutine != null)
            {
                StopCoroutine(updateTickCoroutine);
            }
            TypeEventSystem.Global.Send(new BattleEndEvent());

            // 启动新协程
            updateTickCoroutine = StartCoroutine(UpdateTick(roundIndex));
        }

        private void PreprocessGameRounds()
        {
            _gameRounds.Clear();
            List<JObject> currentRound = new List<JObject>();
            bool isFirstRound = true;

            foreach (JObject recordObj in _recordArray)
            {
                // 将当前recordObj加入当前局
                currentRound.Add(recordObj);

                // 检查是否包含BUFF_SELECT消息
                bool hasBuffSelect = false;
                JArray record = (JArray)recordObj["record"];
                foreach (JObject message in record)
                {
                    if (message["messageType"].ToString() == "BUFF_SELECT")
                    {
                        hasBuffSelect = true;
                        JArray details = (JArray)message["details"];
                        foreach (JObject info in details)
                        {
                            int id = info["token"].ToObject<int>();
                            string buff = info["buff"].ToString();
                            //TODO
                            this.SendCommand(new BuffAddCommand(id, currentRound.Count - 1, buff));
                        }
                        break;
                    }
                }

                // 遇到BUFF_SELECT时分割（前8局）
                if (hasBuffSelect)
                {
                    // 第一局需要包含初始数据
                    if (isFirstRound)
                    {
                        _gameRounds.Add(new List<JObject>(currentRound));
                        currentRound.Clear();
                        isFirstRound = false;
                    }
                    else
                    {
                        _gameRounds.Add(new List<JObject>(currentRound));
                        currentRound.Clear();
                    }
                }
            }

            // 添加最后一局（没有BUFF_SELECT）
            if (currentRound.Count > 0)
            {
                _gameRounds.Add(new List<JObject>(currentRound));
            }
            _recordInfo.GameRounds = _gameRounds.Count;
            for (int i = 0; i < _gameRounds.Count; i++)
            {
                int currentIndex = i;
                Episodes[i].onClick.AddListener(() => PlaySpecificRound(currentIndex));
            }
        }

        void TogglePause()
        {
            isPaused = !isPaused;
            Pause.transform.Find("Background/Label").GetComponentInChildren<Text>().text = isPaused ? "Resume" : "Pause";
        }

        public void OnExitButtonClicked()
        {
            // 停止正在运行的协程
            if (updateTickCoroutine != null)
            {
                StopCoroutine(updateTickCoroutine);
                updateTickCoroutine = null;
            }
            mTanks.DelAllTanks();
            mBullets.DelAllBullets();
            mMap.DeleteMap();

            TypeEventSystem.Global.Send(new BattleEndEvent());

            SceneData.GameStage = "Start";
            GameCanvas.SetActive(false);
            StartCanvas.SetActive(true);
            
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
            string targetStage = stageInfo["currentStage"].ToString();
            if (!Enum.TryParse(typeof(PlayState), targetStage, true, out var result))
            {
                Debug.LogError("this stage is invaild: " + targetStage);
                result = PlayState.Rest;
            }
            _recordInfo.NowPlayState = (PlayState)result;
            int currentTick = stageInfo["totalTicks"].ToObject<int>();
            _recordInfo.NowTick = currentTick;
        }

        private void UpdateBattle(JObject battleInfo)
        {
            int BattleTick = battleInfo["battleTicks"].ToObject<int>();
            _recordInfo.BattleTick = BattleTick;
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

        private IEnumerator BuffSelect(JObject buffInfo, int currentRound)
        {
            JArray details = (JArray)buffInfo["details"];
            foreach (JObject info in details)
            {
                int id = info["token"].ToObject<int>();
                string buff = info["buff"].ToString();
                //TODO
                this.SendCommand(new BuffAddCommand(id, currentRound, buff));
            }
            
            BuffSeclectPanel.SetActive(true);
            yield return new WaitForSeconds(3);
            BuffSeclectPanel.SetActive(false);
            this.SendCommand(new BuffShowCommand(1, currentRound));
            this.SendCommand(new BuffShowCommand(2, currentRound));
        }


        #endregion

        IEnumerator UpdateTick(int i = 0)
        {
            for (;i< _gameRounds.Count;i++)
            {
                _recordInfo.CurrentBattle = i;
                foreach (JObject recordObj in _gameRounds[i])
                {
                    while (isPaused)
                    {
                        yield return null; // 暂停时挂起协程
                    }
                    if (_recordInfo == null)
                    {
                        Debug.LogError("RecordInfo 未初始化！");
                        yield break; // 或尝试重新初始化
                    }
                    //Debug.Log("NowRecordNum：" + _recordInfo.NowRecordNum);
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
                                yield return BuffSelect(message, i);
                                break;
                            default:
                                Debug.LogWarning("Unknown message type: " + messageType);
                                break;
                        }
                    }
                    _recordInfo.NowRecordNum++;
                    yield return new WaitForSeconds(_recordInfo.FrameTime);
                }
            }             
        }
    }
}
