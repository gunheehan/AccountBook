using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using UnityEngine;
using UnityEngine.UI;

public class GoogleSignin : MonoBehaviour
{
    [SerializeField] private Button Btn_Login;
    [SerializeField] private Text processText;
    
    static string[] Scopes = { DriveService.Scope.DriveFile, SheetsService.Scope.Spreadsheets };
    static string ApplicationName = "Google Sheets Integration Example";

    private void Start()
    {
        Btn_Login.onClick.AddListener(OnClickSignIn);
    }

    private void OnClickSignIn()
    {
        AuthenticateSheets();
    }

    public DriveService AuthenticateDrive()
    {
        processText.text = "Start Auth";
        UserCredential credential;
#if UNITY_ANDROID
        string filePath = Path.Combine(Application.streamingAssetsPath, "google-service.json");
        using (var www = new WWW(filePath)) {
            while (!www.isDone) {}
            filePath = Path.Combine(Application.persistentDataPath, "google-service.json");
            File.WriteAllBytes(filePath, www.bytes);
        }
        #else
                string filePath = Path.Combine(Application.streamingAssetsPath, "google-service.json");
#endif

        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        { 
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            processText.text = $"{credential.Token} : {credential.UserId}";
        }

        return new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public SheetsService AuthenticateSheets()
    {
        DriveService driveService = AuthenticateDrive();

        return new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = driveService.HttpClientInitializer,
            ApplicationName = ApplicationName,
        });
    }
}
