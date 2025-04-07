using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

        public enum CameraStatus { freeCamera = 0, player, fixedCamera };
        public CameraStatus _cameraStatus;

        public UnityEngine.Transform initialTransform;
        private int _playerNumber = 0;
        private List<TankModel> _players;

        UnityEngine.Vector3 offset;//相机跟随的偏移量
        public float rotationSpeed;//摄像机旋转速度
        public float zoomSpeed;         // 缩放速度

        //左右旋转、上下旋转功能:
        public bool isRotating, lookup;
        float mousex, mousey;

        public Vector3 velocity = Vector3.zero;
        public Vector3 lookatPositionvelocity = Vector3.zero;
        public Vector3 lookatPosition;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Camera myCamera;

        public Transform parentTransform;
        public Button LockButton;

        // 固定位置和旋转的值
        private Vector3 fixedPosition = new Vector3(-5.2f, 69.5f, -5.2f);
        private Quaternion fixedRotation = Quaternion.Euler(91.14f, 0.0f, 0.0f);

        // Start is called before the first frame update
        void Start()
        {
            mTanks = this.GetModel<Tanks>();
            _players = new();
            offset = new Vector3(5, 5, 5);
            initialTransform = transform;
            RotateSpeed = 200f;
            rotationSpeed = 75f;
            MoveSpeed = 0.1f;
            FreeMoveSpeed = 10f;
            _cameraStatus = CameraStatus.freeCamera;
            targetTank = null;
            zoomSpeed = 30;

            initialPosition = transform.position;
            initialRotation = transform.rotation;
            myCamera = GetComponent<Camera>();
            TypeEventSystem.Global.Register<BattleStageEvent>(e =>
            {
                SetCameraStatus();
            });
            TypeEventSystem.Global.Register<BattleEndEvent>(e =>
            {
                ResetCamera();
            });
            LockButton.onClick.AddListener(LockStatus);
        }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }

        void Update()
        {
            if (SceneData.GameStage == "Battle")
            {
                CameraMove();
            }
        }
        void LockStatus()
        {
            if (_cameraStatus == CameraStatus.fixedCamera)
            {
                _cameraStatus = CameraStatus.freeCamera;
            }
            else
            {
                _cameraStatus = CameraStatus.fixedCamera;
            }
        }
        void FixedCamera()
        {
            if (parentTransform != null)
            {
                // 将相机的位置和旋转设置为相对于父物体的位置和旋转
                transform.SetParent(parentTransform);
                transform.localPosition = fixedPosition; // 设置相对父物体的位置
                transform.localRotation = fixedRotation; // 设置相对父物体的旋转
                myCamera.fieldOfView = 50f;
            }
            else
            {
                Debug.LogWarning("Parent Transform is not set!");
            }
        }
        void ResetCamera()
        {
            _cameraStatus = CameraStatus.freeCamera;
            transform.position = initialPosition;
            transform.rotation = initialRotation;

            // 平滑重置
            /*transform.position = Vector3.SmoothDamp(
                transform.position,
                initialPosition,
                ref velocity,
                0.2f
                );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                initialRotation,
                Time.deltaTime * 5
                );*/
            if (myCamera != null)
            {
                myCamera.fieldOfView = 10f;
            }
        }
        void CameraMove()
        {
            mTanks = this.GetModel<Tanks>();
            if (_cameraStatus == CameraStatus.player)
            {
                Rotate();
                Rollup();
                ExchangeStatus();
                Follow();
                Zoom();
            }
            else if (_cameraStatus == CameraStatus.freeCamera)
            {
                Move();
                ExchangeStatus();
                FreeZoom();
            }
            else if (_cameraStatus == CameraStatus.fixedCamera)
            {
                FixedCamera();
            }

        }

        void SetCameraStatus()
        {
            _cameraStatus = CameraStatus.freeCamera;
        }

        void ExchangeStatus()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Dictionary<int, TankModel> dict = mTanks.GetTankDictCopy();
                _players.Clear();
                foreach (KeyValuePair<int, TankModel> player in dict)
                {
                    _players.Add(player.Value);
                }
                if (_cameraStatus == CameraStatus.player)
                {
                    _cameraStatus = CameraStatus.freeCamera;
                    if (_players.Count - 1 > _playerNumber)
                    {
                        _playerNumber += 1;
                    }
                    else
                    {
                        _playerNumber = 0;
                    }
                }
                else if (_cameraStatus == CameraStatus.freeCamera && _players.Count != 0)
                {
                    _cameraStatus = CameraStatus.player;
                    
                    //visualAngleReset(transform.position, GetHeadPos(targetTank.TankObject.transform.position));
                    //_playerNumber += 1;
                    
                    targetTank = _players[_playerNumber];
                }
            }
        }

        Vector3 GetHeadPos(Vector3 playerPos)
        {
            return new Vector3(playerPos.x, playerPos.y + 1.5f, playerPos.z);
        }
        void Follow()
        {
            // Debug.Log($"player num is {mTanks.GetPlayerNum()}");
            // 视野缩放
            offset = new Vector3(5.7f, 14.6f, -5.2f);
            Vector3 targetPosition = targetTank.TankObject.transform.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
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


        void FreeZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll == 0) return;

            // 使用更精确的移动计算
            float moveAmount = scroll * zoomSpeed;

            //Debug.Log($"Scroll: {scroll} | Origin Move: {moveAmount} | zoom speed: {zoomSpeed}");

            // 直接限制移动量而非累计值
            moveAmount = Mathf.Clamp(moveAmount, -10f, 10f);

            // 使用世界坐标系移动（更稳定）
            transform.position += transform.forward * moveAmount;
            //Debug.Log($"Scroll: {scroll} | Move: {moveAmount}");
        }

        void Rotate()
        {
            if (Input.GetMouseButton(1))
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
        }

        void Rollup()
        {
            if (Input.GetMouseButton(1))
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
            if (Input.GetMouseButton(2)) // 2表示鼠标中键
            {
                float moveX = Input.GetAxis("Mouse X") * FreeMoveSpeed * Time.deltaTime * 5f;
                float moveY = Input.GetAxis("Mouse Y") * FreeMoveSpeed * Time.deltaTime * 5f;

                // 更新相机位置
                transform.position -= transform.right * moveX;
                transform.position -= transform.up * moveY;
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
