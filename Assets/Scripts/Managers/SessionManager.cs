using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public class SessionManager: Singleton<SessionManager>
{
    public static bool SpawnDone = false;
    private NetworkRunner _runner;
    

    public async UniTask<StartGameResult> StartSessionAsync(StartGameArgs args, GameObject runnerPrefab)
    {
        await LeaveSessionAsync();
        
        InitializeNetworkRunner(runnerPrefab);
        var startGameResult = await _runner.StartGame(args);
        
        Debug.Log("StartGameResult: " + startGameResult);
        Debug.Log(_runner.SessionInfo);

        await WaitUntilSpawn();

        return startGameResult;
    }

// 세션 접속 끊기 
    private async UniTask LeaveSessionAsync()
    {
        if (_runner != null)
        {
            Debug.Log("Leaving current session...");
            await _runner.Shutdown();
            _runner = null;
            Debug.Log("Session left successfully.");
        }
    }
    
// Runner 초기화
    private void InitializeNetworkRunner(GameObject runnerPrefab)
    {
        if (_runner == null)
        {
            var go = Instantiate(runnerPrefab);
            _runner = go.GetComponent<NetworkRunner>();
        } 
    }

    
    private async UniTask WaitUntilSpawn()
    {
        await UniTask.WaitUntil(() => SpawnDone);
        SpawnDone = false;
    }
}
