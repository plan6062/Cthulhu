using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // 코루틴

public class Communicator : MonoBehaviour
{
    public enum State
    {
        NoBattery,
        PoweredOn,
        SearchingSatellite,
        Connected
    }

    public State currentState = State.NoBattery;

    [System.Serializable]
    public class StateImage
    {
        public State state;
        public GameObject imageObject;
    }

    public StateImage[] stateImages;
    public float poweredOnDuration = 3f;  // PoweredOn 상태 지속 시간

    void Start()
    {
        UpdateState(State.NoBattery);
    }

    public void UpdateState(State newState)
    {
        currentState = newState;
        UpdateGUI();

        // PoweredOn 상태로 변경되면 타이머 시작
        if (newState == State.PoweredOn)
        {
            StartCoroutine(PoweredOnTimer());
        }
    }

    private void UpdateGUI()
    {
        foreach (var stateImage in stateImages)
        {
            if (stateImage.imageObject != null)
            {
                stateImage.imageObject.SetActive(stateImage.state == currentState);
            }
        }
    }

    public void InsertBattery()
    {
        UpdateState(State.PoweredOn);
    }

    private IEnumerator PoweredOnTimer()
    {
        yield return new WaitForSeconds(poweredOnDuration);
        UpdateState(State.SearchingSatellite);
    }
}