using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickable
{
    public bool IsPicked => _isPicked;
    public bool IsIKRequire => _isIKRequire;
    public bool IsAnimationRequire => _isAnimationRequire;
    public string AnimationName => _animationName;

    [SerializeField] protected Transform _muzzle;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected float _damage;

    protected GameObject _owner;

    [SerializeField] private string _weaponName;
    [SerializeField] private bool _isIKRequire;
    [SerializeField] private bool _isAnimationRequire;
    [SerializeField] private string _animationName;
    [SerializeField] protected List<EffectsInteractions> _effectsInteractions = new List<EffectsInteractions>();
    [SerializeField] protected List<ScriptableEffect> _effects = new List<ScriptableEffect>();

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
