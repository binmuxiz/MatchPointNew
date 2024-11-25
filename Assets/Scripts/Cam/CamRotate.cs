using UnityEngine;

public class CamRotate: MonoBehaviour
{
    public float rotationSpeed = 200f;

    private float mx = 0f;
    private float my = 0f;
    
    void Update()
    {
        // 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // Debug.Log($"x:{mouseX}, y:{mouseY}");

        // 회전 값 변수에 마우스 입력 값만큼 미리 누적시킨다,
        mx += mouseX * rotationSpeed * Time.deltaTime;
        my += mouseY * rotationSpeed * Time.deltaTime;
        // 마우스 상하 이동 회전 변수(my)의 값을 -90~90도 사이로 제한한다. 상하는 360회전 불가능하기 때문에. 
        my = Mathf.Clamp(my, -90f, 90f);
        // 회전 방향으로 물체를 회전시킨다. 
        transform.eulerAngles = new Vector3(-my, mx, 0f);
    }
}
