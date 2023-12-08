using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ball;

    // 권장 설정
    // 포핸드 hitMode = "Forehand", angle = 30, power = 710 
    // 볼리 hitMode = "Volley", angle = 20, power = 700
    // 스매쉬 hitMode = "Smash", angle = 40, power = 620
    public bool canCustomize = false;
    public string hitMode;
    [Range(-45, 90)]
    public float angle = 20f;
    [Range(100, 1000)]
    public float power = 700f;

    private float dummy_height = 1.5f;
    void Start()
    {
        //PracticeManager로부터 현재의 hitMode 가져옴
        hitMode = PracticeManager.instance.practiceMode;

        if (hitMode == "Smash" || hitMode == "smash")
        {
            // 더미 플레이어 위치를 로컬벡터로 넘김
            ReadyHit(new Vector3(-1f, 0.75f, -3.0f));
            angle = 40f;
            power = 620f;
        }
        else if (hitMode == "Forehand" || hitMode == "forehand")
        {
            ReadyHit(new Vector3(-1f, 0.75f, -5.25f));
            angle = 30f;
            power = 680f;
        }
        else if (hitMode == "Volley" || hitMode == "volley")
        {
            ReadyHit(new Vector3(-1f, 0.75f, -3.0f));
            angle = 20f;
            power = 700f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (canCustomize) {
            if(hitMode != PracticeManager.instance.practiceMode) {
                PracticeManager.instance.practiceMode = hitMode;
                PracticeManager.instance.poseText.text = $"{hitMode}(canCustomize)";
            }
        }
        #endif

        if (PracticeManager.instance.nowRound > PracticeManager.instance.maxRound) {
            return;
        }

        //PracticeManager로부터 타이머 가져오기
        if (PracticeManager.instance.isRoundCount == true) {
            if (PracticeManager.instance.roundTimeCount > PracticeManager.instance.reloadTime) {
                //공 던지기 남은 시간 0초 시 공 발사
                Hit();
                PracticeManager.instance.isRoundCount = false;
                //TotalTrialText.text = $"{hitCount}/{totalTrial}";
            } else if (PracticeManager.instance.roundTimeCount > PracticeManager.instance.reloadTime - 3) {
                //공 던지기 전 준비
                ReloadHit();
            }
        }
    }

    Vector3 currentShooterPos;
    Vector3 originalPosition;
    Quaternion originalRotation;
    private void ReadyHit(Vector3 dummyPos)
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;

        // 더미 플레이어 적당한 스매위 위치 설정
        transform.localPosition = dummyPos;

        // 더미 플레이어 머리 위에 공 설치  
        currentShooterPos = transform.localPosition;
        Ball.transform.localPosition = new Vector3(currentShooterPos.x + 1f, dummy_height - 0.5f, currentShooterPos.z);

        // 초기값 저장
        originalPosition = Ball.transform.position;
        originalRotation = Ball.transform.rotation;
    }

    private Vector3 dir;
    private void Hit()
    {
        Ball.GetComponent<Rigidbody>().useGravity = true;

        // 공에 힘 가하기
        dir = Quaternion.AngleAxis(-1 * angle, Vector3.right) * Vector3.forward;
        Ball.GetComponent<Rigidbody>().AddForce(dir * power);
        Ball.GetComponent<Rigidbody>().AddTorque(Vector3.right * power);
    }
    public void ReloadHit()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;

        // 공 위치 리셋
        Ball.transform.position = originalPosition;
        Ball.transform.rotation = originalRotation;

        // 회전 초기화
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //궤적 초기화(공 위치 초기화 시 일직선의 긴 선 그려지는 것 방지)
        PracticeManager.instance.trailRenderer.InitPoint();
    }
}
