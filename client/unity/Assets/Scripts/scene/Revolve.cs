using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour
{
    [Tooltip("公转中心")]
    public Transform BlackHole;

    [Tooltip("公转速度（度/秒）")]
    public float orbitSpeed = 30f;

    private void Start()
    {
        BlackHole = GameObject.Find("BlackHole").GetComponent<Transform>();
    }

    void Update()
    {
        if (BlackHole != null)
        {
            // 绕太阳公转（Y轴为旋转轴，保持平面运动）
            transform.RotateAround(
                BlackHole.position,
                Vector3.up,
                orbitSpeed * Time.deltaTime
            );
        }
    }
}
