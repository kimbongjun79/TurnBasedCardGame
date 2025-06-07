using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 선택한 카드의 상세 정보를 표시하는 UI 컨트롤러입니다.
/// </summary>
public class CardInfoUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("카드 이름 텍스트")] public TextMeshProUGUI cardNameText;
    [Tooltip("카드 일러스트 이미지")] public Image cardIllustration;
    [Tooltip("공격력 텍스트")] public TextMeshProUGUI attackValueText;
    [Tooltip("내구력 텍스트")] public TextMeshProUGUI durabilityValueText;
    [Tooltip("코스트 텍스트")] public TextMeshProUGUI costValueText;
    [Tooltip("카드 효과 설명 텍스트")] public TextMeshProUGUI effectDescriptionText;
    [Tooltip("세계관 설명 텍스트")] public TextMeshProUGUI loreDescriptionText;

    /// <summary>
    /// 전달된 카드 데이터를 UI에 표시합니다.
    /// </summary>
    /// <param name="card">표시할 카드 데이터</param>
    public void DisplayCard(CardDataSO card)
    {
        if (card == null) return;

        cardNameText.text = card.cardName;
        cardIllustration.sprite = card.illustration;
        attackValueText.text = card.attack.ToString();
        durabilityValueText.text = card.durability.ToString();
        costValueText.text = card.cost.ToString();
        effectDescriptionText.text = card.effectDescription;
        loreDescriptionText.text = card.loreDescription;
    }
}
