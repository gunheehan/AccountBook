import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.telephony.SmsMessage;
import android.util.Log;

public class SmsReceiver extends BroadcastReceiver {
    
    @Override
    public void onReceive(Context context, Intent intent) {
        if ("android.provider.Telephony.SMS_RECEIVED".equals(intent.getAction())) {
            Bundle bundle = intent.getExtras();
            StringBuilder str = new StringBuilder();

            if (bundle != null) {
                try {
                    Object[] pdus = (Object[]) bundle.get("pdus");
                    if (pdus != null) {
                        SmsMessage[] msgs = new SmsMessage[pdus.length];

                        for (int i = 0; i < msgs.length; i++) {
                            msgs[i] = SmsMessage.createFromPdu((byte[]) pdus[i]);
                            String sender = msgs[i].getOriginatingAddress();
                            String messageBody = msgs[i].getMessageBody();
                            long timestamp = msgs[i].getTimestampMillis();

                            // Log message details
                            str.append("From: ").append(sender)
                               .append(", Message: ").append(messageBody)
                               .append(", Time: ").append(timestamp)
                               .append("\n");
                        }

                        // Start foreground service
                        Intent serviceIntent = new Intent(context, SmsProcessingService.class);
                        serviceIntent.putExtra("sms_data", str.toString());
                        context.startForegroundService(serviceIntent);
                    }
                } catch (Exception e) {
                    Log.e("SmsReceiver", "Exception: " + e.getMessage());
                }
            }
        }
    }
}
