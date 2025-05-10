using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleSceneController : MonoBehaviour
{
    [Header("Buttons")]
    public Button startGameButton;        // 게임 시작 버튼
    public Button deckBuildingButton;     // 덱 구성 버튼
    public Button quitGameButton;         // 게임 종료 버튼
    public Button settingsButton;         // 설정 버튼

    [Header("Scene Names")]
    public string gamePlaySceneName = "Game_Play_Scene";
    public string deckBuildingSceneName = "Deck_Building_Scene";

    private void Awake()
    {
        // 버튼 클릭 이벤트 연결
        startGameButton.onClick.AddListener(OnStartGame);
        deckBuildingButton.onClick.AddListener(OnDeckBuilding);
        quitGameButton.onClick.AddListener(OnQuitGame);
        settingsButton.onClick.AddListener(OnOpenSettings);
    }

    private void OnDestroy()
    {
        // 메모리 누수 방지용 리스너 해제
        startGameButton.onClick.RemoveListener(OnStartGame);
        deckBuildingButton.onClick.RemoveListener(OnDeckBuilding);
        quitGameButton.onClick.RemoveListener(OnQuitGame);
        settingsButton.onClick.RemoveListener(OnOpenSettings);
    }

    /// <summary>
    /// 게임 시작 → Game_Play_Scene 로드
    /// </summary>
    private void OnStartGame()
    {
        SceneManager.LoadScene(gamePlaySceneName);
    }

    /// <summary>
    /// 덱 구성 → DeckBuildingScene 로드
    /// </summary>
    private void OnDeckBuilding()
    {
        SceneManager.LoadScene(deckBuildingSceneName);
    }

    /// <summary>
    /// 게임 종료
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
    /// 설정창 오픈 (추후 구현 예정)
    /// </summary>
    private void OnOpenSettings()
    {
        Debug.Log("Settings 창 열기 - 아직 구현되지 않음");
        // TODO: Settings UI 활성화 로직 추가
    }
}
