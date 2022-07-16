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

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void UpdateBar(DamageData damageData)
    {
        base.UpdateBar(damageData);
        ShowDamageText(damageData);
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


}
