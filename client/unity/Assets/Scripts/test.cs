using System.Collections;
using System.Collections.Generic;
using BattleCity;
using QFramework;
using UnityEngine;

public class test : MonoBehaviour, IController
{
    // Start is called before the first frame update
    void Start()
    {
        this.SendCommand(new HealthChangeCommand(1, 30));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IArchitecture GetArchitecture()
    {
        return GameApp.Interface;
    }
}
