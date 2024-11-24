using UI;
using UnityEngine;

public class ObjectClickDetector: MonoBehaviour
{
    public Camera _mainCamera;
    public GroupEnter groupEnter;
    
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();                
        }
    }

    private void DetectClick()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
        
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject go = hit.collider.gameObject;
            
            if (go.CompareTag("Player"))
            {
                Debug.Log("player clicked");
                HandlePlayerClicked(go);
            }
            else if (go.CompareTag("GroupMeeting"))
            {
                Debug.Log("GroupMeetingGameObject");
                HandleGroupMeetingObjectClicked(go);
            }
        }
    }


    private void HandleGroupMeetingObjectClicked(GameObject go)
    {
        groupEnter.ShowUI();
    }

    private void HandlePlayerClicked(GameObject go)
    {
        var sharedData = go.GetComponent<SharedData>();
        if (sharedData == null)
        {
            Debug.LogError("플레이어가 SharedData를 가지고 있지 않음");
            return;
        } 
        
        if (sharedData.UserId == PlayerData.Instance.UserId) // 자신을 클릭 
        {
            return;
        }
        
        // ShowPlayerSimpleProfile(sharedData.UserId);
    }
    
    
    // private void ShowPlayerSimpleProfile(string otherId)
    // {
    //     Debug.Log($"myId: {PlayerData.Instance.UserId}, otherId: {otherId}");
    //     otherUserUI.Show(otherId);
    // }
}
