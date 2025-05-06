using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ������ ī���� �� ������ ǥ���ϴ� UI ��Ʈ�ѷ��Դϴ�.
/// </summary>
public class CardInfoUI : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("ī�� �̸� �ؽ�Ʈ")] public TextMeshProUGUI cardNameText;
    [Tooltip("ī�� �Ϸ���Ʈ �̹���")] public Image cardIllustration;
    [Tooltip("���ݷ� �ؽ�Ʈ")] public TextMeshProUGUI attackValueText;
    [Tooltip("������ �ؽ�Ʈ")] public TextMeshProUGUI durabilityValueText;
    [Tooltip("�ڽ�Ʈ �ؽ�Ʈ")] public TextMeshProUGUI costValueText;
    [Tooltip("ī�� ȿ�� ���� �ؽ�Ʈ")] public TextMeshProUGUI effectDescriptionText;
    [Tooltip("����� ���� �ؽ�Ʈ")] public TextMeshProUGUI loreDescriptionText;

    /// <summary>
    /// ���޵� ī�� �����͸� UI�� ǥ���մϴ�.
    /// </summary>
    /// <param name="card">ǥ���� ī�� ������</param>
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
