using BattleCity;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneRevolve : MonoBehaviour
{
    [Tooltip("公转中心")]
    public Transform BlackHole;

    [Tooltip("正常公转速度（度/秒）")]
    public float orbitSpeed = 1f;

    [Tooltip("正常模式持续时间（秒）")]
    public float normalDuration = 200f;

    [Header("速度控制")]
    private float speedFactor = 1f; // 新增时间流逝系数

    [Tooltip("加速模式持续时间（秒）")]
    public float acceleratedDuration = 4f;

    [Tooltip("启用加速")]
    public bool useAcceleration = false;

    private float totalDuration;
    private float speedMultiplier;
    private float initialRadius;
    private float initialAngle;
    private float elapsedTime;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Vector3 velocity = Vector3.zero;



    void Start()
    {
        if (BlackHole == null)
            BlackHole = GameObject.Find("BlackHole").transform;

        Vector3 dir = transform.position - BlackHole.position;
        initialRadius = dir.magnitude;
        initialAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        totalDuration = useAcceleration ? acceleratedDuration : normalDuration;
        speedMultiplier = normalDuration / totalDuration;
        elapsedTime = 0f;

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        TypeEventSystem.Global.Register<BattleEndEvent>(e =>
        {
            ResetPosition();
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
        if (SceneData.GameStage == "Battle")
        {
            Approach();
        }
        else
        {
            JustRevolve();
        }
    }

    void Approach()
    {
        if (elapsedTime >= totalDuration) return;

        elapsedTime += Time.deltaTime * speedFactor;
        float progress = Mathf.Clamp01(elapsedTime / totalDuration);

        // 记录移动前的位置用于计算方向
        Vector3 previousPosition = transform.position;

        // 计算当前轨道参数
        float currentRadius = initialRadius * (1 - progress);
        float currentAngle = initialAngle + orbitSpeed * speedMultiplier * elapsedTime;

        // 更新位置
        float angleRad = currentAngle * Mathf.Deg2Rad;
        Vector3 newPosition = BlackHole.position + new Vector3(
            Mathf.Cos(angleRad),
            0,
            Mathf.Sin(angleRad)
        ) * currentRadius;

        transform.position = newPosition;

        Vector3 moveDirection = newPosition - previousPosition;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }

        

        // 到达中心后停止更新
        if (progress >= 1f)
            enabled = false;
    }

    void ResetPosition()
    {
        // 立即重置位置和旋转
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // 重新计算初始轨道参数
        Vector3 dir = transform.position - BlackHole.position;
        initialRadius = dir.magnitude;
        initialAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        // 重置计时器和启用状态
        elapsedTime = 0f;
        enabled = true;
    }

    void JustRevolve()
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

    // 提供加速开关的公共方法
    public void ToggleAcceleration(bool accelerated)
    {
        useAcceleration = accelerated;
        // 重新初始化参数
        Start();
    }

    void SpeedUp()
    {
        speedFactor *= 2f;
    }

    void SpeedDown()
    {
        speedFactor /= 2f;
    }

    void Stop()
    {
        speedFactor = 0f;
    }

    void Resume()
    {
        // 根据当前模式恢复速度
        speedFactor = useAcceleration ?
            normalDuration / acceleratedDuration :
            1f;
    }
}