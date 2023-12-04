using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PracticeManager : MonoBehaviour
{
    public static PracticeManager instance;
    public RoundUIManager roundUIManager;

    public string practiceMode;//현재 연습의 모드

    public Text poseText;

    // Start is called before the first frame update
    void Awake()
    {
        // 싱글턴 인스턴스 초기화
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
            return;
        }

        #if UNITY_EDITOR
        if (GameManager.instance == null) {
            // 임시 GameManager 생성
            GameObject tempGameManager = new GameObject("TempGameManager");
            tempGameManager.AddComponent<GameManager>();
            // 필요한 경우 임시 값 설정
            tempGameManager.GetComponent<GameManager>().SetGameMode(GameManager.GameMode.volley_practice); // 예시
        }
        #endif

        switch (GameManager.instance.selectedMode) {
            case GameManager.GameMode.forehand_practice:
                // 모드 1에 대한 로직
                practiceMode = "forehand";
                break;
            case GameManager.GameMode.volley_practice:
                // 모드 2에 대한 로직
                practiceMode = "volley";
                break;
            case GameManager.GameMode.smash_practice:
                // 모드 3에 대한 로직
                practiceMode = "smash";
                break;
            default:
                Debug.Log("error");
                break;
        }

        poseText.text = practiceMode;
    }

    public int maxRound; //전체 라운드
    public int nowRound = 1; //현재 라운드
    public int reloadTime = 10; //라운드당 소모 시간
    public float gameTimeCount = 0; //게임 전체 타이머
    public float roundTimeCount = 0; //라운드 한정 타이머
    public bool isStartModel = false; //모델 스타트

    public Text ReloadText;

    // Update is called once per frame
    void Update()
    {
        if (isStartModel) {
            gameTimeCount += Time.deltaTime;
            roundTimeCount += Time.deltaTime;

            ReloadText.text = $"{reloadTime - (int)roundTimeCount}";

            if (nowRound > maxRound) {
                ReloadText.text = $"End of Train";
            }
        } else {
            ReloadText.text = $"Ready";
        }
    }

    public void MoveNextRound()
    {
        roundTimeCount = 0;
        nowRound += 1;
    }
}
