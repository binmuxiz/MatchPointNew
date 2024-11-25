using UI;
using UnityEngine;

public class ObjectClickDetector: MonoBehaviour
{
    public Camera _mainCamera;
    public OtherUserUI OtherUserUI;
    
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
        }
    }


    private void HandlePlayerClicked(GameObject go)
    {
        if (GameManager.Instance.GameState != GameState.World) return;
        
        var sharedData = go.GetComponent<SharedData>();
        if (sharedData == null) return;
        if (sharedData.UserId == PlayerData.Instance.UserId) return;
        
        ShowPlayerSimpleProfile(sharedData.UserId);
    }
    
    
    private void ShowPlayerSimpleProfile(string otherId)
    {
        Debug.Log($"myId: {PlayerData.Instance.UserId}, otherId: {otherId}");
        // otherUserUI.Show(otherId);
    }
}
