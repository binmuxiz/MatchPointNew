using Unity.Cinemachine;
using UnityEngine;

public class CameraController: Singleton<CameraController>
{
    public Camera mainCamera;
    public CinemachineBrain brain;
    public CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is null!!!!!!!!!!!!");
        }
        
        else
        {
            brain = mainCamera.GetComponent<CinemachineBrain>();
            if (brain != null)
            {
                brain.enabled = false;
            }
        }
    }

    

    public void SetWorldCamera(Transform camFollowTarget)
    {
        mainCamera.transform.position = new Vector3(9, 3, 0);
        mainCamera.transform.rotation = Quaternion.Euler(15, 90, 0); 
        
        if (brain != null && cinemachineCamera != null)
        {
            brain.enabled = true;
            cinemachineCamera.Follow = camFollowTarget.transform;
        }
        else
        {
            Debug.LogError("null!!!!!!!!");
        }
    }
    
    

    public void SetGroupMeetingRoomCamera()
    {
        mainCamera.transform.position = new Vector3(27, 3.35f, 3.5f);
        mainCamera.transform.rotation = Quaternion.Euler(20, 128, 0.6f);

        if (brain != null && cinemachineCamera != null)
        {
            brain.enabled = false;
            cinemachineCamera.Follow = null;    
        }
        else
        {
            Debug.LogError("null!!!!!!!!");
        }
    }

    public void SetFacingRoomCamera()
    {
        
    }
}
