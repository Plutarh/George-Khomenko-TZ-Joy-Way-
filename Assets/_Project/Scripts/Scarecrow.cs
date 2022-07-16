using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Scarecrow : Pawn
{
    [SerializeField] private Transform _rotationPivotPoint;

    Rigidbody _rigidbody;

    public override void Awake()
    {
        base.Awake();
        Initialize();

    }

    public override void Start()
    {
        base.Start();
    }

    void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.T))
        {
            DamageData testData = new DamageData();
            testData.damage = 50;
            testData.velocity = Random.onUnitSphere;
            TakeDamage(testData);
        }
    }

    public override void TakeDamage(DamageData damageData)
    {
        base.TakeDamage(damageData);

        if (IsDead)
            DeathImpact(damageData.velocity);
        else
            RotationImpact(damageData.velocity);
    }

    void RotationImpact(Vector3 velocity)
    {
        if (_rotationPivotPoint == null)
        {
            Debug.LogError($"Rotation pivot point nulled {transform.name}", this);
            return;
        }

        _rotationPivotPoint.transform.DOPunchRotation(velocity * Random.Range(1, 10), 0.2f, 10, 1).OnComplete(() => _rotationPivotPoint.transform.DORotate(Vector3.zero, 0.1f));
    }

    void DeathImpact(Vector3 velocity)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        _rigidbody.AddForce(velocity * Random.Range(2, 5), ForceMode.Impulse);
    }


    public override void Death()
    {
        base.Death();


    }
}
