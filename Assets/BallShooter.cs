using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ball;
    public string hitMode;

    [Range(-90, 90)]
    public float angle = 0f;

    [Range(100, 1500)]
    public float power = 300f;

    private float dummy_height = 1.5f;

    void Start()
    {
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
        if (timeCount > 10)
        {
            Smash();
            timeCount = 0;
        }
        else if (timeCount > 8)
        {
            ReadySmash();
        }
    }

    Vector3 currentShooterPos;

    private void ReadySmash()
    {
        Ball.GetComponent<Rigidbody>().useGravity = false;
        // 더미 플레이어 적당한 스매위 위치 설정
        transform.localPosition = new Vector3(0, 0.75f, -1.547f);

        // 더미 플레이어 머리 위에 공 설치  
        currentShooterPos = transform.localPosition;
        Ball.transform.localPosition = new Vector3(currentShooterPos.x, dummy_height + 0.5f, currentShooterPos.z);

        // // 회전 초기화
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
