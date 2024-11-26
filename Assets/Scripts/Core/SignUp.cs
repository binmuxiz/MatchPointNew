using Data;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace Core
{
    public class SignUp: MonoBehaviour
    {
        public TMP_InputField idInputField;
        public TMP_InputField passwordInputField;

        public Transform parentCanvas;

        private void Start()
        {
            parentCanvas = transform.parent;
            
        }

        public void SignUpButton()
        {
            SignUpProcess();            
        }

        private async void SignUpProcess()
        {
            string id = idInputField.text;
            string pw = passwordInputField.text;

            LoginRequest request = new LoginRequest();
            request.user_id = id;
            request.password = pw;

            string data = JsonConvert.SerializeObject(request);
            await GameManager.NetworkController.SignUp(data);
            
            Debug.Log("sign up");
            
            await PlayerData.Instance.GetPlayerData(id);
            
            if (PlayerData.Instance.UserId != null)
            {
                GameManager.Instance.EnterWorld();
                Destroy(parentCanvas.gameObject);
            }
            
            //todo 프로필 세팅 캔버스 띄우기
        }
    }
}