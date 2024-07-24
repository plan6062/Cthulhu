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
    }
}