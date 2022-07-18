using System;
using UnityEngine;

public class InputService : MonoBehaviour
{
    public static InputService get;

    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    public static Action SpawnScarecrow;

    private void Awake()
    {
        get = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SpawnScarecrow?.Invoke();

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.Confined;



        ReadLookInput();
        ReadMoveInput();
    }


    void ReadLookInput()
    {
        _lookInput.x = Input.GetAxis("Mouse X");
        _lookInput.y = Input.GetAxis("Mouse Y");
    }

    void ReadMoveInput()
    {
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");
    }

}
