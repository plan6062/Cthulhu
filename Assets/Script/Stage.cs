public enum Stage
{
    None,
    GameStart,
    Opening_Start,
    Opening_Corpse1,
    Opening_Corpse2,
    Opening_FindBattery,
    Stage1_ConnectSatellite,
    Stage1_EnterZone1,
    Stage1_EnterZone2,
    Stage1_EnterZone3,
    Stage1_SharkDissapear,
    Stage1_BahamutAppear,
    Stage1_BahamutSwimAttack,
    Stage1_BahamutSwimAttack_Death,

    Stage1_GetClosetoReef,
    // 플레이어와 암초의 거리가 n 이하로 되었을 경우
    Stage1_LookThroughHole,
    Stage1_SwimStop,
    Stage1_FindBahamut,
    Stage1_EndBahamut, // 바하무트 들어감. 뗏목을 타고 암초 밖으로 나가면 다음으로 진행
    Stage1_TryConnectAgain,
    Stage1_TransmitSOS,
    Stage2_StormStart,
    Stage2_Picture,
    Stage2_LightDarken,
    Stage2_DagonFind,
    Stage2_DagonOceanClose1,
    Stage2_DagonOceanClose2,
    Stage2_DagonOnBoat,
    Stage2_DagonGetClose, // 작살 맞는건 따로 스테이지로 빼지 않고 다곤에서 처리
    Stage2_DagonDeathScene,
    Stage2_DagonEnd, // 작살 3번 맞으면 다곤 사망
}