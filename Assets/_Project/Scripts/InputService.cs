using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputService : MonoBehaviour
{
    public static InputService get;

    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    public static Action SpawnScarecrow;

    private Vector2 _oldMousePosition;

    private void Awake()
    {
        get = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SpawnScarecrow?.Invoke();

        Look();
        Move();
    }


    void Look()
    {
        _lookInput = (Vector2)Input.mousePosition - _oldMousePosition;
        _oldMousePosition = Input.mousePosition;
    }

    void Move()
    {
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");
    }

}
