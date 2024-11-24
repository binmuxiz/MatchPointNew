using Fusion;
using Network;
using UnityEngine;

public enum GameState
{
    Login, World, Group, Double
}

public class GameManager: Singleton<GameManager>
{
    public GameState GameState = GameState.Login;
    
    
    public static NetworkController NetworkController;
    private const string BaseURI = "https://eternal-leopard-hopelessly.ngrok-free.app";

    public Login Login;
    
    
    [Header("NetworkRunner")]
    public Transform groupRoomSpwanPosition;
    [SerializeField] private GameObject groupRoomRunnerPrefab;

    
    
    private void Awake()
    {
        NetworkController = new NetworkController(new HttpClient(BaseURI));
    }

    private void Start()
    {
        LoginProcess();
    }

    public void LoginProcess()
    {
        GameState = GameState.Login;

        CameraController.Instance.brain.enabled = false;
        Login.ShowLoginUI();
    }
    
    
// 월드 접속 
    public void EnterWorld()
    {
        GameState = GameState.World;
        World.Instance.Enter();
    }
    
    
    
// 그룹 미팅 룸 조인 
    public async void EnterGroupRoom(string roomId, int maxPlayerCount)
    {
        GameState = GameState.Group;
        
        var args = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = roomId,
            PlayerCount = maxPlayerCount,
        };

        await SessionManager.Instance.StartSessionAsync(args, groupRoomRunnerPrefab);
        
        GroupRoom.Instance.Enter(maxPlayerCount);
    }

// 1:1 미팅 룸 조인        
    public void EnterDoubleRoom(string myId, string otherId)
    {
        Debug.Log("Join DoubleRoom");
        // _doubleRoom.Enter(myId, otherId);
    }
}
