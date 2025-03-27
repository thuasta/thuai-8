using BattleCity;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour
{
    [Tooltip("公转中心")]
    public Transform BlackHole;

    [Tooltip("公转速度（度/秒）")]
    public float orbitSpeed = 30f;

    public float changedSpeed;

    private void Start()
    {
        changedSpeed = orbitSpeed;
        BlackHole = GameObject.Find("BlackHole").GetComponent<Transform>();
        TypeEventSystem.Global.Register<BattleEndEvent>(e =>
        {
            Resume();
        });
        TypeEventSystem.Global.Register<SpeedUpEvent>(e =>
        {
            SpeedUp();
        });
        TypeEventSystem.Global.Register<SpeedDownEvent>(e =>
        {
            SpeedDown();
        });
        TypeEventSystem.Global.Register<StopEvent>(e =>
        {
            Stop();
        });
        TypeEventSystem.Global.Register<ResumeEvent>(e =>
        {
            Resume();
        });
    }

    void Update()
    {
        if (BlackHole != null)
        {
            // 绕太阳公转（Y轴为旋转轴，保持平面运动）
            transform.RotateAround(
                BlackHole.position,
                Vector3.up,
                changedSpeed * Time.deltaTime
            );
        }
    }

    void SpeedUp()
    {
        changedSpeed *= 2;
    }

    void SpeedDown()
    {
        changedSpeed /= 2;
    }

    void Stop()
    {
        changedSpeed = 0;
    }

    void Resume()
    {
        changedSpeed = orbitSpeed;
    }
}
