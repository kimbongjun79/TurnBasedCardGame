using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// CardGridItemUI�� ��ӹ޾�, �� ����Ʈ�� ���������� ��Ŭ�� ���� ����� �߰��մϴ�.
/// </summary>
public class DeckListItemUI : CardGridItemUI, IPointerClickHandler
{
    private System.Action<DeckListItemUI> onRightClick;

    /// <summary>
    /// �� ����Ʈ�� Setup: Ŭ���� ��Ȱ��, ���� Ŭ���� ��Ȱ��, ��Ŭ�� �ݹ鸸 Ȱ��ȭ
    /// </summary>
    public void Setup(CardDataSO data, System.Action<DeckListItemUI> rightClickCallback)
    {
        // Ŭ�� �� ���� Ŭ�� ������ ������� �����Ƿ� null ����
        base.Setup(data, null, null);
        onRightClick = rightClickCallback;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // ��Ŭ��, ����Ŭ�� ���� ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick?.Invoke(this);
        }
    }
}
