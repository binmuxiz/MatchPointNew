using Cysharp.Threading.Tasks;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ModularUserInfoItem: MonoBehaviour
    {
        public TMP_Text userName;
        public TMP_Text summary;
        public Slider temperatureSlider;
        public Image avatarImage;
        
        public Sprite[] sprites = new Sprite[4];

        public string userId;

        public Button voteButton;

        public GameObject heartStampPrefab;

        private async void Start()
        {
            await UniTask.WaitUntil(() => GroupRoom.Instance.clickedUserId != null);
            this.userId = GroupRoom.Instance.clickedUserId;
            SimpleProfile profile = GroupRoom.Instance.clickedUserInfo;
            
            GroupRoom.Instance.clickedUserInfo = null;
            GroupRoom.Instance.clickedUserId = null;
            
            userName.text = profile.name;

            if (userId == "1")    // 김민준
            {
                temperatureSlider.value = 72f;
                summary.text = "김민준은 신뢰할 수 있고 성실한 성격을 가지고 있으며, 주요 관심사로 축구, 야구, 여행을 가지고 있습니다.";
            }
            else
            {
                temperatureSlider.value = float.Parse(profile.similarity.Replace("%", ""));
                summary.text = profile.summary;
            }
            
            switch (userId)
            {
                case "1":
                    avatarImage.sprite = sprites[0];
                    break;
                case "2":
                    avatarImage.sprite = sprites[1];
                    break;
                case "3":
                    avatarImage.sprite = sprites[2];
                    break;
                case "4":
                    avatarImage.sprite = sprites[3];
                    break;
                default:
                    avatarImage.sprite = sprites[0];
                    break;
            }
        }

        public void Vote()
        {
            Debug.Log("Vote()");
            string myId = PlayerData.Instance.UserId;
            string votedId = this.userId;

            GroupRoom.Instance.votedUserId = votedId;
            
            if (myId == votedId) return;  // 내 자신 투표 못하도록  
            
            voteButton.interactable = false;
            Instantiate(heartStampPrefab, this.transform, false);
            
            SharedData.Instance.RpcVote(votedId);
        }
    }
}