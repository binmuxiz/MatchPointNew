using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerInfoItem: MonoBehaviour
    {
        public Button button;
        
        [Header("UI Elements")]
        public TMP_Text nameText;
        public Image avatarImage;
        public Sprite[] sprites;

        [Header("Data")]
        public string userId;
        public SimpleProfile SimpleProfile;

        private void Awake()
        {
            button.onClick.AddListener(OnClicked);
        }


        public void OnClicked()
        {
            GroupRoom.Instance.clickedUserId = this.userId;
            GroupRoom.Instance.clickedUserInfo = this.SimpleProfile;
        }

        public void SetPlayerInfoItem(string userId, SimpleProfile simpleProfile)
        {
            this.userId = userId;
            SimpleProfile = simpleProfile;
            nameText.text = simpleProfile.name;

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
    }
}