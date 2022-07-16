using DG.Tweening;
using TMPro;
using UnityEngine;

public class UICombatText : MonoBehaviour
{
    [SerializeField] private Color _baseDamageColor;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void ShowText(DamageData damageData)
    {
        _text.text = damageData.damage.ToString();
        _text.color = _baseDamageColor;

        Vector2 randomPosition = Random.insideUnitCircle * 100;

        _rectTransform.DOAnchorPos(randomPosition, 2.5f).SetEase(Ease.OutBack);
        _canvasGroup.DOFade(0, 0.7f).SetDelay(0.6f);
    }
}
