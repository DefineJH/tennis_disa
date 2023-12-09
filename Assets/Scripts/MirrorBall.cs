﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MirrorBall : MonoBehaviour
{
    private bool isHitByRacket = false;

    public Vector3 ballOkPos;

    public Vector3 acceleration;

    void Start()
    {
        ballOkPos = transform.position;
    }
    public Vector3 lastVelocity;
    private bool checkAccel = false;
    void Update()
    {
        if (checkAccel == true)
        {
            acceleration = (GetComponent<Rigidbody>().velocity - lastVelocity) / Time.deltaTime;
            checkAccel = false;
        }
        lastVelocity = GetComponent<Rigidbody>().velocity;
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

                ballOkPos = transform.position;
                Debug.Log(ballOkPos);

            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // 라켓에 부딪혔을 때
        if (collision.gameObject.CompareTag("Racket"))
        {
            checkAccel = true;
            isHitByRacket = true;
        }

        // 땅에 부딪혔을 때
        // else if (collision.gameObject.CompareTag("Ground"))
        // {
        //     // MirrorManager.instance.roundUIManager.SetRoundUIResult(MirrorManager.instance.nowRound, false);
        //     MirrorManager.instance.ShowHitUI(0);
        //     MirrorManager.instance.GoNextRound();
        //     // 상태 초기화
        //     isHitByRacket = false;
        // }
    }
}