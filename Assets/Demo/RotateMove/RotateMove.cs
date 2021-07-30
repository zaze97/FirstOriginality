using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMove : MonoBehaviour
{
   public Quaternion mFromTo;
    void OnEnable()
    {
        mFromTo = Quaternion.FromToRotation(transform.forward, Vector3.forward);

    }
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, mFromTo, 17 * 
                                                                          Time.deltaTime);
        
        var dot = Quaternion.Dot(transform.rotation, mFromTo);
        Debug.Log(dot);
        if (Mathf.Abs(dot) > 0.8f)
        {
            //省略具体执行代码
            Debug.Log("执行");
        }
    }
}
