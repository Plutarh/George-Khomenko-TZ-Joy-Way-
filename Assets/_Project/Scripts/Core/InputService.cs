using System;
using UnityEngine;

public class InputService : MonoBehaviour
{
    public static InputService get;

    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;

    [SerializeField] private KeyCode _leftHandPickDropKey = KeyCode.Q;
    [SerializeField] private KeyCode _rightHandPickDropKey = KeyCode.E;
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _cursorConfineKey;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    public static Action<EHandType> OnHandPickDropButtonDown;
    public static Action<EHandType> OnAttackButtonDown;

    private void Awake()
    {
        get = this;
    }

    void Update()
    {
        ReadButtonsInput();
        ReadLookInput();
        ReadMoveInput();
    }

    void ReadButtonsInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
            GlobalEvents.SpawnScarecrow?.Invoke();

        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButton(0))
            OnAttackButtonDown?.Invoke(EHandType.Right);

        if (Input.GetMouseButton(1))
            OnAttackButtonDown?.Invoke(EHandType.Right);

        if (Input.GetKeyDown(_cursorConfineKey))
            Cursor.lockState = CursorLockMode.Confined;

        if (Input.GetKeyDown(_leftHandPickDropKey))
            OnHandPickDropButtonDown?.Invoke(EHandType.Left);
        if (Input.GetKeyDown(_rightHandPickDropKey))
            OnHandPickDropButtonDown?.Invoke(EHandType.Right);
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

public enum EHandType
{
    Left,
    Right
}