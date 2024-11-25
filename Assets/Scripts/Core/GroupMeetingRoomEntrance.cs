using Ricimi;
using UnityEngine;

namespace Core
{
    public class GroupMeetingRoomEntrance: MonoBehaviour
    { 
        public GameObject popupPrefab;
        public Canvas canvas;
        
        protected GameObject m_popup;

        private void OnMouseDown()
        {
            if (GameManager.Instance.GameState == GameState.Group)
            {
                return;
            }
            
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