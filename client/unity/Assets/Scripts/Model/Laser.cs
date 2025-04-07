using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class Laser : ICanGetModel
    {
        public RecordInfo _recordInfo;
        public int currentTick;
        public GameObject LaserObject;

        public Vector3 startPos { get; set; }
        public Vector3 endPos { get; set; }

        public Laser(Position start, Position end)
        {
            _recordInfo = this.GetModel<RecordInfo>();
            currentTick = _recordInfo.BattleTick;
            GameObject wallController = GameObject.Find("WallController");

            // 加载并实例化激光预制体
            GameObject LaserPrefab = Resources.Load<GameObject>("Effects/Laser");
            startPos = new Vector3(
                (float)(start.X + Constants.GENERAL_XBIAS),
                (float)(start.Y + Constants.Y_BIAS + 0.05f),
                (float)(start.Z + Constants.GENERAL_ZBIAS)
            );
            endPos = new Vector3(
                (float)(end.X + Constants.GENERAL_XBIAS),
                (float)(end.Y + Constants.Y_BIAS + 0.05f),
                (float)(end.Z + Constants.GENERAL_ZBIAS)
            );

            LaserObject = Object.Instantiate(LaserPrefab, wallController.transform);

            // 设置激光参数
            // 将世界坐标转换为父物体的本地坐标
            var hovlLaser = LaserObject.GetComponent<Hovl_Laser>();
            if (hovlLaser != null)
            {
                hovlLaser.CustomStartPosition = startPos;
                hovlLaser.CustomEndPosition = endPos;
            }
            else
            {
                Debug.LogError("Hovl_Laser component missing on LaserPrefab!");
            }

            // 添加激光生命周期控制器
            LaserObject.AddComponent<LaserLifecycleController>().Init(this);

            // 注册战斗结束事件
            TypeEventSystem.Global.Register<BattleEndEvent>(e => DestroyLaser());
        }

        public void DestroyLaser()
        {
            if (LaserObject != null)
            {
                Object.Destroy(LaserObject);
                LaserObject = null;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }
    }

    // 激光生命周期控制组件
    public class LaserLifecycleController : MonoBehaviour
    {
        private Laser mOwner;

        public void Init(Laser owner)
        {
            mOwner = owner;
            StartCoroutine(CheckExpiration());
        }

        private IEnumerator CheckExpiration()
        {
            // 每帧检查是否超时
            while (mOwner._recordInfo.BattleTick <= mOwner.currentTick + 8)
            {
                yield return null;
            }
            mOwner.DestroyLaser();
        }
    }
}