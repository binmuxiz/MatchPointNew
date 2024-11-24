using UnityEngine;
using TMPro;

namespace UI.Profile
{
    public class Canvas3 : BaseCanvas, IDetailNextButton
    {
        public GameObject buttonPrefab;
        public GameObject[] scrollViews;
        public GameObject[] motherButton;
        public GameObject[] scrollViewContent;

        private string[] subjects = new string[] { "MBTI", "종교", "흡연", "음주" };
        private void Awake()
        {
            Set2();
        }
    
        private void Start()
        {
            for (int i = 0; i < Names.Count; i++)
            {
                for (int j = 0; j < Names[i].Length; j++)
                {
                    InstantiateButton(scrollViewContent[i], buttonPrefab, motherButton[i], scrollViews[i],Names[i][j]);
                }
            }
        }
    
        public void ClickSubjectButton(int idx)
        {
            scrollViews[idx].SetActive(true);
        }
        
        public void CloseScrollView(int idx)
        {
            scrollViews[idx].SetActive(false);
        }
        
        
        public void DetailNextButton()
        {
            Data.Profile request = ProfileManager.Instance.profile;

            
            request.user.mbti = motherButton[0].GetComponentInChildren<TMP_Text>().text;
            request.user.religion = motherButton[1].GetComponentInChildren<TMP_Text>().text;
            request.user.smoking = motherButton[2].GetComponentInChildren<TMP_Text>().text;
            request.user.drinking = motherButton[3].GetComponentInChildren<TMP_Text>().text;
        
            Clear(motherButton,subjects);
        }
    
    }
    
}
