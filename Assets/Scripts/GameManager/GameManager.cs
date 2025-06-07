using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//싱글톤으로 데이터 관리
//싱글톤을 왜 사용하는가? -> 싱글톤은 클래스가 1개의 객체를 갖도록 보장
//1개의 인스턴스만을 가지며, 어디서든 이 인스턴스에 접근 가능하게함
//모든 클래스가 같은 인스턴스를 사용하기에 데이터의 중복이나 엉키는 문제를 방지할 수 있음
//메모리 관리나, 글로벌 엑세스 제공과 같은 이유가있지만 위의 데이터 관리측면에서의
//사용 이점이 더 커보엿음

//슬더스같은 게임을 목표로 하는 튜토리얼이라
//hp랑 경험치, 덱이 게임에 1개만 존재함 그래서 나중에 세팅 변경 필요
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public OptionManager OptionManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public DeckManager DeckManager { get; private set; }

    public bool PlayingCard = false;


    private int playerHealth;
    private int playerXP;
    private int difficulty = 5;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        OptionManager = GetComponentInChildren<OptionManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        DeckManager = GetComponentInChildren<DeckManager>();

        if(OptionManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameManager/OptionManager");
            if(prefab == null)
            {
                Debug.Log($"OptionManager prefab has not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                OptionManager = GetComponentInChildren<OptionManager>();
            }
        }

        if (AudioManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameManager/AudioManager");
            if (prefab == null)
            {
                Debug.Log($"AudioManager prefab has not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                AudioManager = GetComponentInChildren<AudioManager>();
            }
        }

        if (DeckManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameManager/DeckManager");
            if (prefab == null)
            {
                Debug.Log($"DeckManager prefab has not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                DeckManager = GetComponentInChildren<DeckManager>();
            }
        }
    }

    

    public int PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = value; }
    }

    public int PlayerXP
    {
        get { return playerXP; }
        set { playerXP = value;}
    }

    public int Diffculty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }
}
