using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
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
    public Transform cardGridContainer;
    public GameObject cardGridItemPrefab; // CardGridItemUI 프리팹

    [Header("Card Info UI")]
    public CardInfoUI cardInfoUI;

    [Header("Deck UI")]
    public Transform deckListContainer;
    public GameObject deckListItemPrefab; // DeckListItemUI 프리팹
    public TextMeshProUGUI deckCountText;

    private List<CardDataSO> allCards;
    private List<CardDataSO> filteredCards;
    private List<CardDataSO> currentDeck;

    private void Awake()
    {
        // 데이터베이스 로드
        if (cardDatabase == null)
            cardDatabase = Resources.Load<CardDatabaseSO>("CardDatabase");

        allCards = new List<CardDataSO>(cardDatabase.allCards);
        filteredCards = new List<CardDataSO>(allCards);
        currentDeck = new List<CardDataSO>();

        // 버튼 이벤트 연결
        backButton.onClick.AddListener(() =>
            UnityEngine.SceneManagement.SceneManager.LoadScene(titleSceneName));
        searchButton.onClick.AddListener(OnSearch);
    }

    private void Start()
    {
        PopulateGrid(filteredCards);
        UpdateDeckUI();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
        searchButton.onClick.RemoveAllListeners();
    }

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
        // 이전 그리드 초기화
        foreach (Transform child in cardGridContainer)
            Destroy(child.gameObject);

        // 새 카드 아이템 생성
        foreach (var card in cards)
        {
            var go = Instantiate(cardGridItemPrefab, cardGridContainer);
            var item = go.GetComponent<CardGridItemUI>();
            item.Setup(card,
                       c => cardInfoUI.DisplayCard(c),
                       AddCardToDeck);
        }
    }

    private void AddCardToDeck(CardDataSO card)
    {
        currentDeck.Add(card);
        var go = Instantiate(deckListItemPrefab, deckListContainer);
        var item = go.GetComponent<DeckListItemUI>();
        item.Setup(card, RemoveFromDeck);
        UpdateDeckUI();
    }

    private void RemoveFromDeck(DeckListItemUI item)
    {
        currentDeck.Remove(item.CardData);
        Destroy(item.gameObject);
        UpdateDeckUI();
    }

    private void UpdateDeckUI()
    {
        deckCountText.text = $"카드: {currentDeck.Count}장";
    }
}

