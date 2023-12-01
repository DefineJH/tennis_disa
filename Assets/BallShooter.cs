using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ball;
    public Text ReloadText;

    public int reloadTime = 10;
    public string hitMode;

    [Range(-90, 90)]
    public float angle = 0f;

    [Range(100, 1500)]
    public float power = 300f;

    private float dummy_height = 1.5f;

    void Start()
    {
        ReloadText.text = $"{reloadTime}";
        if (hitMode == "Smash" || hitMode == "smash")
        {
            ReadySmash();
        }
    }

    // Update is called once per frame
    float timeCount = 0;
    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > reloadTime)
        {
            Smash();
            timeCount = 0;
        }
        else if (timeCount > reloadTime - 3)
        {
            ReloadSmash();
        }
        ReloadText.text = $"{reloadTime - (int)timeCount}";

    }

    Vector3 currentShooterPos;
    Vector3 originalPosition;
    Quaternion originalRotation;
    private void ReadySmash()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;
        // 더미 플레이어 적당한 스매위 위치 설정
        transform.localPosition = new Vector3(0, 0.75f, -1.547f);

        // 더미 플레이어 머리 위에 공 설치  
        currentShooterPos = transform.localPosition;
        Ball.transform.localPosition = new Vector3(currentShooterPos.x, dummy_height + 0.5f, currentShooterPos.z);

        // 초기값 저장
        originalPosition = Ball.transform.position;
        originalRotation = Ball.transform.rotation;
    }

    private void ReloadSmash()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;
        Ball.transform.position = originalPosition;
        Ball.transform.rotation = originalRotation;

        // 회전 초기화
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    Vector3 dir;
    private void Smash()
    {
        Ball.GetComponent<Rigidbody>().useGravity = true;
        // 공에 힘 가하기
        // angle = 8, power = 775 정도 스메시가 적당함
        dir = Quaternion.AngleAxis(-1 * angle, Vector3.right) * Vector3.forward;
        Ball.GetComponent<Rigidbody>().AddForce(dir * power);
        Ball.GetComponent<Rigidbody>().AddTorque(Vector3.right * power);
    }
}
