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
        private Tank mTanks;
        private Bullet mBullets;

        private JArray _recordArray;
        private string _recordFile;
        public RecordInfo _recordInfo;


        // Start is called before the first frame update
        void Start()
        {
            mTanks = this.GetModel<Tank>();
            mBullets = this.GetModel<Bullet>();

            FileLoaded fileLoaded = GameObject.Find("RecordReader").GetComponent<FileLoaded>();
            _recordFile = fileLoaded.File;
            if (_recordFile == null)
            {
                Debug.Log("Loading file error!");
                return;
            }
            _recordArray = LoadRecordData();

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

        private void UpdateTanks(JArray tanks)
        {
            if (tanks is null)
                return;

            foreach (JObject tank in tanks)
            {
                int tankId = tank["tankId"].ToObject<int>();
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

            if (!(_recordInfo.NowPlayState == PlayState.Play && _recordInfo.NowTick < _recordInfo.MaxTick))
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
