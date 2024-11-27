using ExitGames.Client.Photon;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickDetector: MonoBehaviour
{
    public OtherUserUI OtherUserUI;
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                DetectClick();
            }
        }
    }

    private void DetectClick()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
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
