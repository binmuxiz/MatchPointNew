using System;
using TMPro;
using UnityEngine;

namespace UI.Profile
{
    public class Canvas2 : BaseCanvas, IDetailNextButton
    {
        public GameObject buttonPrefab;
        public GameObject[] scrollViews;
        public GameObject[] motherButton;
        public GameObject[] scrollViewContent;
    
        private string[] subjects = new string[] { "최종학력", "키", "체형", "돌싱/자녀유무" };

        private void Awake()
        {
            Set1();
        }

        private void Start()
        {
            for (int i = 0; i < Names.Count; i++)
            {
                //GameObject[] temp = new GameObject[Names[i].Length];
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
            
            request.user.education = motherButton[0].GetComponentInChildren<TMP_Text>().text;
            request.user.height = Convert.ToInt32(motherButton[1].GetComponentInChildren<TMP_Text>().text);
            request.user.bodyType = motherButton[2].GetComponentInChildren<TMP_Text>().text;
            request.user.divorced = motherButton[3].GetComponentInChildren<TMP_Text>().text;
        
            Clear(motherButton,subjects);
        }
    }
}
