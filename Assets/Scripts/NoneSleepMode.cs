using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneSleepMode : MonoBehaviour
{
    void Start()
    {
        // 앱 실행 중에는 절전 모드로 전환되지 않도록 설정한다.
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
