using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float _floatingTime = 3;
    [SerializeField] private float _floatHeight = 0.5f;
    [SerializeField] private float _rotationSpeed = 15;

    private Tween floatingTween;

    private void Start()
    {
        Floating();
    }

    private void Update()
    {
        Rotating();
    }

    void Rotating()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * _rotationSpeed);
    }

    void Floating()
    {
        floatingTween = transform.DOMoveY(transform.position.y + _floatHeight, _floatingTime).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopFloating()
    {
        if (floatingTween == null) return;

        floatingTween.Kill();
        floatingTween = null;
    }

}
