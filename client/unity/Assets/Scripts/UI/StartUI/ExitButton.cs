using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{

    public Button exit;
    void Start()
    {
        exit.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // 在控制台输出调试信息（可选）
        Debug.Log("Exit button clicked");

        // 正式构建退出
#if UNITY_EDITOR
        // 如果在Unity编辑器中运行，停止播放模式
        EditorApplication.isPlaying = false;
#else
        // 在发布的版本中退出游戏
        Application.Quit();
#endif
    }
}