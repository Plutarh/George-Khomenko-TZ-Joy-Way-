using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Pawn TargetPawn
    {
        get => _pawn;
    }

    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _damageImage;
    [SerializeField] protected GameObject _combatTextParent;
    [SerializeField] private Gradient _healthColorGradient;
    [SerializeField] private TextMeshProUGUI _healthText;

    protected Pawn _pawn;

    public void SetPawn(Pawn pawn)
    {
        _pawn = pawn;
        _pawn.OnTakedDamage += UpdateBar;

        _healthText.text = _pawn.Health.ToString("#");
        UpdateHealthGradient(_healthImage.fillAmount);
    }

    public void ResetBar()
    {
        _damageImage.fillAmount = 1;
        _healthImage.fillAmount = 1;
        UpdateHealthGradient(_healthImage.fillAmount);
    }

    public virtual void UpdateBar(DamageData damageData)
    {
        float targetValue = _pawn.GetHealth01();

        _healthText.text = _pawn.Health.ToString("#");

        _healthImage.DOFillAmount(targetValue, 0.1f).OnUpdate(() => UpdateHealthGradient(_healthImage.fillAmount)).OnComplete(() =>
        {
            _damageImage.DOFillAmount(targetValue, 0.2f);
        });
    }

    void UpdateHealthGradient(float fillAmount)
    {
        _healthImage.color = _healthColorGradient.Evaluate(fillAmount);
    }

    private void OnDestroy()
    {
        if (_pawn != null)
            _pawn.OnTakedDamage -= UpdateBar;
    }
}
