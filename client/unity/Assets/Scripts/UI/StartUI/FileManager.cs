using BattleCity;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    private Button AddFile;
    private Button Remove;
    private Button Exit;
    public GameObject GameCanvas;
    public GameObject StartCanvas;
    public GameObject RecordPlay;
    public static List<string> SelectedFilePaths { get; private set; } = new List<string>();

    public Transform contentParent; // ScrollView的Content对象
    public GameObject fileButtonPrefab; // 文件按钮预制体
    private bool isRemovingMode = false; // 新增删除模式标志
    private Image removeButtonImage; // 新增Image组件引用
    private Color originalColor = Color.white; // 初始颜色
    public CameraController cameraController;


    void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraController = mainCamera.GetComponent<CameraController>();
        }
        StartCanvas = GameObject.Find("StartCanvas");
        AddFile = GameObject.Find("StartCanvas/Window/AddFile").GetComponent<Button>();
        Remove = GameObject.Find("StartCanvas/Window/Remove").GetComponent<Button>();
        Exit = GameObject.Find("StartCanvas/Window/Exit").GetComponent<Button>();
        contentParent = GameObject.Find("StartCanvas/Window/Scroll View/Viewport/Content").GetComponentInParent<Transform>();
        fileButtonPrefab = Resources.Load<GameObject>("UI/Buttons/recordButton");
        Transform backgroundChild = Remove.transform.Find("Background");

        removeButtonImage = backgroundChild.GetComponent<Image>();

        originalColor = removeButtonImage.color;
        Remove.targetGraphic = removeButtonImage;

        SelectedFilePaths.Add("Assets/Scripts/Test.json");
        UpdateFileListUI();

        AddFile.onClick.AddListener(() =>
        {
            StartCoroutine(SelectFileAndUpdate());
        });
        Remove.onClick.AddListener(() =>
        {
            isRemovingMode = !isRemovingMode;
            UpdateRemoveButtonColor();
        });
        Exit.onClick.AddListener(() => ExitFileManager());
    }

    IEnumerator SelectFileAndUpdate()
    {
        yield return FileSelect.SelectFile(SelectedFilePaths);
        UpdateFileListUI();
    }

    void UpdateFileListUI()
    {
        // 清空旧列表
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        Debug.Log($"the length of FilePaths: {SelectedFilePaths.Count}");
        // 创建新列表
        foreach (string filePath in SelectedFilePaths)
        {
            GameObject buttonObj = Instantiate(fileButtonPrefab, contentParent);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            Transform textChild = buttonObj.transform.Find("Background/Label");
            textChild.GetComponentInChildren<Text>().text = fileName;

            // 添加点击事件
            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (isRemovingMode)
                {
                    // 删除操作
                    SelectedFilePaths.Remove(filePath);
                    UpdateFileListUI(); // 立即刷新UI
                }
                else
                {
                    // 选择操作
                    OnFileSelected(filePath);
                }
            });
        }
    }
    void UpdateRemoveButtonColor()
    {
        removeButtonImage.color = isRemovingMode ? Color.red : originalColor;
        
        Debug.Log(removeButtonImage.color);
        Remove.OnPointerExit(null); // 触发状态更新
        if (removeButtonImage.canvasRenderer != null)
        {
            removeButtonImage.canvasRenderer.SetColor(removeButtonImage.color);
        }
    }
    void ExitFileManager()
    {
#if UNITY_EDITOR
        // 如果在Unity编辑器中运行，停止播放模式
        EditorApplication.isPlaying = false;
#else
        // 在发布的版本中退出游戏
        Application.Quit();
#endif
    }
    void OnFileSelected(string filePath)
    {
        SceneData.FilePath = filePath;
        StartCanvas.SetActive(false);
        GameCanvas.SetActive(true);
        SceneData.GameStage = "Battle";
        TypeEventSystem.Global.Send( new BattleStageEvent());
        RecordPlay.SetActive(true);
        cameraController.enabled = !cameraController.enabled;
        // 切换到test_Game场景
        // SceneManager.LoadScene("test_Game");
    }
}

public static class SceneData
{
    public static string FilePath { get; set; }
    public static string GameStage {  get; set; }
}

