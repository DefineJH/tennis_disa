using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorShooter : MonoBehaviour
{

    public GameObject Player;
    public GameObject Ball;

    public GameObject TennisNet;

    // public bool canCustomize = false;

    // public string hitMode;
    public float angle = 40f;
    [Range(100, 1000)]
    public float power = 620f;
    private float dummy_height = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        middle = TennisNet.transform.position;
        ReadyHit(new Vector3(-1f, 0.75f, -3.0f));
    }

    private bool isFirstBall = true;

    private bool posAdjust = true;
    private bool switchPos = false;
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

        // Player.transform.position = new Vector3(1f, 1f, 1f);

        if (MirrorManager.instance.gameTimeCount > 10 && isFirstBall == true)
        {
            isFirstBall = false;
            FirstBall();
            posAdjust = false;
        }

        if (MirrorManager.instance.gameTimeCount > 15 && switchPos == false)
        {
            ReloadHit();
            Ball.GetComponent<Rigidbody>().useGravity = true;
            power = Vector3.Distance(Ball.GetComponent<MirrorBall>().acceleration, new Vector3(0, 0, 0));
            dir = Quaternion.AngleAxis(-1 * angle, Vector3.right) * Vector3.forward;
            //2.5는 임의값 
            Ball.GetComponent<Rigidbody>().AddForce(dir * power * 2.5f);
            switchPos = true;
        }



        //MirrorManager로부터 타이머 가져오기
        // if (MirrorManager.instance.gameTimeCount > MirrorManager.instance.reloadTime + 3 && posAdjust == false)
        // {
        //     Debug.Log("Pos Adjusted");
        //     Vector3 p0 = new Vector3(4.8858f, 0f, -0.6155435f);
        //     Vector3 p1 = new Vector3(-4.910632f, 0f, -0.6155435f);
        //     // Form a unit vector in the direction of the line.
        //     Vector3 lineDirection = (p1 - p0).normalized;

        //     // Rotate the vector 90 degrees in the XY plane
        //     // to get a vector perpendicular to the mirror line.
        //     Vector3 perpendicular = new Vector3(-lineDirection.z, 0f, lineDirection.x);
        //     // If you're working in the XZ plane instead, it's
        //     // (-lineDirection.z, 0, lineDirection.x)

        //     // Take away a's perpendicular offset from this line, twice.
        //     // Once to flatten a onto the line, and a second time to make b,
        //     // an equal distance away on the opposite side of the line.
        //     Vector3 a = Ball.GetComponent<MirrorBall>().ballOkPos;
        //     Vector3 b = a - 2 * Vector3.Dot((a - p0), perpendicular) * perpendicular;
        //     Debug.Log(b);
        //     Player.transform.position = b;
        //     posAdjust = true;
        // }
    }
    //MirrorManager로부터 타이머 가져오기
    // if (MirrorManager.instance.isRoundCount == true)
    // {
    //     if (MirrorManager.instance.roundTimeCount > MirrorManager.instance.reloadTime)
    //     {
    //         //공 던지기 남은 시간 0초 시 공 발사
    //         FirstBall();
    //         MirrorManager.instance.isRoundCount = false;
    //         //TotalTrialText.text = $"{hitCount}/{totalTrial}";
    //     }
    //     else if (MirrorManager.instance.roundTimeCount > MirrorManager.instance.reloadTime - 3 && MirrorManager.instance.gameTimeCount > 10)
    //     {
    //         //공 던지기 전 준비
    //         ReloadHit();
    //         Vector3 p0 = new Vector3(4.8858f, 0f, -0.6155435f);
    //         Vector3 p1 = new Vector3(-4.910632f, 0f, -0.6155435f);
    //         // Form a unit vector in the direction of the line.
    //         Vector3 lineDirection = (p1 - p0).normalized;

    //         // Rotate the vector 90 degrees in the XY plane
    //         // to get a vector perpendicular to the mirror line.
    //         Vector3 perpendicular = new Vector3(-lineDirection.z, 0, lineDirection.x);
    //         // If you're working in the XZ plane instead, it's
    //         // (-lineDirection.z, 0, lineDirection.x)

    //         // Take away a's perpendicular offset from this line, twice.
    //         // Once to flatten a onto the line, and a second time to make b,
    //         // an equal distance away on the opposite side of the line.
    //         Vector3 a = Ball.GetComponent<MirrorBall>().ballOkPos;
    //         Vector3 b = a - 2 * Vector3.Dot((a - p0), perpendicular) * perpendicular;
    //         Player.transform.position = b;
    //     }
    // }

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
    private void FirstBall()
    {
        Ball.GetComponent<Rigidbody>().useGravity = true;

        // 공에 힘 가하기
        dir = Quaternion.AngleAxis(-1 * angle, Vector3.right) * Vector3.forward;
        Ball.GetComponent<Rigidbody>().AddForce(dir * power);
        Ball.GetComponent<Rigidbody>().AddTorque(Vector3.right * power);
    }

    private Vector3 middle;
    public void ReloadHit()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;

        Ball.transform.position = 2 * middle - Player.transform.position;
        // Ball.transform.position = originalPosition;
        Ball.transform.rotation = originalRotation;

        // 공 위치 리셋
        Ball.transform.position = originalPosition;
        Ball.transform.rotation = originalRotation;

        // 회전 초기화
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //궤적 초기화(공 위치 초기화 시 일직선의 긴 선 그려지는 것 방지)
        MirrorManager.instance.trailRenderer.InitPoint();
    }
}
