using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile
{
    public class Canvas4 : BaseCanvas2,IDetailNextButton
    {
        public string[] subjects = new string[] { "운동 및 피트니스", "여행 및 야외활동", "문화 및 예술", "생활 및 자기관리" };

        public GameObject content;
        public GameObject linePrefab;
        public GameObject button2Prefab;
        public GameObject SubjectTextPrefab;
        
        List<string[]> hobbyInList = new List<string[]>();

        private float currentWidth;
        private float viewWidth;

        private void Awake()
        {
            currentWidth = 50;
            ProfileOptions.SetHobbyInList(hobbyInList);
        }

        private void Start()
        {
            Canvas.ForceUpdateCanvases(); 
            viewWidth = content.GetComponent<RectTransform>().rect.width;
            
            InstantiateSubjectText(SubjectTextPrefab,content,subjects[0]);
            GameObject line = InstantiateLine(linePrefab,content);

            for (int i = 0; i < hobbyInList.Count; i++)
            {
                if(i!=0)
                    InstantiateSubjectText(SubjectTextPrefab,content,subjects[i]);
                for (int j = 0; j < hobbyInList[i].Length; j++)
                {
                    GameObject button = InstantiateButton2(button2Prefab);
                    button.GetComponentInChildren<TMP_Text>().text = hobbyInList[i][j];
                    Canvas.ForceUpdateCanvases();
                    if (button.GetComponent<RectTransform>().rect.width + 10 + currentWidth > viewWidth - 50)
                    {
                        line = InstantiateLine(linePrefab, content);
                        currentWidth = 50;
                    }
                    
                    button.transform.SetParent(line.transform,false);
                    currentWidth += (button.GetComponent<RectTransform>().rect.width + 10);
                }
            }
        }

        public void DetailNextButton()
        {
            Data.Profile request = ProfileInput.Instance.profile;

            if (ProfileInput.Instance._state == ProfileInput.State.Mine)
            {
                request.user.hobbies = outList.ToArray();     
            }
            else
            {
                request.idealType.hobbies = outList.ToArray();
            }
           
            Clear();
        }


        protected override void Clear()
        {
            List<Button> buttonsToClear = new List<Button>();
            foreach (var pair in button2List)
            {
                if (pair.Value == true)
                {
                    buttonsToClear.Add(pair.Key);
                }
            }

            foreach (var i in buttonsToClear)
            {
                ChangeColor(i.gameObject.GetComponentInChildren<Image>(),Color.white);
                button2List[i] = false;
            }

            outList = new List<string>();
        }
    }
}
