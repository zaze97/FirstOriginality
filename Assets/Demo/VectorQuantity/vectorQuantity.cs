using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vectorQuantity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.forward,Color.red);
        //用来获取从raycast函数中得到的信息
        RaycastHit hit;
        var hasWall = Physics.Raycast(transform.position, transform.forward,out hit, 1f);
        if (hasWall)                                        //如果有一堵墙我们就绕开它
        {
            Debug.Log("碰到墙了"+hit.collider.name);
            var bypassDirection = Vector3.Cross(transform.forward, Physics.gravity.
                normalized);
            Move(bypassDirection);
        }
        else
        {
            Move(transform.forward);
        }
    }

    private void Move(Vector3 bypassDirection)
    {
        transform.position =  transform.position+bypassDirection*0.01f;
    }
}
