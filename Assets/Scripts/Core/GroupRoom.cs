using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Participant
{
    public string id;
    public string name;
    public string gender;

    public Participant(string id, string name, string gender)
    {
        this.id = id;
        this.name = name;
        this.gender = gender;
    }
}



public class GroupRoom: Singleton<GroupRoom>
{
    [Header("GroupRoomUI")] 
    [SerializeField] private GameObject UIGO;
    [SerializeField] private GameObject groupRoomUIGO;

    [Header("WaitingUI")] 
    [SerializeField] private GameObject waitingUIGO; //exit
    [SerializeField] private TMP_Text WaitingPlayerCountText; // 대기인원 0/4
    
    [SerializeField] private Button exitButton;

    
    [Header("Data")]
    [SerializeField] private List<Participant> _participants;
    [SerializeField] private Participant _voted;



    public const int CountDown = 3;

    private void Awake()
    {
        UIGO.SetActive(false);
        exitButton.onClick.AddListener(ExitRoom);
    }

    public async void Enter(int maxPlayerCount)
    {
        UIGO.SetActive(true);
        await MonitorPlayerCountAsync(maxPlayerCount);
        Process();
    }
    

    private void ExitRoom()
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


    
    private async void Process()
    {
        Debug.Log("Process");

        // VoiceController.Connect();  // 보이스챗 연결


        // voiceController.Disconnect(); // 보이스챗 연결 종료 
    }



    private void SetParticipantList()
    {
        Debug.Log("Set Participant List");
        _participants = new List<Participant>();
        
        // Participant List 생성
        List<SharedData> sharedDatas = RunnerController.SharedDatas;
        foreach (var sh in sharedDatas)
        {
            Participant p = new Participant(sh.UserId, sh.UserName, sh.Gender);
            _participants.Add(p);
        }

        Debug.Log("------------------참여자 목록 ----------------------");

        foreach (var participant in _participants)
        {
            Debug.Log($"id: {participant.id}, name: {participant.name}, gender: {participant.gender}");
        }
    }

}
