using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorShooter : MonoBehaviour
{
    public GameObject Player;
    public GameObject Ball;

    public GameObject MainCamera;
    public GameObject TennisNet;

    // public bool canCustomize = false;

    // public string hitMode;
    public float angle = 40f;
    [Range(100, 1000)]
    public float power = 620f;
    private float dummy_height = 1.5f;

    private Vector3 dummyPos;

    private Vector3 playerInitialPos;
    // Start is called before the first frame update
    void Start()
    {
        playerInitialPos = Player.transform.position;
        middle = TennisNet.transform.position;
        dummyPos = new Vector3(-1f, 0.0f, -3.0f);
        ReadyHit();

    }

    public bool isFirstBall = true;
    public bool switchPos = false;
    private Vector3 dummyVelocity;
    // Update is called once per frame
    void Update()
    {
        // #if UNITY_EDITOR
        //         if (canCustomize) {
        //             if(hitMode != PracticeManager.instance.practiceMode) {
        //                 PracticeManager.instance.practiceMode = hitMode;
        //                 PracticeManager.instance.poseText.text = $"{hitMode}(canCustomize)";
        //             }
        //         }
        // #endif

        if (MirrorManager.instance.roundTimeCount > 20 && switchPos == true)
        {
            MirrorManager.instance.GoNextRound();
        }
        // 유효 타일 때
        else if (MirrorManager.instance.roundTimeCount > 10 && switchPos == false)
        {
            Debug.Log(MirrorManager.instance.mirrorBall.okShot);
            if (MirrorManager.instance.mirrorBall.okShot == true) // 유효 타
            {
                Ball.GetComponent<Rigidbody>().useGravity = true;
                dummyVelocity = MirrorManager.instance.mirrorBall.capturedVelocity;
                dummyVelocity.z *= -1;
                dummyVelocity.x *= -1;
                Ball.GetComponent<Rigidbody>().velocity = dummyVelocity;
                MirrorManager.instance.mirrorBall.okShot = false;
            }
            switchPos = true;
        }
        // 유효 타일 때 (준비)
        else if (MirrorManager.instance.roundTimeCount > 7 && switchPos == false)
        {
            Debug.Log(MirrorManager.instance.mirrorBall.okShot);
            if (MirrorManager.instance.mirrorBall.okShot == true) // 유효 타
            {
                ReloadPlayerHit();
            }
            else // 유효 타가 아님
            {
                MirrorManager.instance.GoNextRound();
            }
        }
        // 첫 공
        else if (MirrorManager.instance.roundTimeCount > 3 && isFirstBall == true)
        {
            isFirstBall = false;
            FirstBall();
        }

    }

    Vector3 currentShooterPos;
    Vector3 originalPosition;
    Quaternion originalRotation;
    public void ReadyHit()
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
    private void FirstBall()
    {
        Ball.GetComponent<Rigidbody>().useGravity = true;

        // 공에 힘 가하기
        dir = Quaternion.AngleAxis(-1 * angle, Vector3.right) * Vector3.forward;
        Ball.GetComponent<Rigidbody>().AddForce(dir * power);
        Ball.GetComponent<Rigidbody>().AddTorque(Vector3.right * power);
    }

    private Vector3 middle;
    private Vector3 tmp;
    public void ReloadPlayerHit()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;

        // 네트 기준 점대칭 공 포지션
        tmp = 2 * middle - playerInitialPos;
        tmp.y = MirrorManager.instance.mirrorBall.collisionPos.y;
        Ball.transform.position = 2 * middle - playerInitialPos;
        transform.position = new Vector3(Ball.transform.position.x - 1, 0, Ball.transform.position.z);

        // 네트 기준 공 착률 지점 점대칭 유저 포지션
        tmp = 2 * middle - MirrorManager.instance.mirrorBall.ballLandPos;
        tmp.y = 0f;
        Player.transform.position = tmp;
        tmp.y = 1.6f;
        MainCamera.transform.position = tmp;

        Ball.transform.rotation = originalRotation;

        // 회전 초기화
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //궤적 초기화(공 위치 초기화 시 일직선의 긴 선 그려지는 것 방지)
        MirrorManager.instance.trailRenderer.InitPoint();
    }

    public void ReloadRegularHit()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;

        transform.localPosition = dummyPos;
        // 공 위치 리셋
        Ball.transform.position = originalPosition;
        Ball.transform.rotation = originalRotation;

        Player.transform.position = playerInitialPos;
        // 회전 초기화
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //궤적 초기화(공 위치 초기화 시 일직선의 긴 선 그려지는 것 방지)
        MirrorManager.instance.trailRenderer.InitPoint();
    }
}
