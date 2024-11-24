using Cysharp.Threading.Tasks;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OtherUserUI : MonoBehaviour
    {
        public GameObject canvas;

        [SerializeField] private CanvasGroup profileCanvasGroup;
        
        [SerializeField] private CanvasGroup chatCanvasGroup;
        [SerializeField] private CanvasGroup videoMeetingPopupCanvasGroup;

        [SerializeField] private Image profileImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text genderText;
        [SerializeField] private TMP_Text summaryText;
        [SerializeField] private Slider temperatureSlider;

        [SerializeField] private Button heartButton;
        [SerializeField] private Button chatButton;
        [SerializeField] private Button videoMeetingButton;

        public Sprite[] profileSprites = new Sprite[4];
        
        
        [SerializeField] private string otherId;
        [SerializeField] private SimpleProfile _otherProfile;

        public bool opened;
        
        private void Start()
        {
            opened = false;
            Fader.FadeOut(profileCanvasGroup, 0f).Forget();
            
        }


        public void ShowTest()
        {
            Fader.FadeIn(profileCanvasGroup, 0f).Forget();
        }
        

        public async void Show(string otherId)
        {
            if (opened) return;
            opened = true;
            
            this.otherId = otherId;
            // this._otherProfile = await NetworkManager.Instance.UserController.GetUserSimpleProfile(PlayerData.Instance.UserId, otherId);

            SetOtherSimpleProfile(this._otherProfile);
            Fader.FadeIn(profileCanvasGroup, 0f).Forget();
        }

        public void Close()
        {
            opened = false;
            Fader.FadeOut(profileCanvasGroup, 0f).Forget();
            otherId = null;
            _otherProfile = null;
        }

        private void SetOtherSimpleProfile(SimpleProfile simpleProfile)
        {
            nameText.text = "이름: " + simpleProfile.name;
            genderText.text = "성별: " + simpleProfile.gender;
            summaryText.text = simpleProfile.summary;
            temperatureSlider.value = float.Parse(simpleProfile.similarity.Replace("%", ""));
        }


        // private void OnClickedMeetingButton()
        // {
        //     Debug.Log($"내 id : {PlayerData.Instance.UserId}, otherId : {otherId}");
        // }
    }
}