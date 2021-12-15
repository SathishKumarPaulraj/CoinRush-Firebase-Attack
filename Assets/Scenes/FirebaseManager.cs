using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;
using System;

public class FirebaseManager : MonoBehaviour
{
    DatabaseReference reference;
    Player playerDetails = new Player();
   // public InputField _userName;
    //public InputField _getPlayerDetails;

   // FirebaseAuth auth;
    GuestLogin mGuestLogin;
   // public Text  _playerIDDataText, _coinDataText, _energyDataText;
    private string mPlayerNameData, mPlayerIDData, mCoinData, mEnergyData, mPlayerCurrentLevelData;
    private GameManager mGameManager;

    private void Awake()
    {
        mGameManager = FindObjectOfType<GameManager>();
        mGuestLogin = FindObjectOfType<GuestLogin>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;  
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        if (mGuestLogin.fetchDetails == true)
        {
            ReadDataForGuest();
        }
    }
    void ReadDataForGuest()
    {
        reference.Child("Guest Users").Child(mGuestLogin.auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task => 
        {
            if(task.IsCompleted)
            {  
                DataSnapshot snapshot = task.Result;
                mPlayerNameData = snapshot.Child("_playerName").Value.ToString();
                mPlayerIDData = snapshot.Child("_playerID").Value.ToString();
                mPlayerCurrentLevelData = snapshot.Child("_playerCurrentLevel").Value.ToString();
                mCoinData = snapshot.Child("_coins").Value.ToString();
                mEnergyData = snapshot.Child("_energy").Value.ToString();

                mGameManager._coins = int.Parse(mCoinData);
                mGameManager._energy = int.Parse(mEnergyData);
                mGameManager._playerCurrentLevel = int.Parse(mPlayerCurrentLevelData);
            }
        });
     }

    void WritedataForGuest()
    {
        playerDetails._coins = mGameManager._coins;
        playerDetails._energy = mGameManager._energy;
        playerDetails._playerCurrentLevel = mGameManager._playerCurrentLevel;
        string json = JsonUtility.ToJson(playerDetails);
        reference.Child("Guest Users").Child(mGuestLogin.auth.CurrentUser.UserId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Write Successful");
            }
        });
    }

    void Update()
    {
       Invoke("WriteDataToDatabase", 2f);
    }
 
    void WriteDataToDatabase()
    {
        WritedataForGuest();
    }
}
