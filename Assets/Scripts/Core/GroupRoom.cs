using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using Network;
using Newtonsoft.Json;
using Ricimi;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GroupRoom : Singleton<GroupRoom>
{
    [Header("Canvas")] public Canvas canvas;
    public GameObject waitingCanvas;
    public GameObject meetingCanvas;

    [FormerlySerializedAs("loadingCanvas")] public LoadingUI loadingUI;

    public RoomInfo roomInfo;
    public int maxPlayers;

    private const int CountDown = 2;

    private bool initalized = false;

    public List<SimpleProfile> PlayerProfiles;

    public string clickedUserId;
    public SimpleProfile clickedUserInfo;

    public string votedUserId = null;

    [Header("Top")] [SerializeField] private TMP_Text roomNameText;

    [Header("Waiting")] public WaitingPanel waitingPanel;

    [Header("PlayersInfo")] public Transform playerInfoTransform;
    public GameObject playerInfoPrefab;



    [Header("Love Selection")]
    public Button voteDoneButton;    // 투표 마감 버튼
    public bool loveResult = false;
    public bool aggregationDone = false;
    public PopupOpener popupOpener;
    public GameObject loveSelectionResultCanvas;


    private void Start()
    {
        canvas.enabled = false;
        meetingCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        DestroyAllChildren(playerInfoTransform);
        canvas.enabled = false;
        meetingCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
    }

    public async void Enter(RoomInfo roomInfo)
    {
        canvas.enabled = true;

        initalized = false;
        this.roomInfo = roomInfo;
        roomNameText.text = roomInfo.roomName;
        this.maxPlayers = roomInfo.maxPlayerCount;

        await loadingUI.Loading(2000, "미팅룸 접속중...");

        Debug.Log("---------------시작 대기중-------------------");
        waitingCanvas.SetActive(true); // 대기 UI 켜기 
        await MonitorPlayerCountAsync(maxPlayers);
        waitingCanvas.SetActive(false); // 대기 UI 끄기 
        Debug.Log("---------------시작------------------------");

        InitializePlayerProfiles().Forget();

        // await LoadingCanvas.Loading(2000, "미팅 준비중...");
        
        await UniTask.WaitUntil(() => initalized);


        // await UniTask.WaitUntil(() => SharedData.Trigger);  // 투표 집계 완료 시 true
        

    }

    private async UniTask InitializePlayerProfiles()
    {
        PlayerProfiles = new List<SimpleProfile>();
        List<SharedData> sharedDataList = RunnerController.SharedDatas;

        string myId = PlayerData.Instance.UserId;
        NetworkController networkController = GameManager.NetworkController;
 
        foreach (var sh in sharedDataList)
        {
            // if (sh.UserId == myId) continue; // 내 프로필 빼고
            
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

        meetingCanvas.SetActive(true);

        initalized = true;
    }


    
    public void OnClickedVoteDoneButton()    // 종료 버튼 눌렀을때
    {
        // if (!RunnerController.Runner.IsSharedModeMasterClient) 
        // {
        //     return;
        // }
        SharedData.Instance.RpcVoteDone();
    }


    public async void AggregationProcess()    // 결과 집계 
    {
        voteDoneButton.interactable = false;  // 버튼 비활성화
        popupOpener.OpenPopup();        // 결과 집계 팝업
        
        loveResult = false;
        
        // if (RunnerController.Runner.IsSharedModeMasterClient)
        // {
        //     await UniTask.Delay(1500);
        // }
        
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
                loveResult = true;
            }
            else
            {
            }
        }

        aggregationDone = true;
    }


    public async void ShowLoveResult()
    {
        aggregationDone = false;

        if (loveResult)
        {
            Debug.Log("작대기 성공");
            loveSelectionResultCanvas.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(3));   // 3초 보여주고 
            loveSelectionResultCanvas.SetActive(false);
            GoToDoubleRoom(PlayerData.Instance.UserId, votedUserId);

        }
        else
        {
            Debug.Log("작대기 실패");
            loveSelectionResultCanvas.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            loveSelectionResultCanvas.SetActive(false);
            GoToWorld();
        }
    }
    

    private void GoToWorld()
    {
        canvas.enabled = false;
        GameManager.Instance.EnterWorld();
    }

    private void GoToDoubleRoom(string myId, string mySelection)
    {
        Debug.Log("1:1 미팅룸으로 이동");
        canvas.enabled = false;
        GameManager.Instance.EnterDoubleRoom(myId, mySelection);
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

    public void Exit()
    {
        GameManager.Instance.EnterWorld();
    }
}
