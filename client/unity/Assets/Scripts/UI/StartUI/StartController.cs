using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{

    public Button start;
    void Start()
    {
        start.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("Game");
    }
}