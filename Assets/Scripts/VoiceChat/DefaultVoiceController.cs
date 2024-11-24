using Photon.Voice.Fusion;
using Photon.Voice.Unity;
using UnityEngine;

namespace VoiceChat
{
    public class DefaultVoiceController: MonoBehaviour
    {
        public Recorder recorder;
        public FusionVoiceClient voiceClient;

        private void Start()
        {
            var devices = UnityEngine.Microphone.devices;
            foreach (var device in devices)
            {
                Debug.Log($"사용 가능한 마이크: {device}");
            }
            
            if (voiceClient != null)
            {
                Debug.Log("Fusion Voice 클라이언트를 수동으로 연결합니다.");
                voiceClient.ConnectAndJoinRoom();
            }
            else
            {
                Debug.LogError("Fusion Voice 클라이언트를 찾을 수 없습니다.");
            }
        }

        
        
        
        public void Connect()
        {
            Debug.Log("VoiceController.Connect");
            if (recorder != null)
            {
                recorder.TransmitEnabled = true; // 녹음된 오디오를 전송할지 여부
                recorder.RecordingEnabled = true; // 오디오 입력 장치(예: 마이크)로부터 오디오 데이터를 캡처할지 여부
            }
            else
            {
                Debug.LogWarning("Recorder is null");
            }
        }

        public void Disconnect()
        {
            if (recorder != null)
            {
                recorder.TransmitEnabled = false;
                recorder.RecordingEnabled = false;
            }
            else
            {
                Debug.LogWarning("Recorder is null");
            }
        }
    }
}