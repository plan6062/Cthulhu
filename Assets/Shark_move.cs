using UnityEngine;

public class Shark_Move : MonoBehaviour
{
    public float forwardSpeed = 5f; // 상어의 전진 속도 (z 축 방향)
    public float curveSpeed = 5f; // 곡선으로 움직일 때의 속도
    public float curveAmount = 0.5f; // 곡선의 휘어짐 정도 (x 축 방향)
    public float turnSpeed = 5f; // 방향 전환 속도

    void Update()
    {
        // 상어가 z 축 방향으로 일정하게 전진하도록 설정
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;

        // 곡선 이동을 위한 x 축 방향 계산
        float curve = Mathf.Sin(Time.time * curveSpeed) * curveAmount;
        Vector3 curveMove = transform.right * curve;

        // 총 이동 벡터: z 축 전진과 x 축 곡선 이동의 합
        Vector3 totalMove = forwardMove + curveMove;

        // 상어 이동
        transform.position += totalMove;

        // 상어가 이동 방향을 바라보도록 설정 (y좌표는 고정)
        Vector3 lookDirection = new Vector3(totalMove.x, 0, totalMove.z).normalized;
        if (lookDirection != Vector3.zero) // 제로 벡터가 아닐 경우에만 방향 전환
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}
