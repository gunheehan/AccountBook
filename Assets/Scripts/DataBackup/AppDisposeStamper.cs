using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDisposeStamper : MonoBehaviour
{
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveLastExitTime();
    }

    private void OnApplicationQuit()
    {
        SaveLastExitTime();
    }

    private void SaveLastExitTime()
    {
        // 마지막 종료 시간을 저장 (UTC로 저장)
        //string lastExitTime = System.DateTime.UtcNow.ToString("o"); // ISO 8601 형식
        string lastExitTime = System.DateTime.UtcNow.AddDays(-5).ToString("o"); // 일주일 전 시간, ISO 8601 형식
        PlayerPrefs.SetString("LastExitTime", lastExitTime);
        PlayerPrefs.Save();
    }
}
