using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TrailRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private List<float> pointTimes = new List<float>(); // 각 포인트의 생성 시간을 추적
    private float trailLifeTime = 1.0f; // 궤적이 지속되는 시간 (1초)

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Vector3 currentPosition = transform.position; // 현재 공의 위치

        // 공의 위치가 변경되었는지 확인
        if (points.Count == 0 || points[points.Count - 1] != currentPosition) {
            points.Add(currentPosition);
            pointTimes.Add(Time.time); // 현재 시간 추가
        }

        // 오래된 궤적 포인트 제거
        while (points.Count > 0 && Time.time > pointTimes[0] + trailLifeTime) {
            points.RemoveAt(0);
            pointTimes.RemoveAt(0);
        }

        // LineRenderer 업데이트
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
