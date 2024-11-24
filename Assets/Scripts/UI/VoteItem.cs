using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VoteItem: MonoBehaviour
    {
        public Button voteItemButton;
        
        public TMP_Text nameText;
        public Image image;
        
        public Sprite sprite;

        public Participant participant;

        public void Vote()
        {
            image.sprite = sprite;
            Debug.Log("투표: " + participant.name);
        }

        public void SetVoteItem(int index, Participant participant)
        {
            this.participant = participant;
            
            string gender = "여자";
            if (participant.gender == "남자")
            {
                gender = "남자";
            }
            
            nameText.text = $"{gender}{index}호\n{participant.name}";
        }
    }
}