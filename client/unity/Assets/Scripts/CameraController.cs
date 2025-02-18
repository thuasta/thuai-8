using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using TMPro;

namespace BattleCity
{
    public class CameraController : MonoBehaviour, IController
    {
        private Tanks mTanks;
        private TankModel targetTank;

        public float RotateSpeed;
        public float MoveSpeed;
        public float FreeMoveSpeed;
        public const float FreeMaxPitch = 80;

        public enum CameraStatus { freeCamera = 0, player };
        public CameraStatus _cameraStatus;

        public UnityEngine.Transform initialTransform;
        private int _playerNumber;
        private List<TankModel> _players;

        UnityEngine.Vector3 offset;//相机跟随的偏移量
        public float rotationSpeed;//摄像机旋转速度
        public float zoomSpeed = 1f; // 视野的缩放速度
        float zoom;//滚轮滚动量

        //左右旋转、上下旋转功能:
        public bool isRotating, lookup;
        float mousex, mousey;

        public Vector3 velocity = Vector3.zero;
        public Vector3 lookatPositionvelocity = Vector3.zero;
        public Vector3 lookatPosition;

        // Start is called before the first frame update
        void Start()
        {
            mTanks = this.GetModel<Tanks>();
            _players = new();
            offset = new Vector3(5, 5, 5);
            initialTransform = transform;
            RotateSpeed = 100f;
            rotationSpeed = 75f;
            MoveSpeed = 0.1f;
            FreeMoveSpeed = 10f;
            _cameraStatus = CameraStatus.freeCamera;
            targetTank = null;
        }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }

        void Update()
        {
            if (_cameraStatus == CameraStatus.player)
            {
                Rotate();
                Rollup();
                ExchangeStatus();
                Follow();
            }
            else
            {
                Move();
                ExchangeStatus();
                Zoom();
            }

        }
        void visualAngleReset(Vector3 from, Vector3 to)
        {
            //offset = (from - to) * 8 / (from - to).magnitude;
            offset = new Vector3(10.7f, 28.6f, -10.2f);
        }
        void ExchangeStatus()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Dictionary<int, TankModel> dict = mTanks.GetTankDictCopy();
                _players.Clear();
                foreach (KeyValuePair<int, TankModel> player in dict)
                {
                    _players.Add(player.Value);
                }
                if (_cameraStatus == CameraStatus.player)
                {
                    // Retry target
                    if (_players.Count - 1 >= _playerNumber)
                    {
                        targetTank = _players[_playerNumber];
                        Debug.Log(transform.position);
                        Debug.Log($"target {targetTank.TankObject.transform.position}");
                        //visualAngleReset(transform.position, GetHeadPos(targetTank.TankObject.transform.position));
                        Debug.Log($"after {transform.position}");
                        _playerNumber += 1;
                    }
                    else
                    {
                        _cameraStatus = CameraStatus.freeCamera;
                        _playerNumber = 0;
                    }

                }
                else if (_cameraStatus == CameraStatus.freeCamera && _players.Count != 0)
                {
                    _cameraStatus = CameraStatus.player;
                    targetTank = _players[_playerNumber];
                    //visualAngleReset(transform.position, GetHeadPos(targetTank.TankObject.transform.position));
                    _playerNumber += 1;
                }
            }
        }

        Vector3 GetHeadPos(Vector3 playerPos)
        {
            return new Vector3(playerPos.x, playerPos.y + 1.5f, playerPos.z);
        }
        void Follow()
        {
            //视野缩放
            zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; // 获取滚轮滚动量
            if (zoom != 0) // 如果有滚动
            {
                offset -= zoom * offset;
            }
            offset = new Vector3(5.7f, 14.6f, -5.2f);
            Vector3 targetPosition = targetTank.TankObject.transform.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2);
            transform.LookAt(targetTank.TankObject.transform);
        }

        void Zoom()
        {
            //鼠标滚轮的效果
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Camera.main.fieldOfView <= 100)
                    Camera.main.fieldOfView += 2;
                if (Camera.main.orthographicSize <= 20)
                    Camera.main.orthographicSize += 0.5F;
            }
            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Camera.main.fieldOfView > 2)
                    Camera.main.fieldOfView -= 2;
                if (Camera.main.orthographicSize >= 1)
                    Camera.main.orthographicSize -= 0.5F;
            }
        }

        void Rotate()
        {
            isRotating = true;
            if (isRotating)
            {
                //得到鼠标x方向移动距离
                mousex = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                //旋转轴的位置是目标物体处，方向是世界坐标系的y轴

                transform.RotateAround(GetHeadPos(targetTank.TankObject.transform.position), Vector3.up, mousex);
                //每次旋转后更新偏移量
                offset = GetHeadPos(targetTank.TankObject.transform.position) - transform.position;
            }
        }
        void Rollup()
        {
            lookup = true;
            if (lookup)
            {
                //得到鼠标y方向移动距离
                mousey = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
                //旋转轴的位置在目标物体处，方向是摄像机的x轴
                if (Mathf.Abs(transform.rotation.x + mousey - initialTransform.rotation.x) > 90) mousey = 0;
                transform.RotateAround(GetHeadPos(targetTank.TankObject.transform.position), transform.right, mousey);
                //每次旋转后更新偏移量
                offset = GetHeadPos(targetTank.TankObject.transform.position) - transform.position;
            }

        }
        void Move()
        {
            CameraRotate();
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            // Move when "w a s d" is pressed
            if (Mathf.Abs(vertical) > 0.01)
            {
                Vector3 fowardVector = transform.forward;
                fowardVector = new Vector3(fowardVector.x, 0, fowardVector.z).normalized;
                // move forward
                transform.Translate(FreeMoveSpeed * Time.deltaTime * vertical * fowardVector, Space.World);
            }
            if (Mathf.Abs(horizontal) > 0.01)
            {
                Vector3 rightVector = transform.right;
                rightVector = new Vector3(rightVector.x, 0, rightVector.z).normalized;
                // move aside 
                transform.Translate(FreeMoveSpeed * Time.deltaTime * horizontal * rightVector, Space.World);
            }

            // Fly up if space is clicked
            if (Input.GetKey(KeyCode.Space))
            {
                transform.Translate(FreeMoveSpeed * Time.deltaTime * Vector3.up, Space.World);
            }
            // Fly down if left shift is clicked
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(FreeMoveSpeed * Time.deltaTime * Vector3.down, Space.World);
            }

        }
        void CameraRotate()
        {
            if (Input.GetMouseButton(1))
            {
                float MouseX = Input.GetAxis("Mouse X");
                float MouseY = Input.GetAxis("Mouse Y");

                if ((Mathf.Abs(MouseX) > 0.01 || Mathf.Abs(MouseY) > 0.01))
                {
                    transform.Rotate(new Vector3(0, MouseX * RotateSpeed * Time.deltaTime, 0), Space.World);

                    float rotatedPitch = transform.eulerAngles.x - MouseY * RotateSpeed * Time.deltaTime * 1f;
                    if (Mathf.Abs(rotatedPitch > 180 ? 360 - rotatedPitch : rotatedPitch) < FreeMaxPitch)
                    {
                        transform.Rotate(new Vector3(-MouseY * RotateSpeed * Time.deltaTime * 1f, 0, 0));
                    }
                    else
                    {
                        if (transform.eulerAngles.x < 180)
                            transform.eulerAngles = new Vector3((FreeMaxPitch - 1e-6f), transform.eulerAngles.y, 0);
                        else
                            transform.eulerAngles = new Vector3(-(FreeMaxPitch - 1e-6f), transform.eulerAngles.y, 0);
                    }
                }
            }
        }
    }

}
