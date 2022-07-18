using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTimedEffect : TimedEffect
{
    IDamageable _target;
    GameObject _whoUse;
    BurnScriptableEffect _burnScriptableEffect;
    ParticleSystem _burningFX;

    float _timeToHit;

    public BurnTimedEffect(ScriptableEffect buff, GameObject targetObj, DamageData damageData) : base(buff, targetObj, damageData)
    {
        _target = targetObj.GetComponent<IDamageable>();
        _whoUse = damageData.owner;
    }

    public BurnTimedEffect(ScriptableEffect buff, GameObject targetObj) : base(buff, targetObj)
    {
        _target = targetObj.GetComponent<IDamageable>();
        _whoUse = damageData.owner;
    }

    protected override void ApplyEffect()
    {
        if (_target == null) return;
        _burnScriptableEffect = (BurnScriptableEffect)Effect;

        CreateFX();
        ApplyMaterialColor();
        currentDuration = totalDuration;
        _timeToHit = _burnScriptableEffect.timerToHit;
    }

    public override void End()
    {
        RemoveMaterialColor();

        if (_burningFX != null)
        {
            GameObject.Destroy(_burningFX.gameObject);
        }

    }

    public override void Tick(float delta)
    {
        base.Tick(delta);

        if (IsFinished) return;
        if (IsVisual) return;

        _timeToHit -= delta;
        if (_timeToHit <= 0)
        {
            DamageData damageData = new DamageData();

            damageData.damage = _burnScriptableEffect.damage;
            damageData.owner = base.damageData.owner;

            _target.TakeDamage(damageData);

            _timeToHit = _burnScriptableEffect.timerToHit;
        }
    }

    void ApplyMaterialColor()
    {
        var targetSkin = _target.GetGameObject().GetComponent<Pawn>().Skin;
        if (targetSkin == null) return;

        targetSkin.material.SetFloat("_FresnelPower", 1f);
        targetSkin.material.SetColor("_FresnelColor", Color.red * 3f);
    }

    void RemoveMaterialColor()
    {
        var targetSkin = _target.GetGameObject().GetComponent<Pawn>().Skin;

        if (targetSkin == null) return;

        targetSkin.material.SetFloat("_FresnelPower", 10f);
        targetSkin.material.SetColor("_FresnelColor", Color.black);
    }

    void CreateFX()
    {
        if (_burnScriptableEffect.burnFX == null) return;

        _burningFX = GameObject.Instantiate(_burnScriptableEffect.burnFX, _target.GetGameObject().transform.position, Quaternion.AngleAxis(90, Vector3.right));
        _burningFX.transform.SetParent(_target.GetGameObject().transform);

        var targetSkin = _target.GetGameObject().GetComponent<Pawn>().Skin;

        if (targetSkin != null)
        {
            var burnShape = _burningFX.shape;

            if (targetSkin is MeshRenderer)
            {
                burnShape.shapeType = ParticleSystemShapeType.MeshRenderer;
                burnShape.meshRenderer = targetSkin as MeshRenderer;
            }
            else
            {
                burnShape.shapeType = ParticleSystemShapeType.Box;
            }
        }
        else
        {
            var burnShape = _burningFX.shape;
            burnShape.shapeType = ParticleSystemShapeType.Box;
        }

        _burningFX.transform.localPosition = Vector3.zero;
    }


}
