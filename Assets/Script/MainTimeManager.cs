using System;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

public class MainTimeManager : MonoBehaviour
{
    public static MainTimeManager Instance; // 싱글톤 인스턴스
    public Stage initialStage = Stage.GameStart; // 초기 스테이지 설정

    // 이벤트 선언
    public event Action<Stage> TimeManagerMessage;
    public Stage currentStage;

    public Stage ChangeStage = Stage.None;

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
        SetStage(initialStage,  this.GetType().Name);
    }

    void Update() {
        if(ChangeStage != currentStage && ChangeStage != Stage.None){
            SetStage(ChangeStage,  this.GetType().Name);
            currentStage = ChangeStage;
            ChangeStage = Stage.None;
        }    
    }    

    public void SetStage(Stage newStage, string actor)
    {
        if (currentStage != newStage)
        {
            currentStage = newStage;
            Debug.Log(actor + "changed to: " + currentStage);
            TimeManagerMessage?.Invoke(currentStage);
        }
    }

    public Stage GetCurrentStage()
    {
        return currentStage;
    }
}
