using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJump : MonoBehaviour
{
    public Animator animator;                         //自身动画组件
    public Rigidbody selfRigidbody;                //自身刚体组件
    public TestMove testMove;                          //之前的移动脚本
    public Transform[] groundPoints;                //地面检测点
    public LayerMask groundLayerMask = ~0;          //地面LayerMask
    public Vector4 arg;                    //x时间，y重力系数，z方向力系数，w偏移
    public AnimationCurve riseCurve = new AnimationCurve(new Keyframe[] 
        { new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f) });
    public AnimationCurve directionJumpCurve = new AnimationCurve(new 
        Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 
            0f) });
    int mIsJumpAnimatorHash;                         //移动Animator变量哈希
    float mGroundedDelay;                               //延迟检测变量
    bool mIsGrounded;                                    //是否正在地面上
    Coroutine mJumpCoroutine;                          //跳跃协程序
    void Start()
    {
        mIsJumpAnimatorHash = Animator.StringToHash("IsJump");
    }
    IEnumerator JumpCoroutine(Vector3 moveDirection, Vector3 upAxis)
    {

        mGroundedDelay = Time.maximumDeltaTime * 2f;          //两帧延迟
        selfRigidbody.useGravity = false;                         //暂时关闭重力
        var t = arg.w;                                                  //时间插值
        do
        {
            Debug.Log("执行跳跃");
            var t_riseCurve = riseCurve.Evaluate(t);          //上升力曲线采样
            //方向力曲线采样
            var t_directionJump = directionJumpCurve.Evaluate(t);
            var gravity = Vector3.Lerp(-upAxis, upAxis, t_riseCurve) * arg.y;
            var forward = Vector3.Lerp(moveDirection * arg.z * Time.fixedDeltaTime,
                Vector3.zero, t_directionJump);
            //获得方向并乘以系数
            selfRigidbody.velocity = gravity + forward;          //更新速率
            t = Mathf.Clamp01(t + Time.deltaTime * arg.x);     //更新插值
            yield return null;
        } while (!mIsGrounded);
        selfRigidbody.useGravity = true;                         //恢复重力
    }
    void Update()
    {
        var upAxis = -Physics.gravity.normalized;               //up轴向
        var raycastHit = default(RaycastHit);
        //其实现与MoveTest一样，但去掉了法线返回
        mIsGrounded = GroundProcess(ref raycastHit, upAxis);
        if (mGroundedDelay > 0f) mIsGrounded = false;          //延迟处理修正
        if (mIsGrounded && mJumpCoroutine != null)               //跳跃打断处理
        {
            StopCoroutine(mJumpCoroutine);
            selfRigidbody.useGravity = true;
            mJumpCoroutine = null;
        }

        //移到inputsystem接口里
        //
        if (Input.GetKeyDown(KeyCode.Space)&&mJumpCoroutine == null && mIsGrounded)
        {//执行跳跃
            mJumpCoroutine = StartCoroutine(JumpCoroutine(testMove.MoveDirection,
                upAxis));
            animator.SetBool(mIsJumpAnimatorHash, true);
        }
        else
        {//落地状态逻辑
            if (mIsGrounded)
                animator.SetBool(mIsJumpAnimatorHash, false);
        }
        
        mGroundedDelay -= Time.deltaTime;          //延迟变量更新
        UpdateGroundDetectPoints();                    //更新地面点
    }
    bool GroundProcess(ref RaycastHit raycastHit, Vector3 upAxis)
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
        //Vector3 groundNormal = raycastHit.normal;//返回地面法线
        //var upQuat = Quaternion.FromToRotation(upAxis, groundNormal);
        //moveDirection = upQuat * moveDirection;//根据地面法线修正移动位置
        return result;
    }
    void UpdateGroundDetectPoints()
    {
        var groupPointsIndex_n = Random.Range(0, groundPoints.Length);//随机一个索引
        var temp = groundPoints[groupPointsIndex_n];
        groundPoints[groupPointsIndex_n] = groundPoints[groundPoints.Length - 1];
        groundPoints[groundPoints.Length - 1] = temp;//交换地面检测点，防止每次顺序都一样。
    }
}
