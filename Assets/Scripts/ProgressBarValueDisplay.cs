using UnityEngine;
using TMPro;
using MagicPigGames;

public class ProgressBarValueDisplay : MonoBehaviour
{
    [Header("연결할 프로그레스 바")]
    [Tooltip("Normalized(0~1) 값을 갖는 Progress Bar 컴포넌트")]
    public VerticalProgressBar progressBar;

    [Header("값을 표시할 텍스트")]
    [Tooltip("TextMeshProUGUI 또는 일반 Text를 사용하세요")]
    public TextMeshProUGUI valueText;

    [Header("최대 코스트 값")]
    [Tooltip("0부터 이 값까지 변환해서 표시합니다")]
    public int maxValue = 15;

    void Update()
    {
        // 1) 프로그레스 바에서 0~1 사이의 값 가져오기
        float normalized = progressBar.Progress;

        int current = maxValue - Mathf.RoundToInt(normalized * maxValue);

        // 3) 텍스트에 "현재값/최대값" 형태로 표시
        valueText.text = $"{current}/{maxValue}";
    }
}
