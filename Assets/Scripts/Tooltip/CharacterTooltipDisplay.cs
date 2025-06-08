using TMPro;
using UnityEngine;

public class CharacterTooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI DamageText;
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    //[SerializeField] private float lerpFactor = 0.1f;
    //[SerializeField] private float xOffset = 200f;

    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

    }

    public void SetStatsText(CharacterStat stats)
    {
        nameText.text = $"{stats.cardName} Stats";
        cardType.text = string.Join(".", stats.cardType);
        healthText.text = stats.Durability.ToString();
        DamageText.text = stats.damage.ToString();
    }
}
