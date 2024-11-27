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
            if (GameManager.Instance.GameState == GameState.Double)
            {
                GameManager.Instance.EnterDoubleRoom("1", "2");
            }
            else
            {
                GameManager.Instance.EnterWorld();
            }
            Destroy(parentCanvas.gameObject);
        }
    }
}
