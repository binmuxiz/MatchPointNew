using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GroupRoom: MonoBehaviour
{
    public int maxPlayers;

    
    public bool enableAIBot;
    public bool enabledVote = false;
    public const int CountDown = 3;

    public string votedName = null;
    
    [Header("GroupRoomUI")] 
    [SerializeField] private GameObject groupRoomUIGO;
    [SerializeField] private GameObject AI;
    [SerializeField] private GameObject TextBox1;
    [SerializeField] private GameObject TextBox2;
    [SerializeField] private GameObject TextBox3;

    [SerializeField] private GameObject voteDoneUI;
    [SerializeField] private GameObject voteTotalUI;
    [SerializeField] private GameObject voteResultUI;
    [SerializeField] private TMP_Text resultText;

    [SerializeField] private TMP_Text timerText;

    [Header("WaitingUI")] 
    [SerializeField] private GameObject waitingUIGO; //exit
    [SerializeField] private TMP_Text WaitingPlayerCountText; // 대기인원 0/4
    

    private void Awake()
    {
        groupRoomUIGO.SetActive(false);
        waitingUIGO.SetActive(false);
    }

    private void Update()
    {
        if (!enableAIBot) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject go = hit.collider.gameObject;
            
                if (go.CompareTag("AIBot"))
                {
                    Debug.Log("AIBot clicked");
                    AI.SetActive(true);
                    TextBox1.SetActive(true);
                }
                else if (enabledVote && go.CompareTag("Player"))
                {
                    Debug.Log("플레이어 클릭해서 투표");
                    enabledVote = false;
                    SharedData sh = go.gameObject.GetComponent<SharedData>();
                    
                    string votedId = sh.UserId;
                    votedName = sh.UserName;
                    
                    SharedData.Instance.RpcVote(votedId);
                }
            }
        }

        if (SharedData.VoteDoneTrigger)
        {
            VoteTotalProcess();
        }
    }


    private void VoteTotalProcess() // 투표 집계
    {
        // UI 띄우고
        voteTotalUI.SetActive(true);
        
        // 집계 
        Dictionary<string, string> dict = SharedData.LoveDict;
        string myId = PlayerData.Instance.UserId;
        
        string selectedId = dict[myId]; // 에러?? 아님 null?? 
        if (selectedId != null)
        {
            string tempId = dict[selectedId];
            if (tempId != null && tempId == myId) //이어짐
            {
                resultText.text = $"당신이 선택한 {votedName}님도 당신을 선택했어요.\n5초 뒤 1:1 미팅룸으로 이동할게요.";
                GoToDoubleRoom(myId, selectedId);
            }
            else
            {
                resultText.text = $"당신이 선택한 {votedName}님이 당신을 선택하지 않았어요.\n5초 뒤 월드로 돌아갈게요.";
                GoToWorld();
            }
        }
        else
        {
            resultText.text = "당신은 아무도 선택하지 않았네요. 다음에 더 좋은 인연을 만날 수 있을 거예요.\n5초 뒤 월드로 돌아갈게요.";
            GoToWorld();
        }
    }

    private void GoToDoubleRoom(string myId, string selectedId)
    {
        for (int i = 10; i > 0; i--)
        {
            timerText.text = "{i}";
        }
        GameManager.Instance.EnterDoubleRoom(myId, selectedId);
    }
    
    private void GoToWorld()
    {
        for (int i = 10; i > 0; i--)
        {
            timerText.text = "{i}";
        }
        GameManager.Instance.EnterWorld();
    }
    

    public void ShowTextBox2UI()
    {
        TextBox1.SetActive(false);
        TextBox2.SetActive(true);
    }

    public void ShowTextBox3UI()
    {
        TextBox1.SetActive(false);
        TextBox3.SetActive(true);
    }

    public void HideTextBox3UI()
    {
        TextBox3.SetActive(false);
    }

    public void AllowBalanceGameProcess() // 밸런스 게임 시작하기 
    {
        SharedData.Instance.RpcSetBalanceGameTrigger(true);
    }
    
    public void EnableVoteProcess() //TODO 사랑의 작대기 하는 대사 클릭 시 -> 버튼에 연결 해주기
    {
        enabledVote = true;
        TextBox2.SetActive(false);
        AI.SetActive(false);
    }
    

    public async void Enter()
    {
        waitingUIGO.SetActive(true);
        await MonitorPlayerCountAsync(maxPlayers);
        waitingUIGO.SetActive(false);
        
        Process();
    }
    
    
    private void Process()
    {
        Debug.Log("Process");

        enableAIBot = true;
        groupRoomUIGO.SetActive(true);

        if (RunnerController.Runner.IsSharedModeMasterClient) //방장
        {
            voteDoneUI.SetActive(true);
        }
    }

    public void OnClickedVoteDoneButton()
    {
        SharedData.Instance.RpcSetVoteDoneTrigger(true);
    }

    public void ExitRoom()
    {
        GameManager.Instance.EnterWorld();
    }
    

    private async UniTask MonitorPlayerCountAsync(int maxPlayerCount)
    {
        Debug.Log(maxPlayerCount);
        int totalCount = maxPlayerCount;

        while (true)
        {
            var currentCount = RunnerController.Runner.SessionInfo.PlayerCount;
            WaitingPlayerCountText.text = $"대기인원 {currentCount}/{totalCount}";

            // 만약 플레이어가 totalCount만큼 모이면 10초 카운트 시작
            if (currentCount == totalCount)
            {
                WaitingPlayerCountText.text = $"대기인원 {currentCount}/{totalCount}";
                bool success = await StartCountdownAsync(totalCount, CountDown);
                if (success) return;
            }
            await UniTask.Delay(500);
        }
    }

    private async UniTask<bool> StartCountdownAsync(int totalCount, int countdownSeconds)
    {
        for (int i = countdownSeconds; i > 0; i--)
        {
            // 현재 세션의 플레이어 수 확인
            var currentCount = RunnerController.Runner.SessionInfo.PlayerCount;

            // 플레이어 수가 줄어들면 카운트 실패
            if (currentCount < totalCount) return false;

            WaitingPlayerCountText.text = $"{i}";
            await UniTask.Delay(1000); // 1초 대기
        }
        WaitingPlayerCountText.text = $"0";
        
        // 10초 동안 인원이 유지되면 성공
        return true;
    }




    
    
    
    
    
    
    
    
    
    
    
    
    // private void SetParticipantList()
    // {
    //     Debug.Log("Set Participant List");
    //     _participants = new List<Participant>();
    //     
    //     // Participant List 생성
    //     List<SharedData> sharedDatas = RunnerController.SharedDatas;
    //     foreach (var sh in sharedDatas)
    //     {
    //         Participant p = new Participant(sh.UserId, sh.UserName, sh.Gender);
    //         _participants.Add(p);
    //     }
    //
    //     Debug.Log("------------------참여자 목록 ----------------------");
    //
    //     foreach (var participant in _participants)
    //     {
    //         Debug.Log($"id: {participant.id}, name: {participant.name}, gender: {participant.gender}");
    //     }
    // }
}
