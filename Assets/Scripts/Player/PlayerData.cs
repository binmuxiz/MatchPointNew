using System;
using Cysharp.Threading.Tasks;
using Data;
using Network;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerData: MonoBehaviour
{
    public static PlayerData Instance;
    
    public string UserId;
    public Profile Profile;
    

    private NetworkController _networkController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _networkController = GameManager.NetworkController;
    }


    public async UniTask GetPlayerData(string userId)
    {
        var response = await _networkController.GetProfile(userId);
    
        if (response.Code == 200)
        {
            Profile profile = JsonConvert.DeserializeObject<Profile>(response.Body);
            UserId = userId;
            Profile = profile;
        }
        else
        {
            Debug.LogWarning("해당 id 정보가 없습니다.");
        }
    }

    public void ClearPlayerData()
    {
        UserId = null;
        Profile = null;
    }
}

