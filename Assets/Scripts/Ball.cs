using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool isHitByRacket = false;

    void OnCollisionEnter(Collision collision)
    {
        // 라켓에 부딪혔을 때
        if (collision.gameObject.CompareTag("Racket")) {
            PracticeManager.instance.roundUIManager.SetRoundResult(PracticeManager.instance.nowRound-1, true); //이미 라운드가 넘어간 상황이라 기존 라운드에 -1
            isHitByRacket = true;
        }
        // 땅에 부딪혔을 때
        else if (collision.gameObject.CompareTag("Ground")) {
            if (isHitByRacket) {
                // 성공: 라켓에 부딪힌 후 땅에 닿음
            } else {
                // 실패: 라켓에 부딪히지 않고 땅에 닿음
                PracticeManager.instance.roundUIManager.SetRoundResult(PracticeManager.instance.nowRound-1, false);
            }

            // 상태 초기화
            isHitByRacket = false;
        }
    }
}