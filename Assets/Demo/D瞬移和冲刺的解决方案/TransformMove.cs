using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMove : MonoBehaviour
{
    private Rigidbody selfRigidbody;

    private void Start()
    {
        selfRigidbody = GetComponent<Rigidbody>();
    }


    private void OnGUI()
    {
        if (GUILayout.Button("坐标重置", GUILayout.Width(100), GUILayout.Height(30)))
        {
            this.transform.position = new Vector3(0, 3.34f, -3.57f);
        }

        if (GUILayout.Button("瞬移", GUILayout.Width(100), GUILayout.Height(30)))
        {
            BlinkTo(this.transform, this.transform.position + this.transform.forward * 5);
        }

        if (GUILayout.Button("冲刺", GUILayout.Width(100), GUILayout.Height(30)))
        {
            StartCoroutine(DashTo(this.transform, this.transform.position + this.transform.forward * 5));
        }
    }

    /// <summary>
    /// 瞬移
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="dstPoint"></param>
    void BlinkTo(Transform trans, Vector3 dstPoint)
    {
        trans.position = SweepTest(trans, dstPoint);
    }

    /// <summary>
    /// 冲刺 冲刺受碰撞影响
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="dstPoint"></pafram>
    /// <returns></returns>
    IEnumerator DashTo(Transform trans, Vector3 dstPoint)
    {
        bool iscol = IsColliderUpdate(trans.position, trans.forward);
        if (iscol) yield break;
        //trans是正在瞬移的对象，dstPoint是目标点
        var selfPoisition = trans.position;
        //此处为自定义函数，修正到准确的地面位置
        dstPoint = GetGroundPoint(trans, dstPoint) + Vector3.up;
        Debug.Log("dstPoint" + dstPoint);
        var waitForFixedUpdate = new WaitForFixedUpdate();
        var beginTime = Time.fixedTime; //此处是物理更新时序
        for (var duration = 0.15f; Time.fixedTime - beginTime <= duration;)
        {
            var t = (Time.fixedTime - beginTime) / duration;
            t = t * t; //逐渐加速插值
            trans.position = Vector3.Lerp(selfPoisition, dstPoint, t);
            // selfRigidbody.position = transform.position;
            //Debug.Log("刚体位置已修正"+selfRigidbody.position);
            yield return waitForFixedUpdate;
            Debug.Log("协程执行完成");
        }
    }

    /// <summary>
    /// 目的地进行SweepTest测试,修正碰撞后的位置
    /// </summary>
    private Vector3 SweepTest(Transform trans, Vector3 dstPoint)
    {
        //trans为正在瞬移的对象，dstPoint为目标点
        var diff = (dstPoint - trans.position); //距离差向量
        var length = diff.magnitude; //目标长度
        var dir = diff.normalized; //目标方向
        var hit = default(RaycastHit);
        if (selfRigidbody.SweepTest(dir, out hit, length) && hit.collider.tag == _Const.TAG_WALL)
        {
            Debug.DrawRay(trans.position, diff, Color.red, 100);
            Debug.Log("碰到障碍物，开始计算位移" + hit.collider.name);
            //返回true说明碰到了障碍物
            var dstClosestPoint =
                hit.collider.ClosestPointOnBounds(hit.collider.transform.position); //目标Bounds的边界点 

            var selfClosestPoint =
                selfRigidbody.ClosestPointOnBounds(dstClosestPoint); //自己相对于目标的边界点                            
            //边界点距离差
            var closestPointDiff = selfClosestPoint - transform.position;
            //返回障碍物边界的位置信息
            //transform.position = dstClosestPoint - closestPointDiff;
            return dstClosestPoint - closestPointDiff;
        }
        else
        {
            Debug.Log("没有碰到障碍物，直接位移" + hit.point);
            //返回目标信息位置信息
            return dstPoint;
            //trans.position = dstPoint;
        }
    }


    /// <summary>
    /// 修正到准确的地面位置
    /// </summary>
    /// <param name="dstPoint"></param>
    /// <returns></returns>
    public Vector3 GetGroundPoint(Transform trans, Vector3 dstPoint)
    {
        RaycastHit cacheRaycastHit = default(RaycastHit);
        IsOnGroundUpdate(dstPoint, 100, ref cacheRaycastHit);
        Debug.Log("执行检测" + cacheRaycastHit.collider.name + "执行检测物体的位置" + cacheRaycastHit.point);

        var afterPos = SweepTest(trans, cacheRaycastHit.point);
        return afterPos;
    }

    /// <summary>
    /// 检测四周是否有除了地面别的碰撞体
    /// </summary>
    /// <param name="groundPoint"></param>
    /// <param name="length"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool IsColliderUpdate(Vector3 groundPoint, Vector3 direction)
    {
        var hit = default(RaycastHit);
        return (Physics.Raycast(new Ray(groundPoint, direction), out hit, 1, ~_Const.LAYER_GROUND)); //射线检测
    }

    /// <summary>
    /// 检测目标点上地面的位置
    /// </summary>
    /// <param name="groundPoint"></param>
    /// <param name="length"></param>
    /// <param name="cacheRaycastHit"></param>
    private void IsOnGroundUpdate(Vector3 groundPoint, float length, ref RaycastHit cacheRaycastHit)
    {
        //遍历所有地面检测点 
        var hit = default(RaycastHit);
        if (Physics.Raycast(new Ray(groundPoint, Vector3.down), out hit, length, _Const.LAYER_GROUND)) //射线检测
        {
            cacheRaycastHit = hit;
        }
    }
}