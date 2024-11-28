using System;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class PopupLoadingSpinner: MonoBehaviour
    {
        private async void Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            await UniTask.WaitUntil(() => GroupRoom.Instance.aggregationDone);
            
            GroupRoom.Instance.ShowLoveResult();
            Destroy(this.gameObject);
        }
    }
}