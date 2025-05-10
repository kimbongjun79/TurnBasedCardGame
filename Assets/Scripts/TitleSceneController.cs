using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleSceneController : MonoBehaviour
{
    [Header("Buttons")]
    public Button startGameButton;        // ���� ���� ��ư
    public Button deckBuildingButton;     // �� ���� ��ư
    public Button quitGameButton;         // ���� ���� ��ư
    public Button settingsButton;         // ���� ��ư

    [Header("Scene Names")]
    public string gamePlaySceneName = "Game_Play_Scene";
    public string deckBuildingSceneName = "Deck_Building_Scene";

    private void Awake()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        startGameButton.onClick.AddListener(OnStartGame);
        deckBuildingButton.onClick.AddListener(OnDeckBuilding);
        quitGameButton.onClick.AddListener(OnQuitGame);
        settingsButton.onClick.AddListener(OnOpenSettings);
    }

    private void OnDestroy()
    {
        // �޸� ���� ������ ������ ����
        startGameButton.onClick.RemoveListener(OnStartGame);
        deckBuildingButton.onClick.RemoveListener(OnDeckBuilding);
        quitGameButton.onClick.RemoveListener(OnQuitGame);
        settingsButton.onClick.RemoveListener(OnOpenSettings);
    }

    /// <summary>
    /// ���� ���� �� Game_Play_Scene �ε�
    /// </summary>
    private void OnStartGame()
    {
        SceneManager.LoadScene(gamePlaySceneName);
    }

    /// <summary>
    /// �� ���� �� DeckBuildingScene �ε�
    /// </summary>
    private void OnDeckBuilding()
    {
        SceneManager.LoadScene(deckBuildingSceneName);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void OnQuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// ����â ���� (���� ���� ����)
    /// </summary>
    private void OnOpenSettings()
    {
        Debug.Log("Settings â ���� - ���� �������� ����");
        // TODO: Settings UI Ȱ��ȭ ���� �߰�
    }
}
