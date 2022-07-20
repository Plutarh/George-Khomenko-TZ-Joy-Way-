using UnityEngine;

public class WetnessTimedEffect : TimedEffect
{
    IDamageable _target;
    GameObject _whoUse;
    WetnessScriptableTimedEffect _wetnessScriptableEffect;
    ParticleSystem _wetnessFx;

    float _timeToHit;

    public WetnessTimedEffect(ScriptableTimedEffect buff, GameObject targetObj, DamageData damageData) : base(buff, targetObj, damageData)
    {
        _target = targetObj.GetComponent<IDamageable>();
        _whoUse = damageData.owner;
    }

    public WetnessTimedEffect(ScriptableTimedEffect buff, GameObject targetObj) : base(buff, targetObj)
    {
        _target = targetObj.GetComponent<IDamageable>();
        _whoUse = damageData.owner;
    }

    protected override void ApplyEffect()
    {
        if (_target == null) return;
        _wetnessScriptableEffect = (WetnessScriptableTimedEffect)Effect;

        CreateFX();
        ApplyMaterialColor();
        currentDuration = totalDuration;

    }

    public override void End()
    {
        RemoveMaterialColor();

        if (_wetnessFx != null)
        {
            GameObject.Destroy(_wetnessFx.gameObject);
        }

    }

    public override void Tick(float delta)
    {
        base.Tick(delta);

    }

    void ApplyMaterialColor()
    {
        var targetSkin = _target.GetGameObject().GetComponent<Pawn>().Skin;
        if (targetSkin == null) return;

        targetSkin.material.SetFloat("_FresnelPower", 1f);
        targetSkin.material.SetColor("_FresnelColor", Color.blue * 3f);
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
        if (_wetnessScriptableEffect.wetnessFX == null) return;
        if (_wetnessFx != null) GameObject.Destroy(_wetnessFx.gameObject);

        _wetnessFx = GameObject.Instantiate(_wetnessScriptableEffect.wetnessFX, _target.GetGameObject().transform.position, Quaternion.AngleAxis(90, Vector3.right));
        _wetnessFx.transform.SetParent(_target.GetGameObject().transform);

        var targetSkin = _target.GetGameObject().GetComponent<Pawn>().Skin;

        if (targetSkin != null)
        {
            var wetnessShape = _wetnessFx.shape;

            if (targetSkin is MeshRenderer)
            {
                wetnessShape.shapeType = ParticleSystemShapeType.MeshRenderer;
                wetnessShape.meshRenderer = targetSkin as MeshRenderer;
            }
            else
            {
                wetnessShape.shapeType = ParticleSystemShapeType.Box;
            }
        }
        else
        {
            var burnShape = _wetnessFx.shape;
            burnShape.shapeType = ParticleSystemShapeType.Box;
        }

        _wetnessFx.transform.localPosition = Vector3.zero;
    }


}
