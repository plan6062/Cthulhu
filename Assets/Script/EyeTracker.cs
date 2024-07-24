using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class EyeTracker : MonoBehaviour {
    public static EyeTracker Instance;
    List<XRNodeState> nodeStatesCache = new List<XRNodeState>();
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
    public bool TryGetCenterEyeNodeStateRotation(out Quaternion rotation)
    {
        InputTracking.GetNodeStates(nodeStatesCache);
        for (int i = 0; i < nodeStatesCache.Count; i++)
        {
            XRNodeState nodeState = nodeStatesCache[i];
            if(nodeState.nodeType == XRNode.CenterEye)
            {
                if (nodeState.TryGetRotation(out rotation))
                    return true;
            }
        }
        // This is the fail case, where there was no center eye was available.
        rotation = Quaternion.identity;
        return false;
    }

}