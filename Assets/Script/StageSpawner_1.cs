using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner_1 : MessageBroker
{
    [SerializeField] private Actor transmitter;
    //[SerializeField] private Actor propeller;
    //[SerializeField] private Actor lamp;
    [SerializeField] private Actor corpse_dead;
    [SerializeField] private Actor corpse_drown;
    [SerializeField] private Actor picture1;
    [SerializeField] private Actor picture2;
    [SerializeField] private Actor battery;
    [SerializeField] private Actor zone1;
    //[SerializeField] private Actor zone2;
    //[SerializeField] private Actor zone3;
    //[SerializeField] private Actor zone4;
    [SerializeField] private Actor sharkspawner;
    [SerializeField] private Actor bahamut;
    [SerializeField] private Actor reef;
    [SerializeField] private Actor gun;
    [SerializeField] private Actor bullet;
    [SerializeField] private Actor thunder; //추가
    [SerializeField] private Actor dagon; //추가


    void Summon(Actor actor){
        Debug.Log(actor.GetType().Name+"_sp");
        Transform summonPosition = SummonPoint.transform.Find(actor.GetType().Name+"_sp");
        Debug.Log(summonPosition.position);
        Instantiate(actor, summonPosition.position, summonPosition.rotation);
    }
    protected override void ChangeActorState(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.GameStart:
                break;
            case Stage.Opening_Start:
                // Summon(reef);
                
                // 생성은 여기서 처리하지만, 파괴는 각 인스턴스가 스스로를 파괴한다. 
                // 보트는 유일하게 처음부터 있어도 되지 않나?
                // 
                break;
            case Stage.Opening_Corpse1:
                // Summon(transmitter);
                Summon(picture1);
                Summon(corpse_dead);
                break;
            case Stage.Opening_Corpse2:
                Summon(picture2);
                Summon(corpse_drown);
                break;
            case Stage.Opening_FindBattery:
                Summon(battery);
                break;
            case Stage.Stage1_ConnectSatellite:
                Summon(zone1);
                break;
            case Stage.Stage1_EnterZone1:
                Summon(sharkspawner);
                break;
            case Stage.Stage1_EnterZone2:
                break;
            case Stage.Stage1_EnterZone3:
                break;
            case Stage.Stage1_SharkDissapear:
                break;
            case Stage.Stage1_BahamutAppear:
                Summon(reef);
                Summon(bahamut);                
                break;
            case Stage.Stage2_StormStart:
                Summon(thunder);
                break;
            case Stage.Stage2_DagonFind:
                Summon(dagon);
                break;
            default:
                Debug.LogWarning("Incorrect Stage");
                break;
        }
    }
}
