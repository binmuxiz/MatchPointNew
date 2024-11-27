using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DoubleRoom: Singleton<DoubleRoom>
{
    public Canvas canvas;

    private void Start()
    {
    }

    public async void Enter()
    {
    }
    
    private async UniTask Process()
    {
    }
    
    
    
    
    // private async void SendAudioDataToServer()
    // {
    //     Debug.Log("Sending audio data to server");
    //
    //     var audioData = _voiceController.GetBufferedAudioData();
    //     // 타임스탬프 초기화
    //     this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    //     
    //     string sessionId = SessionId;
    //     string userId = SharedData.Instance.UserId;
    //     string timestamp = this.timestamp.ToString();
    //
    //
    //     VoiceData voiceData = new VoiceData(
    //         sessionId,  // sessionId 변수의 값을 전달
    //         userId,  // userId 변수의 값을 전달
    //         timestamp,  // timestamp 변수의 값을 전달
    //         audioData,  // audioData 변수의 값을 전달
    //         sessionId + "_" + timestamp  // fileName 인수로 전달될 값
    //     );
    //
    //     this.VoiceData = voiceData;
    //     
    //     Debug.Log("오디오 데이터 ");
    //     
    //     // 서버로 전송
    //     await NetworkManager.Instance.MeetingController.PostVoiceChatData(voiceData);
    //
    //     Debug.Log("오디오 데이터 전송 완료");
    // }
}
