using System;
using Fusion;
using Network;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameState
{
    Login, World, Group, Double
}

public class GameManager: Singleton<GameManager>
{
    public GameObject BalanceGameSettingButton;
    public GameState GameState = GameState.Login;
    
    public static NetworkController NetworkController;
    private const string BaseURI = "https://eternal-leopard-hopelessly.ngrok-free.app";

    [Header("NetworkRunner")]
    public Transform worldSpawnPosition;
    public GameObject worldRunnerPrefab;
    
    public Transform groupRoomSpwanPosition;
    public GameObject groupRoomRunnerPrefab;
    
    public GameObject doubleRoomRunnerPrefab;

    [Header("UI")]
    public LoadingUI loadingUI;
    public GameObject loginCanvas;

    [Header("Game Setings")] 
    public GameObject FacingRoomGameObject;
    public GameObject WorldGameObejct;

    private void Awake()
    {
        NetworkController = new NetworkController(new HttpClient(BaseURI));
        
        WorldGameObejct.SetActive(true);
        FacingRoomGameObject.SetActive(false);
        loginCanvas.SetActive(true);
    }


    // 월드 접속 
    public async void EnterWorld()
    {
        // todo 월드 접속 UI
        
        if (!WorldGameObejct.activeSelf)
        {
            WorldGameObejct.SetActive(true);
        }
        if (FacingRoomGameObject.activeSelf)
        {
            FacingRoomGameObject.SetActive(false);
        }
        
        GameState = GameState.World;
        
        var args = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = "World",
            PlayerCount = 20,
        };

        // session 생성/접속 및 플레이어 스폰
        await SessionManager.Instance.StartSessionAsync(args, worldRunnerPrefab);
        
        World.Instance.Enter();
    }
    
    
    
// 그룹 미팅 룸 조인 
    public async void EnterGroupRoom(RoomInfo roomInfo)
    {
        if (!WorldGameObejct.activeSelf)
        {
            WorldGameObejct.SetActive(true);
        }
        if (FacingRoomGameObject.activeSelf)
        {
            FacingRoomGameObject.SetActive(false);
        }
        
        // await loadingCanvas.Show("접속중...");
        GameState = GameState.Group;
        
        // BalanceGameSettingButton.SetActive(false);
        
        var args = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = roomInfo.roomId,
            PlayerCount = roomInfo.maxPlayerCount,
        };

        await SessionManager.Instance.StartSessionAsync(args, groupRoomRunnerPrefab);

        // await loadingCanvas.Hide();

        GroupRoom.Instance.Enter(roomInfo);
    }
    
    
    

// 1:1 미팅 룸 조인        
    public async void EnterDoubleRoom(string myId, string otherId)
    {
        loadingUI.Loading(3000, "1:1 미팅룸 접속중");

        Debug.Log("Entering Double Room");
        if (WorldGameObejct.activeSelf)
        {
            WorldGameObejct.SetActive(false);
        }
        if (!FacingRoomGameObject.activeSelf)
        {
            FacingRoomGameObject.SetActive(true);
        }
        
        Debug.Log("GameManager.EnterDoubleRoom()");
        GameState = GameState.Double;

        string[] arr = { myId, otherId };
        Array.Sort(arr);
        string roomId = $"{arr[0]}_{arr[1]}";
        
        var args = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = roomId,
            PlayerCount = 2,
        };

        CameraController.Instance.SetFacingRoomCamera();
        
        await SessionManager.Instance.StartSessionAsync(args, doubleRoomRunnerPrefab);

        DoubleRoom.Instance.Enter();
    }
}
