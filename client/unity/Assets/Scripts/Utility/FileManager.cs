using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    private Button AddFile;
    private Button Remove;
    private Button Exit;
    public GameObject targetCanvas;
    public Button start;
    public Button exit;
    public static List<string> SelectedFilePaths { get; private set; } = new List<string>();

    public Transform contentParent; // ScrollView的Content对象
    public GameObject fileButtonPrefab; // 文件按钮预制体
    private bool isRemovingMode = false; // 新增删除模式标志
    private Image removeButtonImage; // 新增Image组件引用
    private Color originalColor = Color.white; // 初始颜色

    void Start()
    {
        targetCanvas = GameObject.Find("Canvas/Canvas");
        AddFile = GameObject.Find("Canvas/Canvas/AddFile").GetComponent<Button>();
        Remove = GameObject.Find("Canvas/Canvas/Remove").GetComponent<Button>();
        Exit = GameObject.Find("Canvas/Canvas/Exit").GetComponent<Button>();
        contentParent = GameObject.Find("Canvas/Canvas/Scroll View/Viewport/Content").GetComponentInParent<Transform>();
        fileButtonPrefab = Resources.Load<GameObject>("UI/Buttons/recordButton");
        removeButtonImage = Remove.GetComponent<Image>();
        originalColor = removeButtonImage.color;

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
            buttonObj.GetComponentInChildren<TMP_Text>().text = fileName;

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
        Remove.OnPointerExit(null); // 触发状态更新
    }
    void ExitFileManager()
    {
        targetCanvas.SetActive(false);
        gameObject.SetActive(false);
        start.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
    }
    void OnFileSelected(string filePath)
    {
        SceneData.FilePath = filePath;
        // 切换到test_Game场景
        SceneManager.LoadScene("test_Game");
    }
}

public static class SceneData
{
    public static string FilePath { get; set; }
}