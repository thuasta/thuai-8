using BattleCity;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour, IController
{
    public GameObject uiElement; // 拖拽你的 UI 对象到这里
    public RecordInfo _recordInfo;
    public Button Episode;
    public GameObject Episodes;
    public Button[] episodes;
    


    void Start()
    {        
        Episode.onClick.AddListener(ShowEpisodes);
        TypeEventSystem.Global.Register<BattleEndEvent>(e =>
        {
            ResetEpisodes();
        });
    }

    void Update()
    {
        HideEsc();
        UpdateCountdown();
        UpdateBattleNum();
    }

    void ResetEpisodes()
    {
        Episodes.SetActive(false);
        for (int i = 0; i < episodes.Length; i++)
        {
            episodes[i].gameObject.SetActive(false);
        }
        _recordInfo.ResetSpeed();
    }

    void ShowEpisodes()
    {
        _recordInfo = this.GetModel<RecordInfo>();
        Episodes.SetActive(!Episodes.activeSelf);
        int gameRounds = _recordInfo.GameRounds;
        // 将所有按钮存入数组，方便批量操作

        // 遍历按钮数组，根据索引设置激活状态
        for (int i = 0; i < episodes.Length; i++)
        {
            // 判断条件：按钮编号（i+1）是否小于等于 gameRounds
            bool isActive = (i + 1) <= gameRounds;
            episodes[i].gameObject.SetActive(isActive);
        }
    }

    void HideEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneData.GameStage == "Battle")
            {
                // 切换 UI 的激活状态
                if (uiElement != null)
                {
                    uiElement.SetActive(!uiElement.activeSelf);
                    Debug.Log("当前 UI 状态: " + uiElement.activeSelf);
                }
            }
        }
    }

    void UpdateCountdown()
    {
        if (SceneData.GameStage == "Battle")
        {
            _recordInfo = this.GetModel<RecordInfo>();
            if (_recordInfo != null)
                this.SendCommand(new CountdownChangeCommand(_recordInfo.BattleTick));
        }
    }

    void UpdateBattleNum()
    {
        if (SceneData.GameStage == "Battle")
        {
            _recordInfo = this.GetModel<RecordInfo>();
            if (_recordInfo != null)
                this.SendCommand(new RoundsChangeCommand(_recordInfo.GameRounds, _recordInfo.CurrentBattle + 1));
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameApp.Interface;
    }
}
