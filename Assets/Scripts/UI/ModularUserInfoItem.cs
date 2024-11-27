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
            summary.text = profile.summary;
            Debug.Log($"유사도 : {profile.similarity}");
            temperatureSlider.value = float.Parse(profile.similarity.Replace("%", ""));
            
            switch (userId)
            {
                case "1":
                    avatarImage.sprite = sprites[3];
                    break;
                case "2":
                    avatarImage.sprite = sprites[0];
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
            
            // if (myId == votedId) return;
            
            voteButton.interactable = false;
            Instantiate(heartStampPrefab, this.transform, false);
            
            SharedData.Instance.RpcVote(votedId);
        }
    }
}