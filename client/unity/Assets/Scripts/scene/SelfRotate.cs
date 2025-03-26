using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Tooltip("自转速度（度/秒）")]
    public float rotationSpeed = 10f;

    void Update()
    {
        // 绕Y轴自转（可根据需要调整旋转轴）
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
