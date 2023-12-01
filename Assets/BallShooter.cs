using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ball;
    public Text ReloadText;
    public Text TotalTrialText;
    public int reloadTime = 10;
    public int totalTrial = 5;

    // 권장 설정
    // 스매쉬 hitMode = "Smash", angle = 40, power = 620 
    // 포핸드 hitMode = "Forehand", angle = 30, power = 710 
    // 볼리 hitMode = "Volley", angle = 20, power = 700
    public string hitMode;
    [Range(-45, 90)]
    public float angle = 0f;
    [Range(100, 1000)]
    public float power = 300f;

    private float dummy_height = 1.5f;
    void Start()
    {
        ReloadText.text = $"{reloadTime}";
        TotalTrialText.text = $"1/{totalTrial}";

        if (hitMode == "Smash" || hitMode == "smash")
        {
            // 더미 플레이어 위치를 로컬벡터로 넘김
            ReadyHit(new Vector3(-1f, 0.75f, -3.0f));
        }
        else if (hitMode == "Forehand" || hitMode == "forehand")
        {
            ReadyHit(new Vector3(-1f, 0.75f, -5.25f));
        }
        else if (hitMode == "Volley" || hitMode == "volley")
        {
            ReadyHit(new Vector3(-1f, 0.75f, -3.0f));
        }
    }

    // Update is called once per frame
    float timeCount = 0;
    int hitCount = 1;
    void Update()
    {
        if (hitCount > totalTrial)
        {
            ReloadText.text = $"End of Train";
            return;
        }


        timeCount += Time.deltaTime;

        if (timeCount > reloadTime)
        {
            Hit();
            timeCount = 0;
            hitCount += 1;
            TotalTrialText.text = $"{hitCount}/{totalTrial}";
        }
        else if (timeCount > reloadTime - 3)
        {
            ReloadHit();
        }
        ReloadText.text = $"{reloadTime - (int)timeCount}";

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
    private void ReloadHit()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;

        // 공 위치 리셋
        Ball.transform.position = originalPosition;
        Ball.transform.rotation = originalRotation;

        // 회전 초기화
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
