using System;
using UnityEngine;

public class MainTimeManager : MonoBehaviour
{
    public static MainTimeManager Instance; // 싱글톤 인스턴스
    public Stage initialStage = Stage.GameStart; // 초기 스테이지 설정

    // 이벤트 선언
    public event Action<Stage> TimeManagerMessage;

    public Stage currentStage;

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 초기 스테이지 설정
        SetStage(initialStage);
    }

    public void SetStage(Stage newStage)
    {
        if (currentStage != newStage)
        {
            currentStage = newStage;
            Debug.Log("Stage changed to: " + currentStage);
            TimeManagerMessage?.Invoke(currentStage);
        }
    }

    public Stage GetCurrentStage()
    {
        return currentStage;
    }
}
