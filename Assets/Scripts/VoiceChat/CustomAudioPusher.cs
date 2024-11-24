using System;
using System.Collections.Generic;
using Photon.Voice;
using UnityEngine;

namespace VoiceChat
{
    /*
     * Photon Voice에서 사용자 정의 오디오 입력 소스로 사용.
     * 외부에서 오디오 데이터를 받아 Photon voice로 전달하면서, 동시에 오디오 데이터를 가공하여 저장하려는 목적 
     */ 
    public class CustomAudioPusher : IAudioPusher<float>
    {
        public int SamplingRate { get; }
        public int Channels { get; }
        public string Error { get; }

        private Action<float[]> callback;
        private AudioClip microphoneClip;
        private int lastSample = 0;
        private string selectedDevice;

        
        private List<float> audioDataBuffer = new List<float>();
        
        public CustomAudioPusher(int samplingRate, int channels, string deviceName)
        {
            SamplingRate = samplingRate;
            Channels = channels;
            selectedDevice = deviceName;
        }

        public void SetCallback(Action<float[]> callback, ObjectFactory<float[], int> bufferFactory)
        {
            this.callback = callback;
            StartMicrophone();
        }

        private void StartMicrophone()
        {
            microphoneClip = Microphone.Start(selectedDevice, true, 1, SamplingRate);
        }

        public void PushData()
        {
            if (microphoneClip == null) return;

            int pos = Microphone.GetPosition(selectedDevice);
            if (pos < lastSample)
            {
                lastSample = 0;
            }

            if (pos - lastSample > 0)
            {
                float[] data = new float[(pos - lastSample) * Channels];
                microphoneClip.GetData(data, lastSample);

                // 오디오 데이터를 버퍼에 추가
                lock (audioDataBuffer)
                {
                    audioDataBuffer.AddRange(data);
                }
                // 여기서 data를 처리하거나 저장할 수 있습니다
                
                callback(data);
                lastSample = pos;
            }
        }
        
        // 버퍼된 오디오 데이터를 반환
        public float[] GetAudioData()
        {
            Debug.Log("GetBufferedAudioData");
            lock (audioDataBuffer)
            {
                return audioDataBuffer.ToArray();
            }
        }

        // 버퍼 초기화
        public void ClearAudioBuffer()
        {
            Debug.Log("ClearAudioBuffer");
            lock (audioDataBuffer)
            {
                audioDataBuffer.Clear();
            }

        }

        public void Dispose()
        {
            Debug.Log("Dispose()");
            if (Microphone.IsRecording(selectedDevice))
            {
                Microphone.End(selectedDevice);
            }
        }
    }
}