using System;
using System.IO;
using Photon.Voice.Unity;
using UnityEngine;

namespace VoiceChat
{
    public class CustomVoiceController: MonoBehaviour
    {
        private CustomAudioPusher audioPusher;
        
        public Recorder recorder;

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
        
        private void Awake()
        {
            string micDevice = null;
            audioPusher = new CustomAudioPusher(48000, 1, micDevice);
        }

        private void Start()
        {
            Debug.Log("Voice Controller Started");
            recorder.InputFactory = () => audioPusher;
            recorder.SourceType = Recorder.InputSourceType.Factory;
            
            Disconnect();
        }
        
        private void Update()
        {
            if (!IsConnected()) return;
            
            // Debug.Log("Update()");
            // 매 프레임마다 오디오 데이터를 푸시합니다
            audioPusher.PushData();
        }

        
        // 버퍼된 오디오 데이터 반환
        public byte[] GetBufferedAudioData()
        {
            if (!IsConnected()) return null;
            
            // 버퍼된 오디오 데이터 가져오기
            var audioData = audioPusher.GetAudioData();
            
            // WAV 바이트 배열로 변환
            var wavData = WavUtility.ConvertAudioDataToWav(audioData, audioPusher.SamplingRate, audioPusher.Channels);
            
            // 버퍼 초기화
            audioPusher.ClearAudioBuffer();

            return wavData;
        }
        
        

        private bool IsConnected()
        {
            return recorder.TransmitEnabled && recorder.RecordingEnabled;
        }


            
        void OnDisable()
        {
            audioPusher.Dispose();
        }


        
        // private IEnumerator<> UploadAudioData(byte[] wavData)
        // {
        //     // ServerUploader 인스턴스 생성 또는 가져오기
        //     ServerUploader uploader = GetComponent<ServerUploader>();
        //     if (uploader == null)
        //     {
        //         uploader = gameObject.AddComponent<ServerUploader>();
        //     }
        //
        //     // 필요한 데이터 설정
        //     string userId = "user123";
        //     string sessionId = "session456";
        //     long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        //
        //     // 업로드 코루틴 실행
        //     yield return uploader.UploadWavData(wavData, userId, sessionId, timestamp);
        // }
        
        
        // 버튼 클릭 시 호출되는 메서드
        private void SaveBufferedAudio()
        {
            Debug.Log("Saving buffered audio");

            // 버퍼된 오디오 데이터 가져오기
            float[] audioData = audioPusher.GetAudioData();

            // string fileName = $"RecordedAudio_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.wav";
            string filePath = $"Assets/Resources/RecordedAudio_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.wav";
            // string filePath = Path.Combine(Application.persistentDataPath, fileName);

            SaveWavFile(filePath, audioData, audioPusher.SamplingRate, audioPusher.Channels);
            Debug.Log("SaveBufferedAudio");

            Debug.Log($"Recorder IsRecording: {recorder.RecordingEnabled}");
            Debug.Log($"Recorder IsCurrentlyTransmitting: {recorder.IsCurrentlyTransmitting}");
            // 버퍼 초기화
            audioPusher.ClearAudioBuffer();
        }

        
        
         
        private void SaveWavFile(string filePath, float[] audioData, int sampleRate, int channels)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));
            directoryInfo.Create();
            
            // string filePath = Path.Combine(Application.persistentDataPath, filename);

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // WAV 헤더 작성
                int byteRate = sampleRate * channels * 2; // 16비트(2바이트) 샘플
                int fileSize = 36 + audioData.Length * 2;

                // RIFF 헤더
                WriteString(fileStream, "RIFF");
                WriteInt(fileStream, fileSize);
                WriteString(fileStream, "WAVE");

                // fmt 서브청크
                WriteString(fileStream, "fmt ");
                WriteInt(fileStream, 16); // 서브청크 크기
                WriteShort(fileStream, 1); // 오디오 포맷 (1 = PCM)
                WriteShort(fileStream, (short)channels);
                WriteInt(fileStream, sampleRate);
                WriteInt(fileStream, byteRate);
                WriteShort(fileStream, (short)(channels * 2)); // 블록 얼라인
                WriteShort(fileStream, 16); // 비트 수

                // data 서브청크
                WriteString(fileStream, "data");
                WriteInt(fileStream, audioData.Length * 2);

                // 오디오 데이터 작성
                byte[] bytesData = new byte[audioData.Length * 2];
                int rescaleFactor = 32767; // to convert float to Int16

                for (int i = 0; i < audioData.Length; i++)
                {
                    short value = (short)(audioData[i] * rescaleFactor);
                    byte[] bytes = BitConverter.GetBytes(value);
                    bytes.CopyTo(bytesData, i * 2);
                }

                fileStream.Write(bytesData, 0, bytesData.Length);
            }

            Debug.Log($"WAV 파일 저장 완료: {filePath}");
        }

        private void WriteString(Stream stream, string value)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteInt(Stream stream, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteShort(Stream stream, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }
        
        
        
    }
}