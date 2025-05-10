using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// CardGridItemUI를 상속받아, 덱 리스트용 아이템으로 우클릭 제거 기능을 추가합니다.
/// </summary>
public class DeckListItemUI : CardGridItemUI, IPointerClickHandler
{
    private System.Action<DeckListItemUI> onRightClick;

    /// <summary>
    /// 덱 리스트용 Setup: 클릭은 비활성, 더블 클릭은 비활성, 우클릭 콜백만 활성화
    /// </summary>
    public void Setup(CardDataSO data, System.Action<DeckListItemUI> rightClickCallback)
    {
        // 클릭 및 더블 클릭 로직은 사용하지 않으므로 null 전달
        base.Setup(data, null, null);
        onRightClick = rightClickCallback;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭, 더블클릭 동작 무시
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick?.Invoke(this);
        }
    }
}
