using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        UpdateState(State.NoBattery);
    }

    public void UpdateState(State newState)
    {
        currentState = newState;
        UpdateGUI();
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
}