using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MirrorBall : MonoBehaviour
{
    private bool isHitByRacket = false;

    public Vector3 ballOkPos;

    public Vector3 capturedVelocity;

    public Vector3 ballLandPos;
    public bool okShot = false;
    void Start()
    {
        ballOkPos = transform.position;
    }

    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (isHitByRacket)
        {
            // 성공: 라켓에 부딪힌 후 땅에 닿음
            if (other.gameObject.name == "opposite_okzone")
            {
                // MirrorManager.instance.roundUIManager.SetRoundUIResult(MirrorManager.instance.nowRound, true);
                MirrorManager.instance.ShowHitUI(100);
                // MirrorManager.instance.GoNextRound();
                isHitByRacket = false;
                okShot = true;
                ballLandPos = transform.position;
                Debug.Log(ballLandPos);

            }
        }
    }
    private Vector3 normal;
    public float reflectAngle;
    void OnCollisionEnter(Collision collision)
    {
        // 라켓에 부딪혔을 때
        if (collision.gameObject.CompareTag("Racket"))
        {

            ballOkPos = transform.position;
            isHitByRacket = true;
            Debug.Log("Hit");
            capturedVelocity = GetComponent<Rigidbody>().velocity;

            // normal = collision.contacts[0].normal;
            // reflectAngle = 180f - Vector3.Angle(capturedVelocity, -normal);
            // Debug.Log(reflectAngle);
        }

        //땅에 부딪혔을 때
        else if (collision.gameObject.CompareTag("Ground") && okShot == false)
        {
            // MirrorManager.instance.roundUIManager.SetRoundUIResult(MirrorManager.instance.nowRound, false);
            MirrorManager.instance.ShowHitUI(0);
            // MirrorManager.instance.GoNextRound();
            // 상태 초기화
            isHitByRacket = false;
            okShot = false;
        }
    }
}