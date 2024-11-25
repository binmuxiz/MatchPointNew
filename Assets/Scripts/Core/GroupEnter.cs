using System.Collections.Generic;
using Ricimi;
using UI;
using UnityEngine;
using UnityEngine.UI;


public class GroupEnter: MonoBehaviour
{
    
    [Header("GroupMeeting List UI")]
    [SerializeField] private GameObject itemPrefab; // 생성할 프리팹
    [SerializeField] private Transform content;     // ScrollView의 Content 트랜스폼
    
    // 방 생성 UI 생략     

    private List<RoomInfo> roomList = new List<RoomInfo>();
    

    private void Start()
    {
        roomList.Add(new RoomInfo("Group_1", "1인 테스트방", 0, 1, "이은빈"));
        roomList.Add(new RoomInfo("Group_2", "2인방", 0, 2, "김민주"));
        roomList.Add(new RoomInfo("Group_3", "3인방", 0, 3, "김민준"));
        roomList.Add(new RoomInfo("Group_4", "4인방", 0, 4, "이동섭"));
        roomList.Add(new RoomInfo("Group_5", "5인방", 0, 5, "김정현")); 
        roomList.Add(new RoomInfo("Group_6", "6인방", 0, 6, "이은빈"));

        foreach (var roomInfo in roomList)
        {
            AddItem(roomInfo);
        }        
    }

    
    
    // 아이템 추가 메서드
    private void AddItem(RoomInfo info)
    {
        Debug.Log("AddItem");
        // Prefab 인스턴스화
        GameObject newItem = Instantiate(itemPrefab, content);

        RoomItem roomItem = newItem.GetComponent<RoomItem>();
        roomItem.SetRoomItem(info);

        Button button = newItem.transform.GetChild(0).GetComponent<Button>();
        // Button button = newItem.GetComponent<Button>();
        
        // Button 컴포넌트의 클릭 이벤트 등록
        button.onClick.AddListener(() => OnItemClicked(roomItem));
        
    }
    
    private void OnItemClicked(RoomItem roomItem)
    {
        RoomInfo roomInfo = roomItem.roomInfo;
        GameManager.Instance.EnterGroupRoom(roomInfo.roomId, roomInfo.maxPlayerCount);

        Popup popup = GetComponent<Popup>();
        popup.Close();
    }
}
