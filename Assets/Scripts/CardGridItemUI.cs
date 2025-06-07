using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// 카드 일러스트와 스탯(A: 공격력, B: 내구력, C: 코스트)을 표시하고,
/// 클릭/더블클릭 이벤트를 전달하며, 호버 시 밝기 변화 제어 기능을 포함합니다.
/// </summary>
public class CardGridItemUI : MonoBehaviour,
                             IPointerClickHandler,
                             IPointerEnterHandler,
                             IPointerExitHandler
{
    [Header("UI Elements")]
    [Tooltip("카드 일러스트 배경 이미지")]
    public Image illustrationImage;
    [Tooltip("공격력 텍스트 (A)")]
    public TextMeshProUGUI attackText;
    [Tooltip("내구력 텍스트 (B)")]
    public TextMeshProUGUI durabilityText;
    [Tooltip("코스트 텍스트 (C)")]
    public TextMeshProUGUI costText;

    // 클릭(싱글/더블) 콜백
    private CardDataSO cardData;
    private System.Action<CardDataSO> onClick;
    private System.Action<CardDataSO> onDoubleClick;

    // 호버 및 다크 처리용 내부 상태
    private Vector3 originalScale;
    private Color originalColor;
    private bool isDarkened = false;   // 이 아이템이 '덱에 3장 채워져 어둡게 된 상태'인지 여부

    private void Awake()
    {
        // 원래 스케일과 색상을 저장
        originalScale = transform.localScale;
        originalColor = (illustrationImage != null) ? illustrationImage.color : Color.white;
    }

    /// <summary>
    /// 카드 데이터를 바탕으로 UI를 초기화합니다.
    /// </summary>
    public void Setup(CardDataSO data,
                      System.Action<CardDataSO> clickCallback,
                      System.Action<CardDataSO> doubleClickCallback)
    {
        cardData = data;
        onClick = clickCallback;
        onDoubleClick = doubleClickCallback;

        // 이미 어두운 상태라면, 이 Setup 호출 시에도 어두운 상태 유지
        if (!isDarkened)
        {
            // (1) 정상 상태로 초기화
            illustrationImage.color = originalColor;
            transform.localScale = originalScale;
        }

        // 카드 정보 표시
        illustrationImage.sprite = data.illustration;
        attackText.text = data.attack.ToString();
        durabilityText.text = data.durability.ToString();
        costText.text = data.cost.ToString();
    }

    #region ────── 클릭, 더블클릭 기능 ──────

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onClick?.Invoke(cardData);
            if (eventData.clickCount == 2)
                onDoubleClick?.Invoke(cardData);
        }
    }

    #endregion

    #region ────── 호버(마우스 오버) 기능 ──────

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 1) 이미 어두운 상태라면 호버 시 밝게 하지 않고, 바로 리턴
        if (isDarkened) return;

        // 2) 카드가 3장 이상 채워진 상태는 아니므로 호버 효과 적용
        //    (a) 크기를 10% 키워서 살짝 확대
        transform.localScale = originalScale * 1.08f;

        //    (b) 색상을 원본보다 60% 더 밝게(클램프)
        if (illustrationImage != null)
        {
            Color c = originalColor;
            float r = Mathf.Min(c.r + 0.6f, 1f);
            float g = Mathf.Min(c.g + 0.6f, 1f);
            float b = Mathf.Min(c.b + 0.6f, 1f);
            illustrationImage.color = new Color(r, g, b, c.a);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 1) Hover 해제 시, '어두운 상태'가 아닐 때만 원래 상태로 복구
        if (!isDarkened)
        {
            transform.localScale = originalScale;
            illustrationImage.color = originalColor;
        }
        // 2) 만약 isDarkened == true라면, 다시 Darken 상태를 유지(아무것도 하지 않음)
    }

    #endregion

    #region ────── 다크(비활성화) 처리 ──────

    /// <summary>
    /// 카드가 덱에 3장 꽉 찬 상태일 때 호출해 줍니다.
    /// 이 메서드를 호출하면 'isDarkened'가 true로 설정되고,
    /// 색상이 어두운 톤으로 바뀝니다. 이후 호버 시에도 밝아지지 않습니다.
    /// </summary>
    public void Darken()
    {
        isDarkened = true;

        // (a) 어두운 회색+반투명 처리
        if (illustrationImage != null)
            illustrationImage.color = new Color(0.3f, 0.3f, 0.3f, 0.7f);

        // (b) 크기는 원래대로 유지
        transform.localScale = originalScale;
    }

    /// <summary>
    /// 카드가 덱에서 제거되어 3장 상태가 해제될 때 호출해 줍니다.
    /// 'isDarkened' 플래그를 false로 되돌리고, 원래 색상으로 복귀합니다.
    /// </summary>
    public void Restore()
    {
        isDarkened = false;

        // (a) 원래 색상으로 복구
        if (illustrationImage != null)
            illustrationImage.color = originalColor;

        // (b) 크기도 원래대로
        transform.localScale = originalScale;
    }

    #endregion

    /// <summary>
    /// 외부에서 카드 데이터를 읽기 위한 프로퍼티
    /// </summary>
    public CardDataSO CardData => cardData;
}
