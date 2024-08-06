using UnityEngine;

public class Raft : Actor
{
    public float firstStageThreshold = 10f;
    public float secondStageThreshold = 40f;
    private bool firstStageTriggered = false;
    private bool secondStageTriggered = false;

    void Update()
    {
        if (!secondStageTriggered && transform.position.z > secondStageThreshold)
        {
            MainTimeManager.Instance.SetStage(Stage.Opening_Corpse2);
            secondStageTriggered = true;
        }
        else if (!firstStageTriggered && transform.position.z > firstStageThreshold)
        {
            MainTimeManager.Instance.SetStage(Stage.Opening_Corpse1);
            firstStageTriggered = true;
        }
    }
}