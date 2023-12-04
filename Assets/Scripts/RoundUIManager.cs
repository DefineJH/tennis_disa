using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundUIManager : MonoBehaviour
{
    public GameObject roundPrefab; // 원형 UI 프리팹
    public Transform roundContainer; // 원 UI를 배치할 부모 컨테이너
    //public float spacing = 50f; // 원들 사이의 간격

    void Start()
    {
        //Vector3 startPosition = Vector3.zero; // 시작 위치
        int maxRound = PracticeManager.instance.maxRound; // 라운드 개수

        for (int i = 0; i < maxRound; i++) {
            Instantiate(roundPrefab, roundContainer);
            //RectTransform rt = round.GetComponent<RectTransform>();
            //rt.anchoredPosition = startPosition + new Vector3(spacing * i, 0, 0); // 위치 조정
        }

        //첫 라운드 색 흰색으로
        Image roundImage = roundContainer.GetChild(0).GetComponent<Image>();
        roundImage.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetRoundUIResult(int roundIndex, bool isSuccess)
    {
        if (roundIndex <= roundContainer.childCount) {
            Image roundImage = roundContainer.GetChild(roundIndex - 1).GetComponent<Image>();
            roundImage.color = isSuccess ? Color.green : Color.red; // 성공: 초록, 실패: 빨강

            if(roundIndex < roundContainer.childCount) {
                Image roundImage2 = roundContainer.GetChild(roundIndex).GetComponent<Image>();
                roundImage2.color = Color.white;
            }
        }
    }
}
