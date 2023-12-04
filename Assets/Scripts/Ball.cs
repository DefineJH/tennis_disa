using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    private bool isHitByRacket = false;

    void OnCollisionEnter(Collision collision)
    {
        // 라켓에 부딪혔을 때
        if (collision.gameObject.CompareTag("Racket")) {
            PracticeManager.instance.roundUIManager.SetRoundUIResult(PracticeManager.instance.nowRound, true);
            PracticeManager.instance.ShowHitUI(100);
            PracticeManager.instance.GoNextRound();
            isHitByRacket = true;
        }
        // 땅에 부딪혔을 때
        else if (collision.gameObject.CompareTag("Ground")) {
            if (isHitByRacket) {
                // 성공: 라켓에 부딪힌 후 땅에 닿음
            } else {
                // 실패: 라켓에 부딪히지 않고 땅에 닿음
                PracticeManager.instance.roundUIManager.SetRoundUIResult(PracticeManager.instance.nowRound, false);
                PracticeManager.instance.ShowHitUI(0);
                PracticeManager.instance.GoNextRound();
            }

            // 상태 초기화
            isHitByRacket = false;
        }
    }
}