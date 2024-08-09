using System;
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
    Quaternion playerRotation;
    public bool CheckSightCollison(GameObject caster, String targetTag)
    {
        TryGetCenterEyeNodeStateRotation(out playerRotation);
        Ray ray = new Ray(caster.transform.position, playerRotation * Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.transform.CompareTag(targetTag))
            {
                return true;
            }
            else
            {
                return false;
            }
    }
}