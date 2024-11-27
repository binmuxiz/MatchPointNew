using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LoadingCanvas: MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public TMP_Text loadingText;

        private void Start()
        {
            Fader.FadeOut(CanvasGroup);
        }

        public async void Loading(int ms, string message)
        {
            loadingText.text = message;
            await Fader.FadeIn(CanvasGroup, 1f);
            await UniTask.Delay(ms); // 대기화면 
            await Fader.FadeOut(CanvasGroup, 1f);
        }
    }
}