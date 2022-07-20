using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : UIHealthBar
{
    public bool IsDeactivated => _deactivated;

    public RectTransform rectTransform;

    [SerializeField] private UICombatText _combatTextPrefab;

    private bool _deactivated;

    [SerializeField] private Image _wetnessImage;

    [SerializeField] ScriptableEffect _wetnessEffect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void SetPawn(Pawn pawn)
    {
        base.SetPawn(pawn);
        pawn.OnEffectValueChanged += UpdateEffect;
    }

    public override void UpdateBar(DamageData damageData)
    {
        base.UpdateBar(damageData);
        ShowDamageText(damageData);
    }

    public override void ResetBar()
    {
        base.ResetBar();
        _wetnessImage.fillAmount = 0;
    }

    void UpdateEffect(Effect effect)
    {
        if (effect.effect != _wetnessEffect) return;

        if (effect != null)
            _wetnessImage.DOFillAmount(effect.GetValue01(), 0.1f);
        else
            _wetnessImage.fillAmount = 0;
    }

    public void Deactivate()
    {
        _deactivated = true;
    }

    void ShowDamageText(DamageData damageData)
    {
        var createdCombatText = Instantiate(_combatTextPrefab, _combatTextParent.transform);

        createdCombatText.ShowText(damageData);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _pawn.OnEffectValueChanged -= UpdateEffect;
    }

}
