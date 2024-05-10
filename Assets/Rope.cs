using UnityEngine;

public class BulletLineRenderer : MonoBehaviour
{
    public Gun gunScript; // Gun 스크립트 참조
    public AnimationCurve affectCurve;
    private LineRenderer lineRenderer;

    public Material lineMaterial; // 라인 렌더러에 적용할 머티리얼

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        if (lineMaterial != null) // 머티리얼이 할당되어 있다면
        {
            lineRenderer.material = lineMaterial; // 라인 렌더러에 머티리얼 적용
        }
    }

    private void Update()
    {
        if (gunScript.Shooted) // 총알이 발사되었을 때
        {
            if (lineRenderer.positionCount != 2)
            {
                lineRenderer.positionCount = 2; // positionCount를 2로 설정
            }

            // 총구와 총알 위치 설정
            lineRenderer.SetPosition(0, gunScript.barrel.position); // 총구 위치
            lineRenderer.SetPosition(1, gunScript.bulletTransform.position); // 총알 위치

            // 출렁이는 효과 적용
            Vector3 direction = (gunScript.bulletTransform.position - gunScript.barrel.position).normalized;
            float distance = Vector3.Distance(gunScript.barrel.position, gunScript.bulletTransform.position);
            float timeRatio = distance / gunScript.bulletSpeed;
            float evaluateTime = Time.time - timeRatio;
            float swayOffset = affectCurve.Evaluate(evaluateTime);

            Vector3 swayVector = direction * swayOffset;
            lineRenderer.SetPosition(1, gunScript.bulletTransform.position + swayVector); // 총알 위치에 출렁이는 효과 적용
        }
        else
        {
            lineRenderer.positionCount = 0; // 총알이 발사되지 않았을 때는 positionCount를 0으로 설정
        }
    }
}
