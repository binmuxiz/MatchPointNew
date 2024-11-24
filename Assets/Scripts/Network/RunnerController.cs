using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class RunnerController : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner Runner;
    [SerializeField] private GameObject playerPrefab;

    public static List<SharedData> SharedDatas;

    private void Awake()
    {
        Debug.Log("RunnerController.Awake()");
        Runner = GetComponent<NetworkRunner>();
        SharedDatas = new List<SharedData>();

        Runner.AddCallbacks(this); // 콜백 등록
    }

    public static void AddSharedData(SharedData sh)
    {
        SharedDatas.Add(sh);
    }

    public static void RemoveSharedData(SharedData sh)
    {
        SharedDatas.Remove(sh);
    }
    
    private async void WaitUntilSpawn(NetworkSpawnOp networkSpawnOp)
    {
        await UniTask.WaitUntil(() => networkSpawnOp.Status == NetworkSpawnStatus.Spawned);
        SessionManager.SpawnDone = true;
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("PlayerJoined");
        if (player == Runner.LocalPlayer)
        {
            Debug.Log("local player");
            
            Vector3 spawnPosition = Vector3.zero;
            
            switch (GameManager.Instance.GameState)
            {
                case GameState.World:
                    spawnPosition = World.Instance.WorldSpawnPosition.position;
                    break;
                case GameState.Group:
                    spawnPosition = GameManager.Instance.groupRoomSpwanPosition.position;
                    break;
                case GameState.Double:
                    spawnPosition = Vector3.zero;
                    break;
                default:
                    Debug.LogError("Invalid game state");
                    break;
            }
            var networkSpawnOp = Runner.SpawnAsync(playerPrefab, spawnPosition, Quaternion.identity, player);
            
            WaitUntilSpawn(networkSpawnOp);
            
            string id = PlayerData.Instance.UserId;
            string name = PlayerData.Instance.Profile.user.name;
            string gender = PlayerData.Instance.Profile.user.gender;
            SharedData.Instance.SetUser(id, name, gender);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("PlayerLeft");
        // 플레이어가 세션을 떠날 때 오브젝트 제거
        var playerObject = Runner.GetPlayerObject(player);
        if (playerObject != null)
        {
            Runner.Despawn(playerObject);
            Debug.Log($"플레이어 {player.PlayerId}의 오브젝트가 제거되었습니다.");
        }
        else
        {
            Debug.LogWarning($"플레이어 {player.PlayerId}의 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
