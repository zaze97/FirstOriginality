using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGround : MonoBehaviour
{
    public Transform[] _transforms;
    public LayerMask _layerMask;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool isGround;
        RaycastHit raycastHit;
        IsOnGroundUpdate(_transforms, _layerMask, 100, out isGround, out raycastHit);
        if (isGround)
        {
            Debug.Log("碰到地面了");
            Debug.Log(raycastHit.collider.name);
        }
    }

    static void IsOnGroundUpdate(Transform[] groundPoints, LayerMask layerMask, float length, out bool isOnGround,
        out RaycastHit cacheRaycastHit)
    {
        isOnGround = false;
        cacheRaycastHit = default(RaycastHit); //初始化数值
        //遍历所有地面检测点
        for (int i = 0, iMax = groundPoints.Length; i < iMax; i++)
        {
            var groundPoint = groundPoints[i];
            var hit = default(RaycastHit);
            var isHit = Physics.Raycast(new Ray(groundPoint.position,
                -groundPoint.up), out hit, length, layerMask); //射线检测
            if (isHit)
            {
                isOnGround = isHit;
                cacheRaycastHit = hit; //缓存结果多次使用
                break; //检测到即刻跳出
            }
        }
    }
}