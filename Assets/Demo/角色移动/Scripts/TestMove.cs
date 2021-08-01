using System;
using UnityEngine;
using Random = UnityEngine.Random;

    public class TestMove : MonoBehaviour
    {
        public Animator animator;
        public Rigidbody selfRigidbody;//自身刚体组件
        public float speed = 17f;//移动速度
        public float rotSpeed = 17f;//旋转速度
        public Transform[] groundPoints;//地面检测点
        public LayerMask groundLayerMask = ~0;//地面LayerMask
        public LayerMask wallLayerMask = ~0;//墙壁LayerMask
        int mIsMoveAnimatorHash;//移动Animator变量哈希
        [SerializeField] private InputReader _inputReader;
        private Vector2 movement;
        public bool IsMoving { get; private set; }
        public Vector3 MoveDirection { get; private set; }

        private void OnEnable()
        {
            _inputReader.MoveSelectionEvent += OnMove;
        }
        private void OnDisable()
        {
            _inputReader.MoveSelectionEvent -= OnMove;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {

            const float INPUT_EPS = 0.2f;
            var horizontal = movement.x;//横向轴的值
            var vertical = movement.y;//纵向轴的值
            var inputDirection = new Vector3(horizontal, 0f, vertical);//输入向量
            var upAxis = -Physics.gravity.normalized;//up轴向
            var moveDirection = CameraDirectionProcess(inputDirection, upAxis);//相机输入方向修正
            MoveDirection = moveDirection;
            Debug.Log(inputDirection.magnitude);
            if (inputDirection.magnitude > INPUT_EPS)//是否有输入方向
            {
                Debug.Log("开始移动");
                var raycastHit = default(RaycastHit);
                var groundNormal = Vector3.zero;
                var groundedFlag = GroundProcess(ref raycastHit, ref moveDirection, out groundNormal, upAxis);//地面检测

                if (groundedFlag)
                {
                    var cacheMoveDirection = moveDirection;
                    var wallFlag = WallProcess(ref raycastHit, ref moveDirection, groundNormal, upAxis);//墙壁检测
                    var cliffFlag = false;
                    if (!wallFlag)
                        cliffFlag = CliffProcess(ref raycastHit, ref moveDirection, groundNormal, upAxis);//悬崖检测
                    if (!cliffFlag)
                    {
                        Debug.Log("执行移动");
                        selfRigidbody.velocity = moveDirection * speed * Time.fixedDeltaTime*20; //更新位置
                    }

                    UpdateGroundDetectPoints();//打乱地面检测点顺序
                    RotateProcess(cacheMoveDirection, upAxis);//更新旋转
                }
                animator.SetBool(mIsMoveAnimatorHash, true);//更新Animator变量
                IsMoving = true;
            }
            else//没有移动
            {
                Debug.Log("不移动");
                MoveDirection = Vector3.zero;
                animator.SetBool(mIsMoveAnimatorHash, false);//更新Animator变量
                IsMoving = false;
            }
        }

        private void OnMove(Vector2 movement)
        {
            Debug.Log("移动触发了:"+movement);
            this.movement = movement;
        }
        
        
        void Start()
        {
            mIsMoveAnimatorHash = Animator.StringToHash("IsMove");
        }
        
        Vector3 CameraDirectionProcess(Vector3 inputDirection, Vector3 upAxis)
        {
            var mainCamera = Camera.main;//获取主相机，具体使用请缓存该值
            var quat = Quaternion.FromToRotation(mainCamera.transform.up, upAxis);//不同重力的up轴修正
            var cameraForwardDirection = quat * mainCamera.transform.forward;//转换forward方向
            var moveDirection = Quaternion.LookRotation(cameraForwardDirection, upAxis) * inputDirection.normalized;//转换输入向量方向
            return moveDirection;
        }

        bool GroundProcess(ref RaycastHit raycastHit, ref Vector3 moveDirection, out Vector3 groundNormal, Vector3 upAxis)
        {
            const float GROUND_RAYCAST_LENGTH = 0.2f;//地面检测射线长度
            var result = false;
            for (int i = 0; i < groundPoints.Length; i++)
            {
                var tempRaycastHit = default(RaycastHit);
                if (Physics.Raycast(new Ray(groundPoints[i].position, -upAxis), out tempRaycastHit, GROUND_RAYCAST_LENGTH, groundLayerMask))//投射地面射线
                {
                    if (raycastHit.transform == null || Vector3.Distance(transform.position, tempRaycastHit.point) < Vector3.Distance(transform.position, raycastHit.point))
                        raycastHit = tempRaycastHit;//选取最近的地面点
                    result = true;
                    break;
                }
            }
            groundNormal = raycastHit.normal;//返回地面法线
            var upQuat = Quaternion.FromToRotation(upAxis, groundNormal);
            moveDirection = upQuat * moveDirection;//根据地面法线修正移动位置
            return result;
        }

        bool WallProcess(ref RaycastHit raycastHit, ref Vector3 moveDirection, Vector3 groundNormal, Vector3 upAxis)
        {
            const float HEIGHT = 1.2f;//玩家高度估算值
            const float OBLIQUE_P0 = 0.3f, OBLIQUE_P1 = 0.6f;//斜方向射线偏移
            const float RAYCAST_LEN = 0.37f;//射线长度
            const float DOT_RANGE = 0.86f;//点乘范围约束
            const float ANGLE_STEP = 30f;//检测角度间距
            var result = false;
            var ray = new Ray(transform.position + upAxis * HEIGHT, moveDirection);
            for (float angle = -90f; angle <= 90f; angle += ANGLE_STEP)//180度内每隔一定角度进行射线检测
            {
                var quat = Quaternion.AngleAxis(angle, upAxis);//得到当前角度
                ray = new Ray(transform.position, quat * moveDirection);
                var p0 = ray.origin + ray.direction * OBLIQUE_P0;
                var p1 = ray.origin + upAxis * HEIGHT + ray.direction * OBLIQUE_P1;
                if (Physics.Linecast(p0, p1, out raycastHit, wallLayerMask))//是否碰到墙壁
                {
                    var newRay = new Ray(Vector3.Project(raycastHit.point, upAxis) + Vector3.ProjectOnPlane(ray.origin, upAxis), ray.direction);
                    if (Physics.Raycast(newRay, out raycastHit, RAYCAST_LEN, wallLayerMask))//重新得到射线位置并投射
                    {
                        if (Vector3.Dot(moveDirection, -raycastHit.normal) < DOT_RANGE)//点乘约束
                        {
                            var cross = Vector3.Cross(raycastHit.normal, upAxis).normalized;
                            var cross2 = -cross;
                            if (Vector3.Dot(cross, moveDirection) > Vector3.Dot(cross2, moveDirection))//获得最接近方向
                                moveDirection = cross;
                            else
                                moveDirection = cross2;
                            break;//若已确定修正方向则跳出循环
                        }
                    }
                    result = true;//确定碰到了墙壁
                }
            }
            return result;
        }

        bool CliffProcess(ref RaycastHit raycastHit, ref Vector3 moveDirection, Vector3 groundNormal, Vector3 upAxis)
        {
            const float GROUND_RAYCAST_LENGTH = 0.4f;//地面检测射线长度
            var result = false;
            for (int i = 0; i < groundPoints.Length; i++)//遍历地面检测点
            {
                var relative = groundPoints[i].position - transform.position;//取相对位置
                var quat = Quaternion.FromToRotation(upAxis, groundNormal);//映射到地面法线方向四元数
                var newPoint = transform.position + moveDirection + quat * relative;
                var ray = new Ray(newPoint, -upAxis);//取移动后的位置投射射线
                Debug.DrawRay(newPoint, -upAxis*GROUND_RAYCAST_LENGTH, Color.yellow, 100);
                if (!Physics.Raycast(ray, out raycastHit, GROUND_RAYCAST_LENGTH, groundLayerMask))//只要有一个未检测到地面则为悬崖
                {
                    result = true;//返回true
                    break;
                }
            }
            return result;
        }

        void RotateProcess(Vector3 moveDirection, Vector3 upAxis)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, upAxis);//投影到up平面上
            var playerLookAtQuat = Quaternion.LookRotation(moveDirection, upAxis);//得到移动方向代表的旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, playerLookAtQuat, rotSpeed * Time.deltaTime);//更新插值
        }

        void UpdateGroundDetectPoints()
        {
            var groupPointsIndex_n = Random.Range(0, groundPoints.Length);//随机一个索引
            var temp = groundPoints[groupPointsIndex_n];
            groundPoints[groupPointsIndex_n] = groundPoints[groundPoints.Length - 1];
            groundPoints[groundPoints.Length - 1] = temp;//交换地面检测点，防止每次顺序都一样。
        }
    }

