using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic; // 添加List所需命名空间
using SimpleFileBrowser;

public class FileSelect : MonoBehaviour
{
    // 单例实例
    public static FileSelect Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 可选：跨场景保持
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static IEnumerator SelectFile(List<string> SelectedFilePaths)
    {
        // 设置文件过滤器（添加.dat、.json、.zip支持）
        FileBrowser.SetFilters(
            true,
            new FileBrowser.Filter("Supported Files", ".dat", ".json", ".zip")
        );

        // 设置默认过滤器为.dat（可选）
        FileBrowser.SetDefaultFilter(".dat");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".rar", ".exe");

        // 添加快捷链接（保持原样）
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        // 启动文件选择协程
        yield return Instance.ShowLoadDialogCoroutine(SelectedFilePaths);
    }

    IEnumerator ShowLoadDialogCoroutine(List<string> SelectedFilePaths)
    {
        // 显示多文件选择对话框
        yield return FileBrowser.WaitForLoadDialog(
            FileBrowser.PickMode.Files,
            true, // 允许多选
            null,
            null,
            "Select Files",
            "Select"
        );

        if (FileBrowser.Success)
        {
            // 清空之前保存的路径
            // SelectedFilePaths.Clear();

            // 保存新选择的路径
            SelectedFilePaths.AddRange(FileBrowser.Result);

            // 处理选中的文件（可保留或移除文件操作代码）
            foreach (string path in SelectedFilePaths)
            {
                Debug.Log("Selected file: " + path);
            }
        }
        else
        {
            Debug.Log("File selection cancelled");
        }
    }
}