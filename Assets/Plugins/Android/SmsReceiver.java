package com.yourcompany.yourapp;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.telephony.SmsMessage;
import android.util.Log;
import java.util.Date;

public class SmsReceiver extends BroadcastReceiver {
    
    @Override
    public void onReceive(Context context, Intent intent) {
        if (intent.getAction().equals("android.provider.Telephony.SMS_RECEIVED")) {
            Bundle bundle = intent.getExtras();
            SmsMessage[] msgs = null;
            String str = "";

            if (bundle != null) {
                try {
                    Object[] pdus = (Object[]) bundle.get("pdus");
                    msgs = new SmsMessage[pdus.length];
                    
                    for (int i = 0; i < msgs.length; i++) {
                        msgs[i] = SmsMessage.createFromPdu((byte[]) pdus[i]);
                        String sender = msgs[i].getOriginatingAddress();
                        String messageBody = msgs[i].getMessageBody();
                        long timestamp = msgs[i].getTimestampMillis(); // SMS 수신 시간

                        // 로그로 확인
                        str += "From: " + sender + ", Message: " + messageBody + ", Time: " + timestamp + "\n";
                    }

                    // Foreground Service 시작 (앱이 꺼져 있어도 동작 가능)
                    Intent serviceIntent = new Intent(context, SmsProcessingService.class);
                    serviceIntent.putExtra("sms_data", str);
                    context.startForegroundService(serviceIntent);

                } catch (Exception e) {
                    Log.e("SmsReceiver", "Exception: " + e.getMessage());
                }
            }
        }
    }
}
