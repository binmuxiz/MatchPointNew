using System.Collections;
using System.Xml;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController: Singleton<CameraController>
{
    public CinemachineBrain brain;

    private void Awake()
    {
        if (brain != null)
        {
            brain.enabled = false;
        }
    }


    
    public void SetGroupMeetingRoomCamera()  
    {
        // 그룹룸 카메라 고정
        brain.enabled = false;

        transform.position = new Vector3(94, 28, -144);
    }
    
    public IEnumerator SetCinemachineCameraTarget(Transform transform)
    {
        brain.enabled = true;

        yield return new WaitForEndOfFrame();
        
        CinemachineCamera cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();

        if (cinemachineCamera != null)
        {
            Transform camFollow = null;
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("CamFollow"))
                {
                    camFollow = child;
                    break; // 원하는 자식 오브젝트를 찾으면 반복문 종료
                }
            }

            if (camFollow != null)
            {
                cinemachineCamera.Follow = camFollow;
                Debug.Log($"Follow target 설젇: {camFollow.name}");
            }
            else
            {
                Debug.LogError("CamFollow 태그를 가진 자식 오브젝트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("CinemachineCamera를 찾을 수 없습니다.");
        }
    }
}
