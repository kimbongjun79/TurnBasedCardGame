using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// ī�� �Ϸ���Ʈ�� ����(A: ���ݷ�, B: ������, C: �ڽ�Ʈ)�� ǥ���ϰ�,
/// Ŭ��/����Ŭ�� �̺�Ʈ�� �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class CardGridItemUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Elements")]
    [Tooltip("ī�� �Ϸ���Ʈ ��� �̹���")] public Image illustrationImage;
    [Tooltip("���ݷ� �ؽ�Ʈ (A)")] public TextMeshProUGUI attackText;
    [Tooltip("������ �ؽ�Ʈ (B)")] public TextMeshProUGUI durabilityText;
    [Tooltip("�ڽ�Ʈ �ؽ�Ʈ (C)")] public TextMeshProUGUI costText;

    protected CardDataSO cardData;
    protected System.Action<CardDataSO> onClick;
    protected System.Action<CardDataSO> onDoubleClick;

    /// <summary>
    /// ī�� �����͸� �������� UI�� �ʱ�ȭ�մϴ�.
    /// </summary>
    public virtual void Setup(CardDataSO data, System.Action<CardDataSO> clickCallback, System.Action<CardDataSO> doubleClickCallback)
    {
        cardData = data;
        onClick = clickCallback;
        onDoubleClick = doubleClickCallback;

        illustrationImage.sprite = data.illustration;
        attackText.text = data.attack.ToString();
        durabilityText.text = data.durability.ToString();
        costText.text = data.cost.ToString();
    }

    /// <summary>
    /// Ŭ�� �� ����Ŭ�� �̺�Ʈ ó��
    /// </summary>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onClick?.Invoke(cardData);
            if (eventData.clickCount == 2)
                onDoubleClick?.Invoke(cardData);
        }
    }

    /// <summary>
    /// �ܺο��� ī�� �����͸� �б� ���� ������Ƽ
    /// </summary>
    public CardDataSO CardData => cardData;
}
