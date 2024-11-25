using Ricimi;
using UnityEngine;

namespace Core
{
    public class GroupMeetingRoomEntrance: MonoBehaviour
    { 
        // private void Update()
        // {
        //     if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         if (Physics.Raycast(ray, out RaycastHit hit))
        //         {
        //             if (hit.transform.gameObject == gameObject) // 자기 자신인지 확인
        //             {
        //                 Debug.Log("자기 자신 클릭됨!");
        //             }
        //         }
        //     }
        // }
        
        public GameObject popupPrefab;
        public Canvas canvas;
        
        protected GameObject m_popup;


        private void OnMouseDown()
        {
            if (gameObject == this.gameObject)
            {
                Debug.Log("자기 자신 클릭됨!");
                OpenPopup();
            }
        }
        
        private void OpenPopup()
        {
            m_popup = Instantiate(popupPrefab, canvas.transform, false);
            m_popup.SetActive(true);
            m_popup.GetComponent<Popup>().Open();
        }
    }
}