using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttach : MonoBehaviour
{
    public List<GameObject> objectsToAttach;

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in objectsToAttach)
        {
            obj.transform.parent = transform;
        }
    }
}