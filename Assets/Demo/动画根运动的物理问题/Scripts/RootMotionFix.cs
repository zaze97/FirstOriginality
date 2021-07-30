using System.Collections;
using UnityEngine;

namespace ACTBook
{
    public class RootMotionFix : MonoBehaviour
    {
        public Rigidbody myRigidbody;
        public Animator myAnimator;

        Coroutine mRootMotionFixCoroutine;


        //按键盘'D键测试根运动处理效果
        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SetVelocity(myAnimator, myRigidbody, new Vector3(5f, 0f, 0f));
            }
        }

        void OnGUI()
        {
            GUILayout.Box("---  请按下'D'键;以进行测试  ---");
        }

        //设置速率,error为是否进入根运动修复的误差值,velocityRecoverError为恢复根运动误差值。
        void SetVelocity(Animator animator, Rigidbody rigidbody, Vector3 velocity, float error = 0.01f, float velocityRecoverError = 0.1f)
        {
            myRigidbody.velocity = velocity;//覆盖新速率值
            if (myRigidbody.velocity.sqrMagnitude > error)//如果大于误差进行特殊处理
                mRootMotionFixCoroutine = StartCoroutine(RootMotionFixCoroutine(animator, rigidbody, velocityRecoverError));
        }
        //根运动特殊处理协程函数
        IEnumerator RootMotionFixCoroutine(Animator animator, Rigidbody rigidbody, float velocityRecoveryError = 0.1f)
        {
            myAnimator.applyRootMotion = false;//关闭根运动
            if (mRootMotionFixCoroutine != null)//检测是否有上一次循环
            {
                StopCoroutine(mRootMotionFixCoroutine);
                mRootMotionFixCoroutine = null;
            }
            while (true)//速率恢复检测的循环
            {
                if (rigidbody.velocity.sqrMagnitude < velocityRecoveryError)
                    break;
                yield return null;
            }
            animator.applyRootMotion = true;//恢复根运动
        }
        
    }
}
