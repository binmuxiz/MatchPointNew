using UnityEngine;

namespace Utilities
{
    public class MouseCursorChanger: MonoBehaviour
    {
        public Texture2D cursorTexture; // 커서로 사용할 이미지
        public string targetTag = "Player"; // 마우스 오버 대상 태그
        private Vector2 hotSpot = Vector2.zero; // 커서 핫스팟 위치

        void Update()
        {
            // 마우스 위치에서 Ray를 쏨
            Camera cam = Camera.main;
            if (cam == null) return;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 히트한 오브젝트가 타겟 태그를 가지고 있다면
                if (hit.collider.CompareTag(targetTag))
                {
                    Debug.Log("Mouse cursor changed");
                    Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto); // 커서 변경
                }
                else
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 기본 커서로 복원
                }
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 기본 커서로 복원
            }
        }
    }
}