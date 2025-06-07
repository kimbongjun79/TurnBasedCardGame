/*using UnityEngine;
using UnityEngine.EventSystems;

public class CardToolTips : MonoBehaviour, IPointerDownHandler
{
    public CharacterTooltipDisplay tooltip;
    public float fadeTime = 0.1f;
    private bool tooltipVisible = false;

    void Awake()
    {
        tooltip = FindFirstObjectByType<CharacterTooltipDisplay>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (tooltip == null)
        {
            tooltip = FindFirstObjectByType<CharacterTooltipDisplay>();
            if (tooltip == null) return;
        }

        tooltip.SetStatsText(GetComponent<CharacterStat>());
        StopAllCoroutines(); // 혹시 이전 코루틴이 남아있을까봐
        StartCoroutine(Utility.FadeIn(tooltip.canvasGroup, 1.0f, fadeTime));
        tooltipVisible = true;
    }

    void Update()
    {
        if (tooltipVisible && Input.GetMouseButtonDown(0))
        {
            // UI 요소 위를 클릭했는지 확인
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // UI 외 영역 클릭 시 툴팁 숨김
                StartCoroutine(Utility.FadeOut(tooltip.canvasGroup, 0.0f, fadeTime));
                tooltipVisible = false;
            }
        }
    }
}*/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CardToolTips : MonoBehaviour, IPointerDownHandler
{
    public CharacterTooltipDisplay tooltip;
    public float fadeTime = 0.1f;
    private bool tooltipVisible = false;
    private bool justOpened = false;

    void Awake()
    {
        tooltip = FindFirstObjectByType<CharacterTooltipDisplay>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (tooltip == null)
        {
            tooltip = FindFirstObjectByType<CharacterTooltipDisplay>();
            if (tooltip == null) return;
        }

        tooltip.SetStatsText(GetComponent<CharacterStat>());
        StopAllCoroutines();
        StartCoroutine(Utility.FadeIn(tooltip.canvasGroup, 1.0f, fadeTime));
        tooltipVisible = true;
        justOpened = true; // 다음 프레임까진 클릭 감지 무시
    }

    void Update()
    {
        // 툴팁 막 열린 경우는 한 프레임 건너뛰기
        if (justOpened)
        {
            justOpened = false;
            return;
        }

        if (tooltipVisible && Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject() ||
                !Utility.IsPointerOverUIObject(tooltip.gameObject))
            {
                StartCoroutine(Utility.FadeOut(tooltip.canvasGroup, 0.0f, fadeTime));
                tooltipVisible = false;
            }
        }
    }
}
