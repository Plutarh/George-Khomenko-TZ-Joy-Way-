using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickable
{
    public bool IsPicked => _isPicked;
    public bool IsIKRequire => _isIKRequire;

    [SerializeField] protected Transform _muzzle;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected float _damage;

    [SerializeField] protected GameObject _owner;
    [SerializeField] protected List<ScriptableEffect> _effectsOnHit = new List<ScriptableEffect>();

    [SerializeField] private string _weaponName;
    [SerializeField] private bool _isIKRequire;

    private bool _isPicked;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_weaponName == string.Empty)
        {
            _weaponName = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

#endif

    public virtual void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    public virtual void PickUp(GameObject owner)
    {
        _owner = owner;
        _isPicked = true;
    }

    public virtual void Drop()
    {
        _isPicked = false;
        _owner = null;

        gameObject.AddComponent<PickableObject>();
    }

    public virtual void StopShoot()
    {

    }

    public virtual void Shoot(Vector3 direction)
    {

    }

    private void OnDestroy()
    {
        GlobalEvents.OnWeaponDestroyed?.Invoke(_weaponName);
    }
}

