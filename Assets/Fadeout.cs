using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeout : MonoBehaviour
{
    public GameObject blackoutQuad; // 검은색 Quad 게임 오브젝트

    // 화면 암전 시작
    public void StartBlackout()
    {
        if (blackoutQuad != null)
        {
            blackoutQuad.SetActive(true);
        }
    }

    // 화면 암전 해제
    public void EndBlackout()
    {
        if (blackoutQuad != null)
        {
            blackoutQuad.SetActive(false);
        }
    }
}
