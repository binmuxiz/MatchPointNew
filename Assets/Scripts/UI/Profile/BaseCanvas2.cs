using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Profile
{
    public abstract class BaseCanvas2 : MonoBehaviour
    {
        [SerializeField] protected List<string> outList = new List<string>();
        
        public Dictionary<Button, bool> button2List = new Dictionary<Button, bool>();

        protected GameObject InstantiateLine(GameObject linePrefab, GameObject content)
        {
            GameObject line = Instantiate(linePrefab);
            line.transform.SetParent(content.transform, false);

            return line;
        }

        protected GameObject InstantiateButton2(GameObject button2Prefab)
        {
            GameObject button2 = Instantiate(button2Prefab);
            Button buttonComponent = button2.GetComponent<Button>();
            button2List[buttonComponent] = false;
            buttonComponent.onClick.AddListener(() => ClickButton(buttonComponent));

            return button2;
        }

        protected void ChangeColor(Image image, Color color)
        {
            image.color = color;
        }

        protected void ClickButton(Button buttonComponent) // 가상 메서드로 바꾸자
        {
            if (button2List[buttonComponent])
            {
                button2List[buttonComponent] = false;
                ChangeColor(buttonComponent.gameObject.GetComponent<Image>(), Color.white);
                outList.Remove(buttonComponent.gameObject.GetComponentInChildren<TMP_Text>().text);
            }
            else
            {
                button2List[buttonComponent] = true;
                ChangeColor(buttonComponent.gameObject.GetComponent<Image>(), Color.gray);
                outList.Add(buttonComponent.gameObject.GetComponentInChildren<TMP_Text>().text);
            }
        }

        protected void InstantiateSubjectText(GameObject subjectTextPrefab, GameObject content, string text)
        {
            GameObject subject = Instantiate(subjectTextPrefab);
            subject.GetComponentInChildren<TMP_Text>().text = text;
            subject.transform.SetParent(content.transform, false);
        }

        protected void Clear(Dictionary<Button, bool> buttonList)
        {
            int index = 0;
            Button[] buttons = new Button[buttonList.Count];
            buttonList.Keys.CopyTo(buttons, 0);

            foreach (var i in buttonList.Values)
            {
                if (i == true)
                {
                    buttonList[buttons[index]] = false;
                    ChangeColor(buttons[index].gameObject.GetComponent<Image>(), Color.white);
                }
            }

        }

        protected abstract void Clear();
    }
}
