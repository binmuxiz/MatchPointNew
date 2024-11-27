using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using Network;
using Newtonsoft.Json;
using TMPro;
using UI;
using UnityEngine;

public class GroupRoom: Singleton<GroupRoom>
{
    public Canvas canvas;

    public RoomInfo roomInfo;
    public int maxPlayers;

    private const int CountDown = 2;

    private bool initalized = false;
    
    public List<SimpleProfile> PlayerProfiles;
    public GameObject meetingPanel;
    
    public string clickedUserId;
    public SimpleProfile clickedUserInfo;


    public string votedUserId = null;

    
    [Header("Top")]
    [SerializeField] private TMP_Text roomNameText;

    [Header("Loading")] 
    public LoadingCanvas LoadingCanvas;
    
    
    [Header("Waiting")] 
    public GameObject panelWaitingPrefab;
    public WaitingPanel waitingPanel;

    [Header("PlayersInfo")] 
    public Transform playerInfoTransform;
    public GameObject playerInfoPrefab;



    private void Awake()
    {
        if (panelWaitingPrefab == null)
        {
            Debug.LogError("The panel waiting prefab is null");
        }
    }
    
    private void Start()
    {
        canvas.enabled = false;
    }

    
    public async void Enter(RoomInfo roomInfo)
    {
        initalized = false;
        meetingPanel.SetActive(false);
        canvas.enabled = true;
        this.roomInfo = roomInfo;
        roomNameText.text = roomInfo.roomName;
        this.maxPlayers = roomInfo.maxPlayerCount;
        
        LoadingCanvas.Loading(2000, "미팅룸 접속중...");
        
        Debug.Log("---------------시작 대기중-------------------");
        this.waitingPanel = Instantiate(panelWaitingPrefab, canvas.transform, false).GetComponent<WaitingPanel>();
        await MonitorPlayerCountAsync(maxPlayers);
        Destroy(waitingPanel.gameObject);
        Debug.Log("---------------시작------------------------");
        
        InitializePlayerProfiles().Forget();
        
        LoadingCanvas.Loading(2000, "미팅 준비중...");

        await UniTask.WaitUntil(() => initalized);
        
        // 보이스챗 켜기
        
        // await UniTask.Delay(TimeSpan.FromSeconds(CountDown));

        
        // await GoToDoubleRoom();
        // GoToWorld();

    }

    private async UniTask InitializePlayerProfiles()
    {
        PlayerProfiles = new List<SimpleProfile>();
        List<SharedData> sharedDataList = RunnerController.SharedDatas;

        string myId = PlayerData.Instance.UserId;
        NetworkController networkController = GameManager.NetworkController;

        foreach (var sh in sharedDataList)
        {
            Response response = await networkController.GetSimpleProfile(myId, sh.UserId);
            if (response.Code == 200)
            {
                string data = response.Body;
                var simpleProfile = JsonConvert.DeserializeObject<SimpleProfile>(data);
                Debug.Log(simpleProfile);
                
                GameObject go = Instantiate(playerInfoPrefab, playerInfoTransform, false);
                PlayerInfoItem infoItem = go.GetComponent<PlayerInfoItem>();
                infoItem.SetPlayerInfoItem(sh.UserId, simpleProfile);
            }
        }
        
        meetingPanel.SetActive(true);

        initalized = true;
    }

    
    
    // 종료 버튼 눌렀을때
    public void OnClickedVoteDoneButton()
    {
        Debug.Log("OnClickedVoteDoneButton");
        VoteDoneProcess();
    }
    
    private async void VoteDoneProcess()
    {
        Dictionary<string, string> dict = SharedData.LoveDict;

        string myId = PlayerData.Instance.UserId;

        string mySelection = null;
        string yourSelection = null;
        
        if (dict.ContainsKey(myId))
        {
            mySelection = dict[myId];
        }

        if (mySelection != null && dict.ContainsKey(mySelection))
        {
            yourSelection = dict[mySelection];
        } 
        

        if (mySelection == null)
        {
            Debug.Log("선택한 사람 없음 ");
        }
        else if (yourSelection == null)
        {
            Debug.Log("상대방이 선택한 사람 없음 ");
        }
        else
        {
            if (myId == yourSelection)
            {
                Debug.Log("서로 선택함");
                GameManager.Instance.EnterDoubleRoom(myId, mySelection);
                meetingPanel.SetActive(false);
                DestroyAllChildren(playerInfoTransform);
                return;
            }
            else
            {
                Debug.Log("서로 선택안됨");
            }
        }
        Debug.Log("월드로 돌아감 ");
        GameManager.Instance.EnterWorld();
        meetingPanel.SetActive(false);
        DestroyAllChildren(playerInfoTransform);
    }
    
    
    
    private async UniTask GoToDoubleRoom()
    {
        await UniTask.Delay(4000);
        canvas.enabled = false;

        GameManager.Instance.EnterDoubleRoom("1", "2");
    }

    private void GoToWorld()
    {
        canvas.enabled = false;
        GameManager.Instance.EnterWorld();
    }
    
    private async UniTask MonitorPlayerCountAsync(int maxPlayerCount)
    {
        waitingPanel.ActiveCounterMembers();
        
        int totalCount = maxPlayerCount;
        waitingPanel.SetMaxPlayerCnt(totalCount);
    
        while (true)
        {
            var currentCount = RunnerController.Runner.SessionInfo.PlayerCount;
            waitingPanel.SetCurrentPlayerCnt(currentCount);
            
            // 만약 플레이어가 totalCount만큼 모이면 10초 카운트 시작
            if (currentCount == totalCount)
            {
                waitingPanel.SetCurrentPlayerCnt(currentCount);
                bool success = await StartCountdownAsync(totalCount, CountDown);
                if (success) return;
            }
            await UniTask.Delay(500);
        }
    }
    
    
    private async UniTask<bool> StartCountdownAsync(int totalCount, int countdownSeconds)
    {
        waitingPanel.ActiveTimer();
        for (int i = countdownSeconds; i > 0; i--)
        {
            // 현재 세션의 플레이어 수 확인
            var currentCount = RunnerController.Runner.SessionInfo.PlayerCount;
    
            // 플레이어 수가 줄어들면 카운트 실패
            if (currentCount < totalCount)
            {
                waitingPanel.ActiveCounterMembers();
                return false;
            }
    
            waitingPanel.SetCounterCnt(i);
            await UniTask.Delay(1000); // 1초 대기
        }
        waitingPanel.SetCounterCnt(0);
        
        // 10초 동안 인원이 유지되면 성공
        return true;
    }
    
    
    
    
    void DestroyAllChildren(Transform parent)
    {
        // 부모 오브젝트의 Transform을 기준으로 모든 자식 검색
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject); // 자식 오브젝트 삭제
        }
    }

}
