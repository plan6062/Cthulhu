using System;
using UnityEngine;

public class Actor : MonoBehaviour
{
   public Transform SummonPosition;
   protected void SubscribeToStageChanges()
    {
        MessageBroker.messageBroker.BrokerMessage += Acting;
    }

    protected void UnsubscribeFromStageChanges()
    {
        MessageBroker.messageBroker.BrokerMessage -= Acting;
    }
   void OnEnable()
    {
        SubscribeToStageChanges();
    }

    void OnDisable()
    {
        UnsubscribeFromStageChanges();
    }

    protected virtual void Acting(Stage newStage)
    {
        // 각 스테이지별로 해당 Actor가 어떤 동작을 취해야 하는지 자식 클래스에서 구현
        // A 스테이지에서 B 스테이지로 넘어가는 순간, 그 순간에 어떻게 작동하는지를 구현해야 함.
        // 매 프레임마다 어떻게 동작할지는 자식 클래스의 Update함수에서 구현한다.
    }
}