public enum Stage
{
    GameStart,
    Opening_Start,
    Opening_Picture1,
    Opening_Picture2,
    Opening_Corpse1,
    Opening_Picture3,
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
    Stage1_StopBoat,
    Stage1_LookThroughHole,
    Stage1_SwimStop,
    Stage1_FindBahamut
    // 플레이어의 시야 벡터가 구멍을 향한 순간
}