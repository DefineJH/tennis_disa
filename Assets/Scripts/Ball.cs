using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    private bool isHitByRacket = false;
    void OnTriggerEnter(Collider other)
    {
        if (isHitByRacket)
        {
            // 성공: 라켓에 부딪힌 후 땅에 닿음
            if (other.gameObject.name == "opposite_okzone")
            {
                PracticeManager.instance.roundUIManager.SetRoundUIResult(PracticeManager.instance.nowRound, true);
                PracticeManager.instance.ShowHitUI(100);
                PracticeManager.instance.GoNextRound();
                isHitByRacket = false;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        Debug.Log("Hit object" + collision.gameObject.name);
        // 라켓에 부딪혔을 때
        if (collision.gameObject.CompareTag("Racket"))
        {
            
            var bt = collision.gameObject.GetComponentInParent<BTReceiver>();
            if(bt != null)
            {
                bt.MakeVibration();
            }
            isHitByRacket = true;
        }

        // 땅에 부딪혔을 때
        else if (collision.gameObject.CompareTag("Ground"))
        {
            PracticeManager.instance.roundUIManager.SetRoundUIResult(PracticeManager.instance.nowRound, false);
            PracticeManager.instance.ShowHitUI(0);
            PracticeManager.instance.GoNextRound();
            // 상태 초기화
            isHitByRacket = false;
        }
    }
}