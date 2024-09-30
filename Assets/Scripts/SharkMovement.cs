using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UIElements;
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
    private bool checker1 = true;

    public ParticleSystem waterSplash;
    public ParticleSystem BloodSplash;
    public State currentState;
    public enum State { Ready, CircleMove, MoveTowardBoat_Start,MoveTowardBoat_ing, AttackBoat, Retreat }

    void Start()
    {   
        currentState = State.Ready;
        angle = 0f;
        SetRandomSpeed();
        waterSplash.Stop();
        audioSource.clip = audioClip;
        StartCoroutine(Ready(20f));
    }

    IEnumerator Ready(float time){
        yield return new WaitForSeconds(time);
        currentState = State.CircleMove;
    }
    void Update()
    {
        if (currentState == State.CircleMove){
            CircleMove();
        } else if (currentState == State.MoveTowardBoat_Start){
            MoveTowardBoat_Start();
            waterSplash.Play();
        } else if (currentState == State.Retreat){
            // AttackBoat();
        } 

        if(isHitTiming&& checker1 && isHit){
            Retreat();
            StartCoroutine(ExecuteAfterDelay(2));
              
            checker1 = false;
            
        }

        

        if (Input.GetMouseButton(0)){
            currentState = State.MoveTowardBoat_Start;
            hasInitializedTargetPosition = false;
            transform.position = record;
        }
    }

    

    private Vector3 lastPositionBeforeAttack;
    public int smallcircleRadius;
    void CircleMove()
    {
        if(sharkhealth < 1){
            circleRadius = smallcircleRadius;
            circleMoveTimeCounter = 0;
            yMin = -0.5f;
            yMax = -0.5f;
        }
        sharkGuide.setTrue();
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
                underWater = false;
                circleMoveTimeCounter = 0;
                Debug.Log("체커 초기화");
                checker1 = true;
                isHit = false;
                
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
        if (circleMoveTimeCounter > 5){
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
        if(sharkhealth < 1){
            speed = 28;
        } else {
            speedChangeTimer += Time.deltaTime;
            if (speedChangeTimer >= speedChangeInterval)
            {
                SetRandomSpeed();
            }
            speed = Mathf.MoveTowards(speed, targetSpeed, Time.deltaTime * speedchangerate); // 10f는 속도 변화 속도 조절
        }
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

    public bool isHitTiming = false;
    public bool isHit = false;

    [SerializeField] private Transform FrontpathParent;
    [SerializeField] private Transform BackpathParent;
    
    [SerializeField] private PathType pathType;
    Vector3[] pathArray;
    private Sequence sequence_MovetoBoat;
    public SharkGuide sharkGuide;
    public Bahamut_old bahamut;

    public float dist1;
    public float disty;
    public AudioSource audioSource;
    public AudioClip audioClip;
    void MoveTowardBoat_Start()
    {
        if (!checker) {
            transform.LookAt(raft);
            record = transform.position;
            checker = true;
        }
        if (!hasInitializedTargetPosition)
        {
            anim.SetBool("bite", true);
            sharkGuide.setFalse();
            // 초기 위치 설정
            lastPositionBeforeAttack = transform.position;
            initialPosition = transform.position;
            targetPosition = new Vector3(raft.position.x, initialPosition.y, raft.position.z);
            hasInitializedTargetPosition = true;

            // XZ 평면상의 거리 계산
            float distance = Vector3.Distance(new Vector3(initialPosition.x, 0, initialPosition.z), new Vector3(targetPosition.x, 0, targetPosition.z));

            
            // Dotween 시퀀스 생성
            sequence_MovetoBoat = DOTween.Sequence();
            // 1단계: 거리의 0.6까지 y좌표 변화 없이 이동
            float stage1Distance = distance * closedistance;
            Vector3 stage1Position = initialPosition + (targetPosition - initialPosition).normalized * stage1Distance;
            sequence_MovetoBoat.Append(transform.DOMove(new Vector3(stage1Position.x, initialPosition.y, stage1Position.z), stage1Distance / closerspeed).SetEase(Ease.InCubic));
            // 2단계: 거리의 0.8까지 포물선을 그리며 점프
            float stage2Distance = distance * jumpdistance;
            Vector3 stage2Position = stage1Position + (targetPosition - stage1Position).normalized * stage2Distance;
            
            audioSource.Play();
            sequence_MovetoBoat.Append(transform.DOJump(new Vector3(stage2Position.x, jumpHeight, stage2Position.z), jumpHeight, 1, stage2Distance / closerspeed).SetEase(Ease.OutSine));
            sequence_MovetoBoat.Join(transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 60f), stage2Distance / closerspeed).SetEase(Ease.OutCirc));
            
            sequence_MovetoBoat.AppendCallback(() =>
            {
                isHitTiming = true;
            });
            
            sequence_MovetoBoat.AppendInterval(2f);
            
            sequence_MovetoBoat.AppendCallback(() => {
                if (!isHit && checker1)
                {
                    anim.SetBool("bite", false);
                    Debug.Log("JDKsij");
                    isHitTiming = false;
                    Vector3[] pathArray = new Vector3[BackpathParent.childCount];
                    for (int i = 0; i < pathArray.Length; i++)
                    {
                        pathArray[i] = BackpathParent.GetChild(i).position;
                    }
                    transform.DOPath(pathArray, 5f, pathType).OnComplete(() => {
                        
                        currentState = State.CircleMove;
                        hasInitializedTargetPosition = false;
                        checker = false;
                    });
                }
                }
            );
        
            

            // 시퀀스 실행
            sequence_MovetoBoat.Play();
        }

        // 애니메이션 설정
        
    }
    IEnumerator ExecuteAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("2초 후 실행");
        isHitTiming = false;
        currentState = State.CircleMove;
        hasInitializedTargetPosition = false;
        checker = false;
        anim.SetBool("hurt", false);
        anim.SetBool("bite", false);
    }
    IEnumerator DeathAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        
    }
    public float deathtime;
    public Gun gun;
    public void Death(){
        gun.bulletSpeed = gun.bulletSpeed *3f;
        StartCoroutine(DeathAfterDelay(deathtime));
    }
    public int sharkhealth = 2;
    void Retreat(){
        
        anim.SetBool("hurt", true);
        // sequence_MovetoBoat.Kill();
        Vector3[] pathArray = new Vector3[BackpathParent.childCount];
        for(int i = 0; i < pathArray.Length; i++)
        {
            pathArray[i] = BackpathParent.GetChild(i).position;
        } 
        transform.DOPath(pathArray,5f,pathType);
        sharkhealth--;
        
        // isHit = false;        
    }

}