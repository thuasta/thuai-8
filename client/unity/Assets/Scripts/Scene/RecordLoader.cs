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
using UnityEngine.Pool;
using TMPro;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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


        public Slider progressBar;
        public TMP_Text statusText;

        private bool isLoading = false;

        // Start is called before the first frame update
        void Start()
        {
            TypeEventSystem.Global.Register<LoadingEvent>(e =>
            {
                PlayRecord();
            });
            /*TypeEventSystem.Global.Register<BattleEndEvent>(e =>
            {
                mTanks.DelAllTanks();
                mBullets.DelAllBullets();
                mMap.DeleteMap();
                BattleStart = true;
            });*/
        }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }

        public IEnumerator StartLoading()
        {
            if (isLoading) yield break;
            isLoading = true;

            if (string.IsNullOrEmpty(SceneData.FilePath))
            {
                Debug.LogError("文件路径未提供！");
                isLoading = false;
                yield break;
            }
            _recordFile = SceneData.FilePath;

            progressBar.value = 0;
            statusText.text = "Loading...";

            JObject recordJsonObject = null;
            bool isLoaded = false;
            Exception loadException = null;

            // 在后台线程加载数据
            Task.Run(() =>
            {
                try
                {
                    var progress = new Progress<float>(value =>
                    {
                        // 通过主线程分发器更新 UI
                        UnityMainThreadDispatcher.Instance.Enqueue(() =>
                        {
                            progressBar.value = value;
                            statusText.text = $"Loading... {value * 100:F2}%";
                        });
                    });
                    recordJsonObject = JsonUtility.LoadRecord(_recordFile, progress);
                }
                catch (Exception e)
                {
                    loadException = e;
                }
                finally
                {
                    isLoaded = true;
                }
            });

            // 等待加载完成
            while (!isLoaded)
            {
                yield return null;
            }

            // 错误处理
            if (loadException != null)
            {
                Debug.LogError($"加载错误: {loadException.Message}");
                isLoading = false;
                yield break;
            }

            if (recordJsonObject == null)
            {
                Debug.LogError("Initialize Failed!");
                isLoading = false;
                yield break;
            }

            _recordArray = (JArray)recordJsonObject["records"];
            if (_recordArray == null)
            {
                Debug.Log("Record file is empty!");
                isLoading = false;
                yield break;
            }

            // 加载完成后的操作
            StartCanvas.SetActive(false);
            GameCanvas.SetActive(true);
            SceneData.GameStage = "Battle";
            TypeEventSystem.Global.Send(new BattleStageEvent());
            isLoading = false;
        }

        private IEnumerator StartLoadingAndPreprocess()
        {
            // 等待加载完成
            yield return StartCoroutine(StartLoading());

            Debug.Log("start preprocess");
            StartCoroutine(PreprocessGameRounds());
        }

        public void PlayRecord()
        {
            mTanks = this.GetModel<Tanks>();
            mBullets = this.GetModel<Bullets>();
            mMap = this.GetModel<Map>();

            //UI
            exitButton.onClick.AddListener(() => OnExitButtonClicked());

            //record
            BattleStart = true;
            _recordInfo = this.GetModel<RecordInfo>();

            Debug.Log("Stsrt Load Record!");
            
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
            Debug.Log("start preprocess");
            StartCoroutine(StartLoadingAndPreprocess());
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
        private IEnumerator PreprocessGameRounds()
        {
            _gameRounds.Clear();
            List<JObject> currentRound = new List<JObject>();
            int currentEpisode = 0;
            bool updateTickStarted = false;

            int totalCount = _recordArray.Count;

            for (int i = 0; i < totalCount; i++)
            {
                JObject recordObj = (JObject)_recordArray[i];
                currentRound.Add(recordObj);
                bool hasBuffSelect = false;

                JArray record = (JArray)recordObj["record"];
                foreach (JObject message in record)
                {
                    //if (!updateTickStarted && (message["currentStage"]?.ToString() ?? "") == "REST")
                    //{
                    //    currentRound.Remove(recordObj);
                    //}
                    if (message["messageType"].ToString() == "BUFF_SELECT")
                    {
                        hasBuffSelect = true;
                        JArray details = (JArray)message["details"];
                        foreach (JObject info in details)
                        {
                            int id = info["token"].ToObject<int>();
                            string buff = info["buff"].ToString();
                            this.SendCommand(new BuffAddCommand(id, currentEpisode, buff));
                        }
                        ++currentEpisode;
                        break;
                    }
                }

                if (hasBuffSelect)
                {
                    _gameRounds.Add(new List<JObject>(currentRound));
                    currentRound.Clear();

                    if (!updateTickStarted)
                    {
                        Debug.Log("start update ticks");
                        updateTickCoroutine = StartCoroutine(UpdateTick(0));
                        updateTickStarted = true;
                    }
                    _recordInfo.GameRounds = _gameRounds.Count;
                    Debug.Log("Current Counts" + _recordInfo.GameRounds);
                    yield return null;
                }
            }

            if (currentRound.Count > 0)
            {
                _gameRounds.Add(new List<JObject>(currentRound));
                _recordInfo.GameRounds = _gameRounds.Count;
            }

            for (int i = 0; i < _gameRounds.Count; i++)
            {
                int currentIndex = i;
                if (SceneData.GameStage == "Battle")
                {
                    Episodes[i].onClick.AddListener(() => PlaySpecificRound(currentIndex));
                }
                
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

        private static readonly object _syncRoot = new object();
        private static readonly ConditionalWeakTable<TankModel, JToken> _positionHistory = new ConditionalWeakTable<TankModel, JToken>();


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
                //lock (_syncRoot)
                //{
                //    // 获取上次位置并比较
                //    bool hasHistory = _positionHistory.TryGetValue(tank, out JToken lastPosition);
                //    bool shouldMove = !hasHistory || !JToken.DeepEquals(PositionData, lastPosition);

                //    // 更新位置存储（无论是否变化都需要更新）
                //    if (hasHistory) _positionHistory.Remove(tank);
                //    _positionHistory.Add(tank, PositionData.DeepClone()); // 使用深拷贝保证数据隔离

                //    // 根据比较结果发送命令
                //    if (shouldMove)
                //    {
                //        this.SendCommand(new MoveTankCommand(tank));
                //    }
                //    else
                //    {
                //        this.SendCommand(new StopTankCommand(tank));
                //    }
                //}
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
            BuffSeclectPanel.SetActive(true);
            foreach (JObject info in details)
            {
                int id = info["token"].ToObject<int>();
                string buff = info["buff"].ToString();
                Debug.Log("Player" + id + " select a buff");
                Image Player_Buff = GameObject.Find($"Canvas/BuffSelect/Player_{id}_Buff_1")?.GetComponent<Image>();
                Player_Buff.sprite = Resources.Load<Sprite>($"UI/Icons/{buff}");
                Color new_color = Player_Buff.color;
                new_color.a = 1;
                Player_Buff.color = new_color;
                this.SendCommand(new BuffAddCommand(id, currentRound, buff));
            }
            yield return new WaitForSeconds(3);
            BuffSeclectPanel.SetActive(false);
            this.SendCommand(new BuffShowCommand(1, currentRound));
            this.SendCommand(new BuffShowCommand(2, currentRound));
            TypeEventSystem.Global.Send(new BattleEndEvent());
        }


        #endregion

        IEnumerator UpdateTick(int startIndex = 0)
        {
            for (int i = startIndex;i< _gameRounds.Count;i++)
            {
                _recordInfo.CurrentBattle = i;
                foreach (JObject recordObj in _gameRounds[i])
                {
                    while (isPaused)
                    {
                        yield return null; 
                    }
                    if (_recordInfo == null)
                    {
                        Debug.LogError("RecordInfo 未初始化！");
                        yield break; // 或尝试重新初始化
                    }
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
                    yield return new WaitForSecondsRealtime(_recordInfo.FrameTime);
                }
                if (i + 1 >= _gameRounds.Count)
                {
                    int lastCount = _gameRounds.Count;
                    while (_gameRounds.Count == lastCount)
                    {
                        yield return null;
                    }
                }
            }             
        }
    }
}
