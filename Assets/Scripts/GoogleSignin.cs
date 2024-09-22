
using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;

public class GoogleSigninManager : MonoBehaviour
{
    private string webClientId = "945509069547-dukg9gdkmell4chsbdfsqeccogu8fhqs.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    private FirebaseAuth auth;
    private FirebaseUser user;

    public Text userNickname;
    public Text userEmail;
    public Button signInButton;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration()
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
    }

    private void Start()
    {
        signInButton.onClick.AddListener(OnClickGoogleSignIn);
    }

    private void OnClickGoogleSignIn()
    {
        auth = FirebaseAuth.DefaultInstance;
        
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    private void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        if(task.IsFaulted)
            Debug.Log("Fault");
        else if(task.IsCanceled)
            Debug.Log("Login Cancel");
        else
        {
            Firebase.Auth.Credential credential =
                Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWuthCredentialAsync encountered an error : " + task.Exception);
                    return;
                }

                user = auth.CurrentUser;

                userNickname.text = user.DisplayName;
                userEmail.text = user.Email;
            });
        }
    }

    IEnumerator LoadImage(string imageUri)
    {
        WWW www = new WWW(imageUri);
        yield return www;

        Sprite profile = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }
}
