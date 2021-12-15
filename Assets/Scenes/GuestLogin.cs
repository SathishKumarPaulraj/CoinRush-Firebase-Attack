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
    public FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser newUser;
    public FirebaseUser _SignInuser;
    public FirebaseUser _existingUser;
    bool disableLogin=false;
    public GameObject _LoginPanel;
    
    public Text _guestPlayerName;
    public GameObject LoginButton;
   // private string _getGuestName;

    bool CanSignIn = false;
    public bool fetchDetails = false;
   //Firebase Database References
   DatabaseReference reference;
    Player playerDetails = new Player();

    //SignIn Guest via FaceBook
   // public string _accessToken;
   // public string fbname;
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
            }
            _existingUser = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + _existingUser.UserId);
                Debug.Log("Now u can Upgrade the Guest Account");
                fetchDetails = true;
                CanSignIn =true;
               // _guestLoginPanel1.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Not Signed In bro");
        }
    }
    void Start()
    {
        //playerDetails._playerName = "Guest";
        //playerDetails._playerCurrentLevel = 1;
        //playerDetails._coins = 0;
        //playerDetails._energy = 0;

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
            SaveNewUser(newUser.UserId);
            disableLogin=true;
            //_getGuestName = playerDetails._playerName;            
            //Debug.Log("UserName : " + playerDetails._playerName);
            //Debug.Log("UserID : " + newUser.UserId);

            //playerDetails._playerID = newUser.UserId;
            //playerDetails._playerCurrentLevel = 1;
            //playerDetails._coins = 0;
            //playerDetails._energy = 0;

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

    //private void Update()
    //{
    //    _guestPlayerName.text = _getGuestName;
    //}
    private void Update()
    {
        if(disableLogin==true)
        {
            _LoginPanel.SetActive(false);
        }
    }
}