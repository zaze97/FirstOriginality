using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Matrix4x4 gopos;

    void Start()
    {
        gopos = this.transform.localToWorldMatrix;

        Matrix4x4 test1 = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0),
            new Vector4(0, -10, 10, 1));
        Debug.Log(test1);
        Matrix4x4 test2 = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 0.866f, -0.5f, 0),
            new Vector4(0, 0.5f, 0.866f, 0), new Vector4(0, 0, 0, 1));
        Debug.Log(test2);
        Matrix4x4 test3 = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, -1, 0),
            new Vector4(0, 0, 0, 1));
        Debug.Log(test3);
        Matrix4x4 testmax = test1 * test2 * test3 ;
        Debug.Log(testmax);
    }

    private void Update()
    {
        gopos = this.transform.localToWorldMatrix;
    }
}
