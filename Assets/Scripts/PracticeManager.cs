using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PracticeManager : MonoBehaviour
{
    public static PracticeManager instance;
    public RoundUIManager roundUIManager;
    public BallShooter ballShooter;
    public TrailRenderer trailRenderer;

    public string practiceMode;//현재 연습의 모드

    //UI
    public Text poseText;
    public Text hitText;

    //일시정지 기능
    public Button pauseButton;
    public GameObject PausePannel;

    // Start is called before the first frame update
    void Awake()
    {
        // 싱글턴 인스턴스 초기화
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
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
    public int reloadTime; //라운드당 소모 시간
    public float gameTimeCount = 0; //게임 전체 타이머
    public float roundTimeCount = 0; //라운드 한정 타이머
    public bool isRoundCount = true; //라운드 카운트 여부
    public bool isStartModel = false; //모델 스타트 여부
    public bool isPaused = false; //모델 스타트 여부

    public Text ReloadText;

    // Update is called once per frame
    /// <summary>
    /// 연습 씬 UI 업데이트
    /// </summary>
    void Update()
    {
        if (isStartModel) {
            //모델 작동 시 타이머 작동
            gameTimeCount += Time.deltaTime;

            if (isRoundCount) {
                roundTimeCount += Time.deltaTime;
                //공 던지기 남은 시간 표기
                ReloadText.text = $"{reloadTime - (int)roundTimeCount}";
            } else {
                //공이 던져졌을 때 go 표기
                ReloadText.text = $"Go!";
            }

            //연습 종료
            if (nowRound > maxRound) {
                ReloadText.text = $"End of Train";
                StartCoroutine(IEnumQuit(3f));//끝나면 나가기
            }

        } else {
            //모델 미 작동 시 Ready 표기
            ReloadText.text = $"Ready";
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused) {
            Time.timeScale = 0f; // 게임 일시정지
            PausePannel.gameObject.SetActive(true);
        } else {
            Time.timeScale = 1f; // 게임 재개
            PausePannel.gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        TogglePause();
        nowRound = 1; //현재 라운드
        gameTimeCount = 0; //게임 전체 타이머
        roundTimeCount = 0; //라운드 한정 타이머
        isRoundCount = true; //라운드 카운트 여부
        isStartModel = false; //모델 스타트 여부
        isPaused = false; //모델 스타트 여부
    }

    public void Quit()
    {
        TogglePause();
        SceneManager.LoadScene("LobbyScene"); // 로비 씬 로드
    }

    public void GoNextRound()
    {
        ballShooter.ReloadHit();
        roundTimeCount = 0;
        isRoundCount = true;
        nowRound += 1;
    }

    //타격 성공 or 실패 텍스트 보여주기(score: 0~100)
    public void ShowHitUI(int score)
    {
        string text;
        Color textColor;
        if (score > 80) {
            text = "Perfect!";
            textColor = Color.yellow;
        } else if (score > 50) {
            text = "Good!";
            textColor = Color.green;
        } else if (score > 30) {
            text = "Not Bad";
            textColor = Color.black;
        } else {
            text = "Fail";
            textColor = Color.red;
        }

        hitText.text = text;
        hitText.color = textColor;
        hitText.gameObject.SetActive(true);

        StartCoroutine(HideTextAfterTime(0.5f));
    }

    private IEnumerator HideTextAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        hitText.gameObject.SetActive(false); // 텍스트 숨김
    }

    private IEnumerator IEnumQuit(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("LobbyScene");
    }
}
