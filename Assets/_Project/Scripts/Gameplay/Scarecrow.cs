using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Scarecrow : Pawn
{
    [SerializeField] private Transform _rotationPivotPoint;

    Rigidbody _rigidbody;

    public ScriptableEffect burn;

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
    }

    public override void TakeDamage(DamageData damageData)
    {
        base.TakeDamage(damageData);

        RotationImpact(damageData.velocity);
    }

    void RotationImpact(Vector3 velocity)
    {
        if (IsDead) return;
        if (_rotationPivotPoint == null)
        {
            Debug.LogError($"Rotation pivot point nulled {transform.name}", this);
            return;
        }

        velocity = Vector3.ClampMagnitude(velocity, Random.Range(1, 5));
        _rotationPivotPoint.transform.DOPunchRotation(velocity, 0.2f, 3, 0.5f).OnComplete(() => _rotationPivotPoint.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.1f));
    }

    void DeathImpact(Vector3 velocity)
    {
        _rotationPivotPoint.transform.localRotation = Quaternion.identity;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        velocity = Vector3.ClampMagnitude(velocity, Random.Range(1, 5));
        _rigidbody.AddForce(velocity, ForceMode.Impulse);
    }

    public override void Death(DamageData damageData)
    {
        base.Death(damageData);

        DeathImpact(damageData.velocity);
    }
}
