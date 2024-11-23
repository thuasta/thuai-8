using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    public Button Return;
    void Start()
    {
        Return.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("Start");
    }
}