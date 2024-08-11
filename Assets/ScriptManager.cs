using UnityEngine;

public class ScriptManager : MessageBroker
{
    [SerializeField] private MonoBehaviour scriptToManage; 
    [SerializeField] private Stage[] activeStages; // 이 스크립트가 활성화될 스테이지들

    protected override void OnStageChanged_broker(Stage newStage)
    {
        base.OnStageChanged_broker(newStage);
        UpdateScriptState(newStage);
    }

    private void UpdateScriptState(Stage currentStage)
    {
        if (scriptToManage != null)
        {
            bool shouldBeActive = System.Array.Exists(activeStages, stage => stage == currentStage);
            scriptToManage.enabled = shouldBeActive;
        }
        else
        {
            Debug.LogWarning("Script to manage is not assigned in the inspector.");
        }
    }

    // Unity 에디터에서 스크립트가 수정될 때마다 초기 상태를 설정
    private void OnValidate()
    {
        if (Application.isPlaying && scriptToManage != null)
        {
            UpdateScriptState(MainTimeManager.Instance.GetCurrentStage());
        }
    }
}