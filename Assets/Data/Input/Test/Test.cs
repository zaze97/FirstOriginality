using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject go;
    private Vector2 movement;
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
        Vector2 playerVelocity=new Vector2(movement.x,movement.y)*Time.deltaTime;
        Debug.Log(playerVelocity);
        go.GetComponent<Rigidbody>().velocity += (Vector3)playerVelocity;
        // go.transform.position += (Vector3)playerVelocity ;
    }

    private void OnMove(Vector2 movement)
    {
        Debug.Log("移动触发了:"+movement);
        this.movement = movement;
    }


}
