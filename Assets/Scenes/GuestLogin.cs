using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using UnityEngine.UI;
//using Facebook.Unity;
public class GuestLogin : MonoBehaviour
{
    FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser newUser;
    public FirebaseUser _SignInuser;
    public FirebaseUser _existingUser;
   // public GameObject _GuestLoginPanel;
    public GameObject _guestLoginPanel1;
    
    public Text _guestPlayerName;
    public GameObject LoginButton;
    private string _getGuestName;

    bool CanSignIn = false;

   //Firebase Database References
   DatabaseReference reference;
    Player playerDetails = new Player();

    //SignIn Guest via FaceBook
    public string _accessToken;
    public string fbname;
    void Awake()
    {
        InitializeFirebase();
    }
    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser.IsAnonymous != null)
        {
            bool signedIn = auth.CurrentUser != null;
            if (!signedIn)
            {
                Debug.Log("Signed out " + _existingUser.UserId);
               // _guestLoginPanel1.SetActive(false);
                //loginPanel.SetActive(true);
            }
            //_existingUser = auth.CurrentUser;
            _existingUser = null;
            if (signedIn)
            {
                Debug.Log("Signed in " + _existingUser.UserId);
                Debug.Log("Now u can Upgrade the Guest Account");
                CanSignIn =true;
                _guestLoginPanel1.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Not Signed In bro");
        }
    }
    void Start()
    {
        playerDetails._playerName = "Guest";
        playerDetails._playerCurrentLevel = 2;
        playerDetails._coins = 3;
        playerDetails._energy = 4;

    }
    public void OnClickGuestLogin()
    {
        LoginButton.SetActive(false);        
        auth = FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            newUser = task.Result;
            _getGuestName = playerDetails._playerName;
            playerDetails._playerID = newUser.UserId;
            Debug.Log("UserName : " + playerDetails._playerName);
            Debug.Log("UserID : " + newUser.UserId);
            Debug.Log("checked1");
            //  SaveNewUser(newUser.UserId);

            Debug.Log(_guestLoginPanel1.name);
            _guestLoginPanel1.SetActive(false);
            Debug.Log("checked2");
        });
    }
     void SaveNewUser(string userId)
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        string json = JsonUtility.ToJson(playerDetails);
        reference.Child("Guest Users").Child(currentUser.UserId).SetRawJsonValueAsync(json);
    }
   
   //public void ClickToLogin()
   // {
   //     if (/*newUser.IsAnonymous && */CanSignIn==true)
   //     {
   //         StartCoroutine(Guestupgrade(_accessToken));
   //     }
   // }
   // IEnumerator Guestupgrade(string inaccessToken)
   // {
   //     yield return null;      
   //     var credential = Firebase.Auth.FacebookAuthProvider.GetCredential(inaccessToken);
   //     auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task=> 
   //     {
   //         if (task.IsFaulted)
   //         {
   //             Debug.Log("signin encountered error" + task.Exception);
   //         }          
   //         _SignInuser = task.Result;
   //         Debug.Log("Disp name: " + fbname);

   //         //Database Details Update
   //         playerDetails._playerName = _SignInuser.DisplayName;
   //         playerDetails._playerID = _SignInuser.UserId;
   //         SaveGuestAsUser(_SignInuser.UserId);
   //     });     
   // } 
   // public void SaveGuestAsUser(string userId)
   // {
   //     var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
   //     string json = JsonUtility.ToJson(playerDetails);
   //     reference.Child("Facebook Users").Child(currentUser.UserId).SetRawJsonValueAsync(json);
   //     reference.Child("Guest Users").Child(newUser.UserId).RemoveValueAsync();
   // }

    private void Update()
    {
        _guestPlayerName.text = _getGuestName;
    }
}