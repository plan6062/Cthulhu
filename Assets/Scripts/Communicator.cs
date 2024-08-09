using UnityEngine;
using System.Collections;

public class Communicator : Actor
{
    public enum State
    {
        NoBattery,
        PoweredOn,
        SearchingSatellite,
        Connected,
        SOSTransmitting
    }

    public State currentState = State.NoBattery;

    [System.Serializable]
    public class StateImage
    {
        public State state;
        public GameObject imageObject;
    }

    public StateImage[] stateImages;
    public float poweredOnDuration = 3f;
    public float fullConnectionDuration = 3f;
    public float connectedDuration = 3f;


    // 연결 진행률을 나타내는 Text 오브젝트들
    public GameObject[] connectionTexts; // 0%, 30%, 50%, 70%, 100% 순서로 할당

    private int[] connectionStages = { 0, 30, 50, 70, 100 };
    private int currentConnectionStage = 0;
    private bool isSearchingStarted = false;

    void Start()
    {
        UpdateState(State.NoBattery);
    }

    void Update()
    {
        if (currentState == State.SearchingSatellite)
        {
            CheckConnectionZones();
        }
    }

    public void UpdateState(State newState)
    {
        currentState = newState;
        UpdateGUI();

        if (newState == State.PoweredOn)
        {
            StartCoroutine(PoweredOnTimer());
        }
        else if (newState == State.SearchingSatellite)
        {
            ResetConnectionTexts();
            isSearchingStarted = true;
        }
        else if (newState == State.Connected)
        {
            StartCoroutine(ConnectedTimer());
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
        if (currentState == State.NoBattery)
        {
            UpdateState(State.PoweredOn);
            MainTimeManager.Instance.SetStage(Stage.Stage1_ConnectSatellite);

        }
    }

    private System.Collections.IEnumerator PoweredOnTimer()
    {
        yield return new WaitForSeconds(poweredOnDuration);
        UpdateState(State.SearchingSatellite);
    }

    private void ResetConnectionTexts()
    {
        for (int i = 0; i < connectionTexts.Length; i++)
        {
            connectionTexts[i].SetActive(i == 0); // 0%만 활성화, 나머지는 비활성화
        }
        currentConnectionStage = 0;
    }

    private void CheckConnectionZones()
    {
        if (!isSearchingStarted) return;

        bool inAnyZone = false;

        for (int i = connectionStages.Length - 1; i > 0; i--)
        {
            if (IsInConnectionZone($"ConnectionZone{connectionStages[i]}"))
            {
                UpdateConnectionStage(i);
                inAnyZone = true;
                break;
            }
        }

        if (!inAnyZone)
        {
            UpdateConnectionStage(0); // 어떤 구역에도 없으면 0% 상태로
        }
    }

    private bool IsInConnectionZone(string zoneName)
    {
        Collider zoneCollider = GameObject.Find(zoneName)?.GetComponent<Collider>();
        if (zoneCollider != null)
        {
            return zoneCollider.bounds.Contains(transform.position);
        }
        return false;
    }

    private void UpdateConnectionStage(int newStage)
    {
        if (newStage != currentConnectionStage)
        {
            for (int i = 0; i < connectionTexts.Length; i++)
            {
                connectionTexts[i].SetActive(i == newStage);
            }
            currentConnectionStage = newStage;

            if (currentConnectionStage == connectionTexts.Length - 1)
            {
                // 100% 연결 완료
                StartCoroutine(FullConnectionTimer());
            }
        }
    }

    private IEnumerator FullConnectionTimer()
    {
        yield return new WaitForSeconds(fullConnectionDuration);
        UpdateState(State.Connected);
    }

    private IEnumerator ConnectedTimer()
    {
        yield return new WaitForSeconds(connectedDuration);
        UpdateState(State.SOSTransmitting);
    }
}