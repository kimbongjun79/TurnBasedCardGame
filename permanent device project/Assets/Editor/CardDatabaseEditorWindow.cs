using UnityEditor;
using UnityEngine;

public class CardDatabaseEditorWindow : EditorWindow
{
    private CardDatabaseSO cardDatabase;
    private string databaseResourcePath = "CardDatabase";  // Resources 폴더 내 경로 (확장자 제외)

    private string cardName = "New Card";
    private Sprite illustration;
    private int attack;
    private int durability;
    private int cost;
    private string effectDescription;
    private string loreDescription;

    [MenuItem("Window/Card Creator")]
    public static void ShowWindow()
    {
        GetWindow<CardDatabaseEditorWindow>("Card Creator");
    }

    private void OnEnable()
    {
        // Editor 창 활성화 시 자동으로 로드 시도
        LoadDatabase();
    }

    private void OnGUI()
    {
        GUILayout.Label("Card Database & Creator", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Database Resource Path:", GUILayout.Width(140));
        databaseResourcePath = EditorGUILayout.TextField(databaseResourcePath);
        if (GUILayout.Button("Load", GUILayout.Width(50)))
        {
            LoadDatabase();
        }
        EditorGUILayout.EndHorizontal();

        if (cardDatabase == null)
        {
            EditorGUILayout.HelpBox("카드 데이터베이스를 Resources에서 로드할 수 없습니다. 경로를 확인하세요.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();
        GUILayout.Label($"Loaded: {cardDatabase.name}", EditorStyles.label);

        EditorGUILayout.Space();
        GUILayout.Label("New Card Settings", EditorStyles.label);
        cardName = EditorGUILayout.TextField("Card Name", cardName);
        illustration = (Sprite)EditorGUILayout.ObjectField("Illustration", illustration, typeof(Sprite), false);
        attack = EditorGUILayout.IntField("Attack", attack);
        durability = EditorGUILayout.IntField("Durability", durability);
        cost = EditorGUILayout.IntField("Cost", cost);
        effectDescription = EditorGUILayout.TextField("Effect Description", effectDescription);
        loreDescription = EditorGUILayout.TextField("Lore Description", loreDescription);

        EditorGUILayout.Space();
        if (GUILayout.Button("Create and Add Card"))
        {
            CreateAndAddCard();
        }
    }

    private void LoadDatabase()
    {
        cardDatabase = Resources.Load<CardDatabaseSO>(databaseResourcePath);
        if (cardDatabase == null)
        {
            Debug.LogWarning($"Failed to load CardDatabaseSO at Resources/{databaseResourcePath}");
        }
    }

    private void CreateAndAddCard()
    {
        // Create new CardDataSO
        var newCard = ScriptableObject.CreateInstance<CardDataSO>();
        newCard.cardName = cardName;
        newCard.illustration = illustration;
        newCard.attack = attack;
        newCard.durability = durability;
        newCard.cost = cost;
        newCard.effectDescription = effectDescription;
        newCard.loreDescription = loreDescription;

        // Determine save path based on database asset location
        string dbAssetPath = AssetDatabase.GetAssetPath(cardDatabase);
        string dbFolder = System.IO.Path.GetDirectoryName(dbAssetPath);
        string assetPath = AssetDatabase.GenerateUniqueAssetPath(
            dbFolder + "/" + cardName + ".asset");

        // Save new card asset
        AssetDatabase.CreateAsset(newCard, assetPath);
        AssetDatabase.SaveAssets();

        // Add to database array
        Undo.RecordObject(cardDatabase, "Add Card");
        int oldLength = cardDatabase.allCards.Length;
        System.Array.Resize(ref cardDatabase.allCards, oldLength + 1);
        cardDatabase.allCards[oldLength] = newCard;
        EditorUtility.SetDirty(cardDatabase);
        AssetDatabase.SaveAssets();

        Debug.Log($"Created and added '{cardName}' to '{cardDatabase.name}'.");
    }
}
