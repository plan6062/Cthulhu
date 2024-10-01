using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BahamutEye : MonoBehaviour
{
    public Bahamut bahamut;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            bahamut.isBahamutHit = true;
        }
    }
}
