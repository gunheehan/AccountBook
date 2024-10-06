import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.Service;
import android.content.Intent;
import android.os.Build;
import android.os.IBinder;
import android.util.Log;

public class SmsProcessingService extends Service {

    @Override
    public void onCreate() {
        super.onCreate();

        // Foreground Service를 위한 Notification 생성
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel channel = new NotificationChannel(
                    "SmsProcessingChannel",
                    "SMS Processing Channel",
                    NotificationManager.IMPORTANCE_DEFAULT);
            NotificationManager manager = getSystemService(NotificationManager.class);
            manager.createNotificationChannel(channel);

            Notification notification = new Notification.Builder(this, "SmsProcessingChannel")
                    .setContentTitle("Processing SMS")
                    .setContentText("Processing incoming messages...")
                    .setSmallIcon(android.R.drawable.ic_dialog_info) // 아이콘 추가
                    .build();

            startForeground(1, notification);
        }
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        String smsData = intent.getStringExtra("sms_data");
        if (smsData != null) {
            Log.d("SmsProcessingService", "Received SMS data: " + smsData);
            // 문자 메시지 처리 로직 추가
        }
        return START_NOT_STICKY;
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }
}
