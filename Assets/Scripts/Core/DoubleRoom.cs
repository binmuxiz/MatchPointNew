using System;
using Cysharp.Threading.Tasks;
using Data;
using UI;
using UnityEngine;

public class DoubleRoom: MonoBehaviour
{
    /*public static string SessionId;
    
    [SerializeField] private DoubleRoomUI _doubleRoomUI;
    [SerializeField] private WorldUI _worldUI;

    private const int ConversationTime = 90;
    private const int QuitUITime = 10;

    public bool Enabled = false;


    [SerializeField] private VoiceController _voiceController;
    
    // 타임스탬프 기록 
    private long timestamp;

    public VoiceData VoiceData;

    private void ClearDoubleRoom()
    {
        SessionId = null;
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("GetKeyDown LeftControl");
            _doubleRoomUI.ShowResult();
        }
        
        // if (!Enabled) return;
        
        // 스페이스바 누를때마다 음성데이터 전송하도록
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SendAudioDataToServer();
        // }
    }
    
    

    public async void Enter(string myId, string otherId)
    {
        Enabled = true;
        
        string[] arr = { myId, otherId };
        Array.Sort(arr);
        SessionId = $"{arr[0]}_{arr[1]}";
        
        try
        {
            // await SessionManager.Instance.EnterDoubleRoom(SessionId);
            
            _voiceController = RunnerController.Runner.GetComponentInChildren<VoiceController>();
            
            await Process(myId, otherId);

            Clear();

            Enabled = false;
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }
    
                    
    // 서버로 오디오 데이터 전송
    private async void SendAudioDataToServer()
    {
        Debug.Log("Sending audio data to server");

        var audioData = _voiceController.GetBufferedAudioData();
        // 타임스탬프 초기화
        this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        string sessionId = SessionId;
        string userId = SharedData.Instance.UserId;
        string timestamp = this.timestamp.ToString();


        VoiceData voiceData = new VoiceData(
            sessionId,  // sessionId 변수의 값을 전달
            userId,  // userId 변수의 값을 전달
            timestamp,  // timestamp 변수의 값을 전달
            audioData,  // audioData 변수의 값을 전달
            sessionId + "_" + timestamp  // fileName 인수로 전달될 값
        );

        this.VoiceData = voiceData;
        
        Debug.Log("오디오 데이터 ");
        
        // 서버로 전송
        await NetworkManager.Instance.MeetingController.PostVoiceChatData(voiceData);

        Debug.Log("오디오 데이터 전송 완료");
    }


    
    
    
    private async UniTask Process(string myId, string otherId)
    {
        Debug.Log("Double Room Process 시작");

        Fader.FadeIn(_doubleRoomUI.doubleRoomCanvasGroup);
        Fader.FadeOut(_worldUI.worldMainCanvasGroup);
        /*
         * 자유롭게 대화를 나누다가,
         * 한문장 말할때마다 버튼 눌러서 음성 데이터 전송해놓고
         * 주제 추천 버튼 누르면 주제 받고,
         * 대화 시간 다 끝나면 종료 
         #1#
        
        // 대화 시간 
        // this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await VoiceChat.Connect(ConversationTime);
        
        Debug.Log("더블룸 대화 시간 종료 ");
        
        // _doubleRoomUI.ShowResult();
        
        // Fader.FadeOut(_doubleRoomUI.doubleRoomCanvasGroup);
        // Fader.FadeIn(_worldUI.worldMainCanvasGroup);
        
        // GameManager.Instance.SetStateWorld();
        
        // // 더블룸 종료 UI
        // await _doubleRoomUI.ShowAndHideQuitUI(QuitUITime);
        //
        // SessionManager.Instance.EnterWorld();
    }
    
    private void Clear()
    {
        SessionId = null;
        _voiceController = null;
    }
    
    
    public static async UniTask Connect(int voiceChatTime)
    {            
        SharedData.Instance.RpcSetTrigger(false);

        var voiceController = RunnerController.Runner.GetComponentInChildren<VoiceController>();
        
        voiceController.Connect();  // 보이스챗 연결
        
        if (RunnerController.Runner.IsSharedModeMasterClient)
        {
            Debug.Log("Is Master Client");
            
            await UniTask.Delay(TimeSpan.FromSeconds(voiceChatTime)); 
            
            SharedData.Instance.RpcSetTrigger(true);
        }
        else
        {
            await UniTask.WaitUntil(() => SharedData.Trigger);
        }
        
        voiceController.Disconnect(); // 보이스챗 연결 끊기 

        Debug.Log("==== 대화 프로세스 종료 ====");
    }*/
}
