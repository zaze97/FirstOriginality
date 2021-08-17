using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AnimEventListener.AddEvent(this.gameObject,"idle",0.3f,TriggerEvent);
    }

    private  void TriggerEvent()
    {

        Debug.Log("第一个事件:");
    }
}
