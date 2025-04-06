using BattleCity;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour, IController
{
    public GameObject uiElement; // ��ק��� UI ��������
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
        // �����а�ť�������飬������������

        // ������ť���飬�����������ü���״̬
        for (int i = 0; i < episodes.Length; i++)
        {
            // �ж���������ť��ţ�i+1���Ƿ�С�ڵ��� gameRounds
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
                // �л� UI �ļ���״̬
                if (uiElement != null)
                {
                    uiElement.SetActive(!uiElement.activeSelf);
                    Debug.Log("��ǰ UI ״̬: " + uiElement.activeSelf);
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
