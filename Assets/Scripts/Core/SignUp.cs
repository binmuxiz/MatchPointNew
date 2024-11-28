using Cysharp.Threading.Tasks;
using Data;
using Network;
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
            SignUpProcess().Forget();            
        }

        private async UniTaskVoid SignUpProcess()
        {
            string id = idInputField.text;
            string pw = passwordInputField.text;

            LoginRequest request = new LoginRequest();
            request.user_id = id;
            request.password = pw;

            string data = JsonConvert.SerializeObject(request);
            Response response = await GameManager.NetworkController.SignUp(data);

            if (response.Code == 200)
            {
                Debug.Log("회원등록됨.");
            }
            else
            {
                Debug.Log("이미 존재하는 회원?");
                return;
            }

            PlayerData.Instance.UserId = id;
            
            ProfileInput.Instance.Show();
            await UniTask.WaitUntil(() => ProfileInput.Instance.isDone);
            
            await PlayerData.Instance.GetPlayerData(id);

            if (PlayerData.Instance.UserId != null)
            {
                await GameManager.Instance.EnterWorld();
                Destroy(parentCanvas.gameObject);
            }

            Debug.Log("회원 가입 완료");
            ProfileInput.Instance.gameObject.SetActive(false);
        }
    }
}