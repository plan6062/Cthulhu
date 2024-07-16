using System;
using UnityEngine;

public class MessageBroker : MonoBehaviour
{
    private Stage currentStage;
    public event Action<Stage> BrokerMessage;
    protected virtual void OnStageChanged_broker(Stage newStage)
    {
        currentStage = newStage;
        ChangeActorState(newStage);
        Debug.Log(gameObject.name + " received stage change to: " + newStage);
        BrokerMessage?.Invoke(currentStage);
    }

    protected virtual void ChangeActorState(Stage newStage)
    {
        // 이 함수의 내용은 자식 클래스에서 구현. newStage 에 따라서, 참조 중인 오브젝트를 활성화/비활성화한다.
        // 각 Stage 별로, 참조 중인 Actor의 활성/비활성화 타이밍만을 조절한다. 
        // Actor 클래스에서, 처음 시작시는 메세지를 받을 지 확실하지 않기 때문에, 
        // 시작 타이밍에 이루어져야 하는 작업은 Actor의 Start() 함수에 넣는다.   
    }
    protected void SubscribeToStageChanges()
    {
        MainTimeManager.Instance.TimeManagerMessage += OnStageChanged_broker;
    }

    protected void UnsubscribeFromStageChanges()
    {
        MainTimeManager.Instance.TimeManagerMessage -= OnStageChanged_broker;
    }

    void OnEnable()
    {
        SubscribeToStageChanges();
    }

    void OnDisable()
    {
        UnsubscribeFromStageChanges();
    }
}