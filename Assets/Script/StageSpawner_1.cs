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
                break;
            case Stage.Opening_Corpse1:
            // 그냥 스테이지 시작 전부터 상자, 시체를 비롯해서 다양한 잔해 등 많은 부유물들이 떠 있는 상태.
            // 부유물과 보트가 부딪히는 순간 물리 작용 구현 필요
            // 좌초된 배 근처에 상자 위치      
            // 상자 건드리고 나면 배는 가라앉기  
                // Summon(transmitter);
                Summon(picture1);
                Summon(corpse_dead);
                break;
            case Stage.Opening_Corpse2:
                Summon(picture2);
                Summon(corpse_drown);
                break;
            case Stage.Opening_FindBattery:
            // 상자 안에 배터리 포함(부모자식 관계), 상자가 플레이어에게 닿으면 해제
                Summon(battery);
                break;
            case Stage.Stage1_ConnectSatellite:
            // 무전기 작동 방식 변경. 무전기가 가리키는 방향에 따라 GUI 변경.(거리 상관없이)
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
            // 바하무트가 보트를 쳐서 가라앉힐 때 배 뒤집히는 물리 작용 필요.
            // 빠지는 순간 사운드 효과 & 비눗방울 파티클
            
            // 바하무트가 플레이어 
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
