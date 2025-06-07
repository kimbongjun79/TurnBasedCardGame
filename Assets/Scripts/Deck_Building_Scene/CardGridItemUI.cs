using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// 카드 일러스트와 스탯(A: 공격력, B: 내구력, C: 코스트)을 표시하고,
/// 클릭/더블클릭 이벤트를 전달하는 컴포넌트입니다.
/// </summary>
public class CardGridItemUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Elements")]
    [Tooltip("카드 일러스트 배경 이미지")] public Image illustrationImage;
    [Tooltip("공격력 텍스트 (A)")] public TextMeshProUGUI attackText;
    [Tooltip("내구력 텍스트 (B)")] public TextMeshProUGUI durabilityText;
    [Tooltip("코스트 텍스트 (C)")] public TextMeshProUGUI costText;

    protected CardDataSO cardData;
    protected System.Action<CardDataSO> onClick;
    protected System.Action<CardDataSO> onDoubleClick;

    /// <summary>
    /// 카드 데이터를 바탕으로 UI를 초기화합니다.
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
    /// 클릭 및 더블클릭 이벤트 처리
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
    /// 외부에서 카드 데이터를 읽기 위한 프로퍼티
    /// </summary>
    public CardDataSO CardData => cardData;
}
