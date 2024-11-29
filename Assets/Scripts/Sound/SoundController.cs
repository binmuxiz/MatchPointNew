using System;
using UnityEngine;

namespace Sound
{
    public class SoundController: MonoBehaviour
    {
        private AudioSource audioSource;

        void Start()
        {
            // 현재 게임 오브젝트의 AudioSource 컴포넌트를 가져옵니다.
            audioSource = GetComponent<AudioSource>();
        }

        // private void OnEnable()
        // {
        //     Play();
        // }
        //
        // private void OnDisable()
        // {
        //     Stop();
        // }

        public void Play()
        {
            // 예: 스페이스바를 누를 때 사운드 재생
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }
    }
}