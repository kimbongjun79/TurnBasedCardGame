using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CardDatabaseEditorWindow : EditorWindow
{
    private CardDatabaseSO cardDatabase;
    private string databaseResourcePath = "CardDatabase";  // Resources/ 경로(확장자 제외)

    //—— 입력용 임시 변수(새 카드 생성 시 사용) ————————————————
    private string cardName = "New Card";                   // 카드 이름
    private Sprite illustration;                            // 일러스트
    private int attack;                                     // 공격력
    private int durability;                                 // 내구력
    private int cost;                                       // 비용

    private List<Vector2Int> attackRange = new List<Vector2Int>();  // 공격 범위 오프셋
    private List<Vector2Int> moveRange = new List<Vector2Int>();  // 이동 범위 오프셋

    private EffectTrigger effectTrigger;                    // 효과 발동 시점
    private EffectType effectType;                       // 효과 타입
    private int effectValue;                      // 효과 값
    private List<Vector2Int> effectRange = new List<Vector2Int>();  // 효과 범위 오프셋

    private string effectDescription;                       // 효과 설명
    private string loreDescription;                         // 스토리 설명

    [MenuItem("Window/카드 생성기")]
    public static void ShowWindow()
    {
        GetWindow<CardDatabaseEditorWindow>("카드 생성기");
    }

    private void OnEnable()
    {
        LoadDatabase();  // 창 열릴 때 자동 로드
    }

    private void OnGUI()
    {
        GUILayout.Label("카드 데이터베이스 및 생성기", EditorStyles.boldLabel);

        // — 데이터베이스 경로 입력 & 로드 ——————————————
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("데이터베이스 경로:", GUILayout.Width(100));
        databaseResourcePath = EditorGUILayout.TextField(databaseResourcePath);
        if (GUILayout.Button("로드", GUILayout.Width(50)))
            LoadDatabase();
        EditorGUILayout.EndHorizontal();

        if (cardDatabase == null)
        {
            EditorGUILayout.HelpBox("Resources에서 카드 데이터베이스를 찾을 수 없습니다.", MessageType.Warning);
            return;
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("로드된 DB:", cardDatabase.name);

        EditorGUILayout.Space();
        GUILayout.Label("새 카드 설정", EditorStyles.boldLabel);

        // —— Identification ——
        cardName = EditorGUILayout.TextField("카드 이름", cardName);
        illustration = (Sprite)EditorGUILayout.ObjectField("일러스트", illustration, typeof(Sprite), false);

        // —— Core Params ——
        attack = EditorGUILayout.IntField("공격력", attack);
        durability = EditorGUILayout.IntField("내구력", durability);
        cost = EditorGUILayout.IntField("비용", cost);

        // —— Ranges (Unit 전용) ——
        GUILayout.Label("공격 범위 (오프셋 리스트)", EditorStyles.label);
        DrawOffsetList(attackRange);

        GUILayout.Label("이동 범위 (오프셋 리스트)", EditorStyles.label);
        DrawOffsetList(moveRange);

        // —— Effects (Building/Spell) ——
        effectTrigger = (EffectTrigger)EditorGUILayout.EnumPopup("효과 발동 시점", effectTrigger);
        effectType = (EffectType)EditorGUILayout.EnumPopup("효과 타입", effectType);
        effectValue = EditorGUILayout.IntField("효과 값", effectValue);

        GUILayout.Label("효과 범위 (오프셋 리스트)", EditorStyles.label);
        DrawOffsetList(effectRange);

        effectDescription = EditorGUILayout.TextField("효과 설명", effectDescription);
        loreDescription = EditorGUILayout.TextField("스토리 설명", loreDescription);

        EditorGUILayout.Space();
        if (GUILayout.Button("카드 생성 및 추가"))
        {
            CreateAndAddCard();
        }
    }

    // Resources에서 CardDatabaseSO 로드
    private void LoadDatabase()
    {
        cardDatabase = Resources.Load<CardDatabaseSO>(databaseResourcePath);
        if (cardDatabase == null)
            Debug.LogWarning($"[{nameof(CardDatabaseEditorWindow)}] '{databaseResourcePath}' 경로에서 DB 로드 실패");
    }

    //  Vector2Int 리스트를 에디터에 그려 주는 헬퍼
    private void DrawOffsetList(List<Vector2Int> list)
    {
        int removeIndex = -1;
        for (int i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            list[i] = EditorGUILayout.Vector2IntField($"  {i}", list[i]);
            if (GUILayout.Button("삭제", GUILayout.Width(50)))
                removeIndex = i;
            EditorGUILayout.EndHorizontal();
        }
        if (removeIndex >= 0)
            list.RemoveAt(removeIndex);

        if (GUILayout.Button("추가"))
            list.Add(Vector2Int.zero);
    }

    // 새 CardDataSO 에셋 생성 및 DB에 추가
    private void CreateAndAddCard()
    {
        // 1) 새로운 ScriptableObject 인스턴스 생성
        var newCard = ScriptableObject.CreateInstance<CardDataSO>();
        newCard.cardName = cardName;
        newCard.illustration = illustration;
        newCard.attack = attack;
        newCard.durability = durability;
        newCard.cost = cost;
        newCard.attackRange = new List<Vector2Int>(attackRange);
        newCard.moveRange = new List<Vector2Int>(moveRange);
        newCard.effectTrigger = effectTrigger;
        newCard.effectType = effectType;
        newCard.effectValue = effectValue;
        newCard.effectRange = new List<Vector2Int>(effectRange);
        newCard.effectDescription = effectDescription;
        newCard.loreDescription = loreDescription;

        // 2) DB 에셋과 같은 폴더에 저장 경로 생성
        string dbPath = AssetDatabase.GetAssetPath(cardDatabase);
        string dbFolder = System.IO.Path.GetDirectoryName(dbPath);
        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{dbFolder}/{cardName}.asset");

        // 3) 에셋 파일로 저장
        AssetDatabase.CreateAsset(newCard, assetPath);
        AssetDatabase.SaveAssets();

        // 4) DB 배열 크기 늘리고 새 카드 할당
        Undo.RecordObject(cardDatabase, "Add Card");
        int oldLen = cardDatabase.allCards.Length;
        System.Array.Resize(ref cardDatabase.allCards, oldLen + 1);
        cardDatabase.allCards[oldLen] = newCard;
        EditorUtility.SetDirty(cardDatabase);
        AssetDatabase.SaveAssets();

        Debug.Log($"새 카드 '{cardName}' 생성 및 DB에 추가 완료");
    }
}
