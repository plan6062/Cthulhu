using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : MonoBehaviour
{
    public _Bahamut bahamut;
    public ParticleSystem particleSystem; 
    public bool testDamage;

    private void Start() {
        particleSystem.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            bahamut.GetDamage();
            // particleSystem.Play();
        }
    }

    private void Update() {
        if(testDamage){
            testDamage = false;
            bahamut.GetDamage();
            // particleSystem.Play();
        }
    }
}
