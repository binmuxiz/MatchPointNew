using TMPro;
using UnityEngine;

namespace UI
{
    public class RoomItem: MonoBehaviour
    {
        public TMP_Text roomNameText;
        public TMP_Text currentCountText;
        public TMP_Text maxCountText;
        
        public RoomInfo roomInfo;

        public void SetRoomItem(RoomInfo roomInfo)
        {
            this.roomInfo = roomInfo;
            
            roomNameText.text = roomInfo.roomName;
            currentCountText.text = roomInfo.currentCount.ToString();
            maxCountText.text = roomInfo.maxPlayerCount.ToString();
        }
    }
}