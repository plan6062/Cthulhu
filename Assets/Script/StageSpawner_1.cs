using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner_1 : MessageBroker
{
    [SerializeField] private Actor boat;
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
    [SerializeField] private Actor sharkspawner1;
    [SerializeField] private Actor sharkspawner2;
    [SerializeField] private Actor sharkspawner3;
    [SerializeField] private Actor bahamut;
    [SerializeField] private Actor reef;
    [SerializeField] private Actor gun;
    [SerializeField] private Actor bullet;
    [SerializeField] private Actor thunder; //추가

    protected override void ChangeActorState(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.GameStart:
                break;
            case Stage.Opening_Start:
                Instantiate(boat ,boat.SummonPosition.position ,boat.SummonPosition.rotation);
                Instantiate(reef ,reef.SummonPosition.position ,reef.SummonPosition.rotation);
                Instantiate(thunder ,thunder.SummonPosition.position ,thunder.SummonPosition.rotation);
                //Instantiate(transmitter ,transmitter.SummonPosition.position ,transmitter.SummonPosition.rotation);
                //Instantiate(propeller ,propeller.SummonPosition.position ,propeller.SummonPosition.rotation);
                //Instantiate(lamp ,lamp.SummonPosition.position ,lamp.SummonPosition.rotation);

                // 생성은 여기서 처리하지만, 파괴는 각 인스턴스가 스스로를 파괴한다. 
                // 보트는 유일하게 처음부터 있어도 되지 않나?
                // 
                break;
            case Stage.Opening_Picture1:
                //Instantiate(picture1, picture1.SummonPosition.position ,picture1.SummonPosition.rotation);
                break;
            case Stage.Opening_Picture2:
                //Instantiate(picture2, picture2.SummonPosition.position ,picture2.SummonPosition.rotation);
                break;
            case Stage.Opening_Corpse1:
                Instantiate(corpse_dead, corpse_dead.SummonPosition.position ,corpse_dead.SummonPosition.rotation);
                break;
            case Stage.Opening_Picture3:
                break;
            case Stage.Opening_Corpse2:
                Instantiate(corpse_drown, corpse_drown.SummonPosition.position ,corpse_drown.SummonPosition.rotation);
                break;
            case Stage.Opening_FindBattery:
                Instantiate(battery, battery.SummonPosition.position ,battery.SummonPosition.rotation);
                break;
            case Stage.Stage1_ConnectSatellite:
                Instantiate(zone1, zone1.SummonPosition.position ,zone1.SummonPosition.rotation);
                break;
            case Stage.Stage1_EnterZone1:
                //Instantiate(sharkspawner1, sharkspawner1.SummonPosition.position ,sharkspawner1.SummonPosition.rotation);
                break;
            case Stage.Stage1_EnterZone2:
                break;
            case Stage.Stage1_EnterZone3:
                break;
            case Stage.Stage1_SharkDissapear:
                break;
            case Stage.Stage1_BahamutAppear:
                break;
            case Stage.Stage1_BahamutSwimAttack:
                break;
            case Stage.Stage1_BahamutSwimAttack_Death:
                break;
            case Stage.Stage1_GetClosetoReef:
                break;
            case Stage.Stage1_LookThroughHole:
                break;
            default:
                Debug.LogWarning("Incorrect Stage");
                break;
        }
    }
}
