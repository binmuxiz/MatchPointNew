using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class Login: MonoBehaviour
{
    public GameObject loginUIGO;
    public TMP_InputField idField;
    public Button loginButton;

    private void Start()
    {
        loginButton.onClick.AddListener(OnClickedLoginButton);
    }

    public async void ShowLoginUI()
    {
        idField.text = null;
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        
        loginUIGO.SetActive(true);
    }    
    
    private async void OnClickedLoginButton()
    {
        string id = idField.text;

        if (id.IsNullOrEmpty()) return;
        
        await PlayerData.Instance.GetPlayerData(id);

        if (PlayerData.Instance.UserId != null)
        {
            GameManager.Instance.EnterWorld();
            loginUIGO.SetActive(false);    
        }
    }
}
