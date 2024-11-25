using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController: Singleton<CameraController>
{
    public CinemachineBrain brain;

    public CinemachineCamera cinemachineCamera;

    public GameObject moveCamera;
    public GameObject lockCamera;
    

    private void Awake()
    {
        if (brain != null)
        {
            brain.enabled = false;
        }

        moveCamera.SetActive(true);
        lockCamera.SetActive(false);
    }


    public void SetWorldCamera(Transform camFollowTarget)
    {
        moveCamera.SetActive(true);
        lockCamera.SetActive(false);
        
        Debug.Log("Set World Camera");
        brain.enabled = true;

        cinemachineCamera.Follow = camFollowTarget.transform;
        
        // camFollow.target = camPosition;
        // camRotate.enabled = true;
        // camFollow.enabled = true;
    }
    
    

    public void SetGroupMeetingRoomCamera()  
    {
        moveCamera.SetActive(false);
        lockCamera.SetActive(true);
        // 그룹룸 카메라 고정
        // brain.enabled = false;
    }
}
