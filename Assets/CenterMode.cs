using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 modelCenter = GetComponent<MeshRenderer>().bounds.center;
        transform.position -= modelCenter;
    }

  
}
