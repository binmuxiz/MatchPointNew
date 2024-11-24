using Fusion;
using Photon.Voice.Fusion;
using Photon.Voice.Unity;
using UnityEngine;

namespace VoiceChat
{
    public class VoiceSetUp: NetworkBehaviour
    {
        public Recorder _recorder;
        public Speaker _speaker;
        private static FusionVoiceClient _voiceClient;

        
        public override void Spawned()
        {
            Debug.Log("VoiceSetUp spawned");
            
            _recorder = GetComponentInChildren<Recorder>(true);
            _speaker = GetComponentInChildren<Speaker>(true);

            if (_recorder == null || _speaker == null)
            {
                Debug.LogError("recorder, speaker가 초기화되지 않음 ");
            }
            

            if (HasStateAuthority) // 로컬 플레이어
            {
                Debug.Log("Local Player");
                _voiceClient = FindAnyObjectByType<FusionVoiceClient>();
                _voiceClient.PrimaryRecorder = _recorder;

                _recorder.gameObject.SetActive(true);
                _speaker.gameObject.SetActive(false);

                _voiceClient.Disconnect();
                // _recorder.enabled = true;
            }
            else                           // 원격 플레이어 
            {
                _speaker.gameObject.SetActive(true);
                _recorder.gameObject.SetActive(false);
                // _speaker.enabled = true;
            }
            
            
            
        }
    }
}