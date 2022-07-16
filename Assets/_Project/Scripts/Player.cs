using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Pawn
{
    public Transform CameraRoot => _cameraRoot;

    [SerializeField] private Transform _cameraRoot;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
