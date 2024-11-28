using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WebSocketSharp;

public class Login: MonoBehaviour
{
    public TMP_InputField idInputField;
    public Button loginButton;
    
    public Transform parentCanvas;
    

    private void Start()
    {
        parentCanvas = transform.parent;
        
        loginButton.onClick.AddListener(OnClickedLoginButton);
    }

    private async void OnClickedLoginButton()
    {
        string id = idInputField.text;

        if (id.IsNullOrEmpty()) return;
        
        await PlayerData.Instance.GetPlayerData(id);

        if (PlayerData.Instance.UserId != null)
        {
            GameManager.Instance.EnterWorld();
            Destroy(parentCanvas.gameObject);
        }
    }
}
