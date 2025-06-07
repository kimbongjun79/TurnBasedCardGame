using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DeckBuildingSceneController : MonoBehaviour
{
    [Header("Database")]
    [Tooltip("Inspector에서 드래그하거나, Resources에서 로드")]
    public CardDatabaseSO cardDatabase;

    [Header("Navigation")]
    public Button backButton;
    public string titleSceneName = "Title_Scene";

    [Header("Search UI")]
    public TMP_InputField searchInputField;
    public Button searchButton;

    [Header("Card Grid")]
    public Transform cardGridContainer;         // Scroll View → Viewport → Content
    public GameObject cardGridItemPrefab;       // CardGridItemUI 프리팹

    [Header("Card Info UI")]
    public CardInfoUI cardInfoUI;               // 카드 상세 정보 패널

    [Header("Deck UI")]
    public Transform deckListContainer;         // Deck List → Content
    public GameObject deckListItemPrefab;       // DeckListItemUI 프리팹
    public TextMeshProUGUI deckCountText;       // 덱 총 카드 수

    [Header("Warnings")]
    [Tooltip("화면 중앙에 띄울 경고용 TextMeshProUGUI (빨간색)")]
    public TextMeshProUGUI warningText;         // 초기 상태: 알파 0으로 두고 비활성화

    // 내부 데이터
    private List<CardDataSO> allCards;
    private List<CardDataSO> filteredCards;
    private List<CardDataSO> currentDeck; // 런타임에 실제 덱에 있는 카드 목록

    private void Awake()
    {
        // 1) 카드 데이터베이스 로드
        if (cardDatabase == null)
            cardDatabase = Resources.Load<CardDatabaseSO>("CardDatabase");

        allCards = new List<CardDataSO>(cardDatabase.allCards);
        filteredCards = new List<CardDataSO>(allCards);
        currentDeck = new List<CardDataSO>();

        // 2) 버튼 이벤트 연결
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(OnBackButtonPressed);

        searchButton.onClick.RemoveAllListeners();
        searchButton.onClick.AddListener(OnSearch);

        // 3) 경고 텍스트 초기화 (알파 0, 비활성화)
        if (warningText != null)
        {
            warningText.alpha = 0f;
            warningText.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        

        // 4) 저장된 덱 불러오기
        var savedIDs = DeckSaveManager.LoadDeckIDs();
        foreach (var id in savedIDs)
        {
            var card = allCards.Find(c => c.cardName == id);
            if (card != null && CountInCurrentDeck(card) < 3)
            {
                currentDeck.Add(card);
            }
        }
        // 5) 카드 그리드 초기화
        PopulateGrid(filteredCards);

        RefreshDeckListUI();
        UpdateDeckUI();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
        searchButton.onClick.RemoveAllListeners();
    }

    #region ───────────────────────────────── 검색 기능 ─────────────────────────────────

    private void OnSearch()
    {
        string query = searchInputField.text;
        if (string.IsNullOrEmpty(query))
            filteredCards = new List<CardDataSO>(allCards);
        else
            filteredCards = allCards.FindAll(c => c.cardName.Contains(query));

        PopulateGrid(filteredCards);
    }

    private void PopulateGrid(List<CardDataSO> cards)
    {
        // 기존 그리드 초기화
        foreach (Transform child in cardGridContainer)
            Destroy(child.gameObject);

        // 새 카드 아이템 생성
        foreach (var card in cards)
        {
            var go = Instantiate(cardGridItemPrefab, cardGridContainer);
            var item = go.GetComponent<CardGridItemUI>();

            // 1) 먼저 Setup 호출 (호버 시 원래 상태로 시작)
            item.Setup(
                card,
                clickCallback: c => cardInfoUI.DisplayCard(c),
                doubleClickCallback: c => AddCardToDeck(c)
            );

            // 2) 만약 이미 덱에 카드가 3장 이상 있다면 Darken() 호출
            if (CountInCurrentDeck(card) >= 3)
            {
                item.Darken();
            }
            else
            {
                // 혹시 이전에 Darken된 상태로 남아 있을까봐 Restore()도 가능
                item.Restore();
            }
        }
    }

    #endregion

    #region ───────────────────────────────── 덱 편성 기능 ─────────────────────────────────

    /// <summary>
    /// 덱에 카드를 추가할 때 사용하는 로직
    /// </summary>
    /// <param name="card">추가할 카드 데이터</param>
    /// <param name="saveImmediately">바로 파일로 저장할지 여부</param>
    private void AddCardToDeck(CardDataSO card, bool saveImmediately = true)
    {
        int existingCount = CountInCurrentDeck(card);
        if (existingCount >= 3)
        {
            Debug.Log("3장 초과 경고 실행");
            // 이미 3장이면 경고만 띄우고 종료
            StartCoroutine(ShowWarningCoroutine("덱에는 같은 종류의 카드를 최대 3장까지 사용할 수 있습니다!"));
            return;
        }

        currentDeck.Add(card);
        UpdateGridItemsForCard(card, darken: CountInCurrentDeck(card) >= 3);

        // 덱 UI 전체를 비용순으로 다시 그린다
        RefreshDeckListUI();
        UpdateDeckUI();

        DeckSaveManager.SaveDeck(currentDeck);
    }

    /// <summary>
    /// 덱에서 카드를 제거할 때 사용하는 로직
    /// </summary>
    private void RemoveFromDeck(DeckListItemUI item)
    {
        var removedCard = item.CardData;
        currentDeck.Remove(removedCard);

        // 제거 후, 만약 남은 개수가 2장 이하라면 그리드에서 밝게 복원
        if (CountInCurrentDeck(removedCard) < 3)
            UpdateGridItemsForCard(removedCard, darken: false);

        // 덱 UI 전체를 비용순으로 다시 그린다
        RefreshDeckListUI();
        UpdateDeckUI();

        DeckSaveManager.SaveDeck(currentDeck);
    }
    /// <summary>
    /// DeckListContainer 아래에 있는 모든 자식(DeckListItemUI)을 삭제하고,
    /// currentDeck을 cost 오름차순으로 정렬한 뒤 다시 Instantiate하여 표시합니다.
    /// </summary>
    private void RefreshDeckListUI()
    {
        // 1) 기존 리스트 아이템 모두 Destroy
        foreach (Transform child in deckListContainer)
        {
            Destroy(child.gameObject);
        }

        // 2) currentDeck을 비용(cost) 기준으로 오름차순 정렬
        var sorted = currentDeck.OrderBy(c => c.cost).ThenBy(c => c.cardName);

        // 3) 정렬된 순서대로 DeckListItemUI를 Instantiate
        foreach (var card in sorted)
        {
            var go = Instantiate(deckListItemPrefab, deckListContainer);
            var item = go.GetComponent<DeckListItemUI>();
            item.Setup(card, RemoveFromDeck);
        }
    }
    /// <summary>
    /// 특정 카드가 현재 덱에서 몇 장 사용 중인지 반환합니다.
    /// </summary>
    private int CountInCurrentDeck(CardDataSO card)
    {
        return currentDeck.Count(c => c.cardName == card.cardName);
    }

    /// <summary>
    /// 카드 그리드에 있는 모든 CardGridItemUI 중, 특정 카드 데이터와 일치하는 아이템을
    /// 어둡게(darken=true) 혹은 원래대로 복원(darken=false) 시킵니다.
    /// </summary>
    private void UpdateGridItemsForCard(CardDataSO card, bool darken)
    {
        foreach (Transform child in cardGridContainer)
        {
            var item = child.GetComponent<CardGridItemUI>();
            if (item != null && item.CardData.cardName == card.cardName)
            {
                if (darken)
                    item.Darken();
                else
                    item.Restore();
            }
        }
    }

    /// <summary>
    /// GridItem을 어둡게(비활성화 느낌) 처리하는 예시입니다.
    /// – Image 알파값을 반투명하게 낮추거나, 클릭 기능을 막으면 됩니다.
    /// </summary>
    private void DarkenGridItem(CardGridItemUI item)
    {
        // 예: 일러스트 이미지 반투명 처리
        item.illustrationImage.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    }

    /// <summary>
    /// GridItem을 원래대로(활성화된 상태) 복원합니다.
    /// </summary>
    private void RestoreGridItem(CardGridItemUI item)
    {
        item.illustrationImage.color = new Color(1f, 1f, 1f, 0.3921f);
        var collider = item.GetComponent<GraphicRaycaster>();
        if (collider != null)
            collider.enabled = true;
    }

    #endregion

    #region ───────────────────────────────── 경고 메시지 처리 ─────────────────────────────────

    /// <summary>
    /// 지정된 메시지를 빨간색으로 Fade In → 1.5초 대기 → Fade Out 한 뒤 비활성화합니다.
    /// </summary>
    private IEnumerator ShowWarningCoroutine(string message)
    {
        if (warningText == null)
            yield break;

        warningText.text = message;
        warningText.color = Color.red;
        warningText.gameObject.SetActive(true);

        // Fade In
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            warningText.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        warningText.alpha = 1f;

        // 1.5초 대기
        yield return new WaitForSeconds(1.5f);

        // Fade Out
        elapsed = 0f;
        while (elapsed < duration)
        {
            warningText.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        warningText.alpha = 0f;
        warningText.gameObject.SetActive(false);
    }

    #endregion

    #region ───────────────────────────────── 뒤로 가기 버튼 처리 ─────────────────────────────────

    /// <summary>
    /// Back 버튼 누를 때 호출됩니다.
    /// – 덱이 45장이 아니면 경고 메시지 띄우기
    /// – 덱이 45장이면 저장 후 씬 전환
    /// </summary>
    private void OnBackButtonPressed()
    {
        if (currentDeck.Count != 45)
        {
            StartCoroutine(ShowWarningCoroutine("총 45장의 카드를 덱에 추가해야 합니다!"));
            return;
        }

        // 덱이 45장일 때만 저장하고 씬 복귀
        DeckSaveManager.SaveDeck(currentDeck);
        UnityEngine.SceneManagement.SceneManager.LoadScene(titleSceneName);
    }

    #endregion

    /// <summary>
    /// 덱 UI에 현재 총 카드 수를 표시합니다.
    /// </summary>
    private void UpdateDeckUI()
    {
        if (deckCountText != null)
            deckCountText.text = $"카드: {currentDeck.Count}장";
    }
}
