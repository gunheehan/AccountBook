using UnityEngine;
using System;
using UnityEngine.UI;

public class SMSReader : MonoBehaviour
{
    [SerializeField] private Text text;
    void Start()
    {
        GetMessagesSinceLastExit();
    }

    void GetMessagesSinceLastExit()
    {
        // 마지막 종료 시간을 가져옴
        string lastExitTimeStr = PlayerPrefs.GetString("LastExitTime", null);
        if (!string.IsNullOrEmpty(lastExitTimeStr))
        {
            DateTime lastExitTime = DateTime.Parse(lastExitTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind);

            // SharedPreferences에 저장된 SMS 데이터 가져오기
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject prefs = currentActivity.Call<AndroidJavaObject>("getSharedPreferences", "SmsData", 0);
            string allSmsData = prefs.Call<string>("getString", "allSms", ""); // 모든 메시지 데이터 가져오기
            text.text = allSmsData;
            // 수신된 SMS 목록 파싱 및 필터링
            string[] smsEntries = allSmsData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in smsEntries)
            {
                // 각 SMS 항목에서 시간을 추출하고, 필터링
                string[] parts = entry.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    string sender = parts[0].Replace("From: ", "");
                    string message = parts[1].Replace("Message: ", "");
                    long timestamp = long.Parse(parts[2].Replace("Time: ", ""));
                    DateTime messageTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;

                    // 마지막 종료 시간 이후의 메시지 필터링
                    if (messageTime > lastExitTime)
                    {
                        Debug.Log($"New SMS from {sender}: {message} at {messageTime}");
                    }
                }
            }
        }
    }
}