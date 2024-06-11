using UnityEngine;
using DG.Tweening;
public class SharkMovement : MonoBehaviour
{
    public float speed;
    public float speedmin;
    public float speedmax;
    public float speedchangerate;
    public float noiseScale = 0.5f;
    public float noiseFrequency = 1f;
    public float circleRadius = 10f;
    public Transform raft; // 원형 경로의 중심점
    public float yMin = -2f;
    public float yMax = 2f;
    private bool transitioning = false;
    private float transitionStartTime;
    public float yTarget = -10f;
    private float initialY;
    private float angle;
    private float noiseOffsetX =50f;
    private float noiseOffsetY =50f;
    private float noiseOffsetZ = 50f;
    private float targetSpeed;
    private float speedChangeTimer;
    private float speedChangeInterval;
    private bool underWater;
    private float circleMoveTimeCounter;
    public Animator anim;
    
    public State currentState;
    public enum State { CircleMove, MoveTowardBoat_Start,MoveTowardBoat_ing, AttackBoat, Retreat }

    void Start()
    {
        currentState = State.CircleMove;
        angle = Random.Range(0f, 360f);
        SetRandomSpeed();
    }

    void Update()
    {
        
        if (currentState == State.CircleMove){
            CircleMove();
        } else if (currentState == State.MoveTowardBoat_Start){
            MoveTowardBoat_Start();
        } else if (currentState == State.AttackBoat){
            // AttackBoat();
        } 

        if (Input.GetMouseButton(0)){
            currentState = State.MoveTowardBoat_Start;
            hasInitializedTargetPosition = false;
            transform.position = record;
        }
    }

    void CircleMove()
    {
        // X와 Z 좌표는 원형을 그리며 이동
        angle += speed * Time.deltaTime; // 시간에 따라 각도 증가
        float radian = angle * Mathf.Deg2Rad;
        float x = raft.position.x + Mathf.Cos(radian) * circleRadius;
        float z = raft.position.z + Mathf.Sin(radian) * circleRadius;

        radian = (angle+10) * Mathf.Deg2Rad;
        float x_toward = raft.position.x + Mathf.Cos(radian) * circleRadius;
        float z_toward = raft.position.z + Mathf.Sin(radian) * circleRadius;

        // Y 좌표는 제한된 범위 내에서 노이즈를 추가하여 이동
        float time = Time.time * noiseFrequency;
        float y;
        if(underWater){
            if (!transitioning)
            {
                transitioning = true;
                transitionStartTime = Time.time;
                initialY = transform.position.y;
            }

            float transitionElapsed = Time.time - transitionStartTime;
            float t = transitionElapsed / 5f;
            y = Mathf.Lerp(initialY, yTarget, t);

            if (t >= 1f)
            {
                y = yTarget; // 최종적으로 yTarget에 고정
                currentState = State.MoveTowardBoat_Start;
            }
        } else {
            y = Mathf.Lerp(yMin, yMax, Mathf.PerlinNoise(noiseOffsetY, time));
        }

        // 노이즈를 추가하여 자연스러운 움직임을 만듦
        x += Mathf.PerlinNoise(noiseOffsetX, time) * noiseScale;
        z += Mathf.PerlinNoise(noiseOffsetZ, time) * noiseScale;

        // 새로운 위치로 이동
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;

        Vector3 newPosition_dir = new Vector3(x_toward, y, z_toward);
        transform.LookAt(newPosition_dir);

        circleMoveTimeCounter += Time.deltaTime;
        if (circleMoveTimeCounter > 1){
            underWater = true;
        }
        
        UpdateSpeed();    
    }
    void SetRandomSpeed()
    {
        targetSpeed = Random.Range(speedmin, speedmax);
        speedChangeInterval = Random.Range(2f, 4f);
        speedChangeTimer = 0f;
    }

    void UpdateSpeed()
    {
        speedChangeTimer += Time.deltaTime;
        if (speedChangeTimer >= speedChangeInterval)
        {
            SetRandomSpeed();
        }
        speed = Mathf.MoveTowards(speed, targetSpeed, Time.deltaTime * speedchangerate); // 10f는 속도 변화 속도 조절
    }

    private bool check1 = true;
    private void MoveTowardBoat_Start_rotation(){
        Vector3 directionToRaft = (raft.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToRaft);
        transform.rotation = lookRotation;
        check1 = false;
    }

    public float closerspeed;

    public float jumpHeight = 5f; // 포물선의 최대 높이
    // public float yTargetDrop = -10f; // 최종 y 좌표
    public float closedistance = 0.4f;
    public float jumpdistance = 0.2f;
    // public float downdistance = 0.1f;
    
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool hasInitializedTargetPosition = false;
    public float shakeStrength = 0.1f; // 떨림 강도
    public int shakeVibrato = 10; // 떨림 진동 수
    public float shakeDuration = 0.1f; // 떨림 지속 시간

    private bool checker = false;
    Vector3 record;
    void MoveTowardBoat_Start()
    {
        if (!checker) {
            transform.LookAt(raft);
            record = transform.position;
            checker = true;
        }
        if (!hasInitializedTargetPosition)
        {
            // 초기 위치 설정
            initialPosition = transform.position;
            targetPosition = new Vector3(raft.position.x, initialPosition.y, raft.position.z);
            hasInitializedTargetPosition = true;

            // XZ 평면상의 거리 계산
            float distance = Vector3.Distance(new Vector3(initialPosition.x, 0, initialPosition.z), new Vector3(targetPosition.x, 0, targetPosition.z));
            
            // Dotween 시퀀스 생성
            Sequence sequence = DOTween.Sequence();

            // 1단계: 거리의 0.6까지 y좌표 변화 없이 이동
            float stage1Distance = distance * closedistance;
            Vector3 stage1Position = initialPosition + (targetPosition - initialPosition).normalized * stage1Distance;
            sequence.Append(transform.DOMove(new Vector3(stage1Position.x, initialPosition.y, stage1Position.z), stage1Distance / closerspeed).SetEase(Ease.InCubic));

            // 2단계: 거리의 0.8까지 포물선을 그리며 점프
            float stage2Distance = distance * jumpdistance;
            Vector3 stage2Position = stage1Position + (targetPosition - stage1Position).normalized * stage2Distance;
            sequence.Append(transform.DOJump(new Vector3(stage2Position.x, jumpHeight, stage2Position.z), jumpHeight, 1, stage2Distance / closerspeed).SetEase(Ease.OutSine));
            sequence.Join(transform.DOShakePosition(stage2Distance / speed, shakeStrength, shakeVibrato, 90, false, false).SetLoops(-1, LoopType.Yoyo));
            // sequence.Join(transform.DORotate(new Vector3( transform.eulerAngles.x,90f, transform.eulerAngles.z), stage2Distance / speed).SetEase(Ease.OutQuad));

            // // 3단계: 거리의 0.9까지 포물선으로 떨어짐
            // float stage3Distance = distance * downdistance;
            // Vector3 stage3Position = stage2Position + (targetPosition - stage2Position).normalized * stage3Distance;
            // sequence.Append(transform.DOMove(new Vector3(stage3Position.x, yTargetDrop, stage3Position.z), stage3Distance / closerspeed).SetEase(Ease.InCubic));

            // 시퀀스 실행
            sequence.Play();
        }

        // 애니메이션 설정
        anim.SetBool("bite", true);
    }

}