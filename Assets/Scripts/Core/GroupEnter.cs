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
        roomList.Add(new RoomInfo("Group_1", "1인방{테스트방}", 0, 1, "이은빈"));
        roomList.Add(new RoomInfo("Group_2", "2인방{테스트방}", 0, 2, "김민주"));
        roomList.Add(new RoomInfo("Group_3", "베타발표방", 1, 4, "김민준"));
        roomList.Add(new RoomInfo("Group_4", "어서오슈", 2, 4, "이동섭"));
        roomList.Add(new RoomInfo("Group_5", "20대만 들어와", 3, 4, "김정현")); 
        roomList.Add(new RoomInfo("Group_6", "매치포인트", 3, 4, "이은빈"));

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
        Popup popup = GetComponent<Popup>();
        popup.Close();
        
        GameManager.Instance.EnterGroupRoom(roomInfo);
    }
}
