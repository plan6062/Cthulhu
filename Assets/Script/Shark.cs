using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections;

public class Shark : Actor
{
    Quaternion headRotation;
    public LineRenderer lineRenderer;
    public int numPoints = 50;
    public float moveSpeed = 5f;

    // Fixed points and parameters
    public Vector3 P0;
    private Vector3 P3;
    Quaternion dir;
    Vector3 direction; // This should be a normalized vector
    public float distanceM; // 이 값은 매번 

    private Vector3[] points;
    private int currentPointIndex = 0;

    void Start()
    {
        GenerateBezierCurve();
        // lineRenderer.positionCount = points.Length;
        // lineRenderer.SetPositions(points);
        StartCoroutine(MoveAlongCurve());
    }


    void Step()
    {
        //  생성 : 상어가 스폰될 위치를 결정한다. 플레이어가 바라보는 벡터에서 y성분을 제거한 벡터의 원점 기준 반대 벡터의 방향으로
        //  플레이어 위치에서 N만큼 이동 + y성분을 -2로 변경한 점에서 스폰된다.
        //  곡선 : 생성점을 시작으로 하는 선분을 만들어야 한다. 선분의 끝 점은 일정 범위 내의 랜덤 각도 / 길이를 이용해 결정한다.
        //  해당 점으로 이동 후, 45도 회전하며 이동 후, 또 직선 이동을 한다. 
        //  속도의 경우, 
        //  ㅅㅂ~ 그냥 윤재한테 부탁하자
    }
    void GenerateBezierCurve()
    {
        
        // EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out dir);
        // direction = dir.eulerAngles;
        // direction = new Vector3(-direction.x, 0, -direction.z);
        // P0 = direction.normalized*10;
        // direction = Vector3.Cross(direction.normalized, Vector3.up).normalized;
        // Define P3
        P0 = transform.position;
        direction = transform.forward;
        Vector3 P3 = P0 + direction * distanceM;
        Debug.Log(P0);
        Debug.Log(P3);
        Debug.Log(direction);

        // Define a vector perpendicular to the line P0P3
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized * (distanceM / 6);

        // Define P1 and P2 with some y-axis offset
        Vector3 yOffset = new Vector3(0, distanceM / 6, 0);
        Vector3 P1 = P0 + perpendicular + yOffset;
        Vector3 P2 = P3 + perpendicular - yOffset;

        // If perpendicular is zero vector, use another axis
        if (perpendicular == Vector3.zero)
        {
            perpendicular = Vector3.Cross(direction, Vector3.forward).normalized * (distanceM / 6);
            P1 = P0 + perpendicular + yOffset;
            P2 = P3 + perpendicular - yOffset;
        }

        // Calculate points on the Bezier curve
        points = new Vector3[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)(numPoints - 1);
            points[i] = CalculateCubicBezierPoint(t, P0, P1, P2, P3);
        }
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    IEnumerator MoveAlongCurve()
    {
        while (true)
        {
            if (currentPointIndex >= points.Length)
            {
                currentPointIndex = 0;
                yield return new WaitForSeconds(3);
                P0 = P3; // 이부분 이대로 가면 어색함. P0를 새로고침하는 과정을 조금 더 자연스럽게. 
                GenerateBezierCurve();
            }

            Vector3 targetPosition = points[currentPointIndex];
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                Vector3 direction = targetPosition - transform.position;
                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * moveSpeed);
                }
                yield return null;
            }

            currentPointIndex++;
            yield return null;
        }
    }

    public void Retreat(){
        EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
        Vector3 headRotation_euler = headRotation.eulerAngles;
        headRotation_euler.y = 0;
        Quaternion newRotation = Quaternion.Euler(headRotation_euler);
        // 이후 newRotation 방향으로 후퇴, 이후 스스로를 파괴
        // 트래킹 없이, 지정된 방향으로 후퇴?
    }

    protected override void Acting(Stage newStage)
    {
        switch (newStage)
        {
            // case Stage.Stage1_EnterZone1:
            //     // 처음 등장하는 타이밍. 따라서 해당 부분 작업은 Start() 안에서 이루어진다.
            //     break;
            // case Stage.Stage1_EnterZone2:
            //     break;
            // case Stage.Stage1_EnterZone3:
            //     break;
            case Stage.Stage1_SharkDissapear:
                Retreat();
                break;
            default:            
                break;
        }
    }
}