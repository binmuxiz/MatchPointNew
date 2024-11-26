using UnityEngine;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine.UI;

namespace UI.Profile
{
    public class BaseCanvas : MonoBehaviour
    {
        protected List<string[]> Names = new List<string[]>();

        protected void Set1()
        {
            int num = 140;
            for (int i = 0; i < 61; i++)
            {
                ProfileOptions.Height[i] = num++.ToString();
            }

            ProfileOptions.SetBaseProfileList(Names);
        }

        protected void Set2()
        {
            ProfileOptions.SetBaseProfileList2(Names);
        }

        protected void InstantiateButton(GameObject content, GameObject prefab, GameObject motherButton,
            GameObject scrollView, string name)
        {
            GameObject button = Instantiate(prefab);

            button.GetComponentInChildren<TMP_Text>().text = name;
            button.transform.SetParent(content.transform, false);

            button.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                motherButton.GetComponentInChildren<TMP_Text>().text = button.GetComponentInChildren<TMP_Text>().text;
                scrollView.SetActive(false);
            });
        }

        public void Clear(GameObject[] motherButton, string[] subjectName)
        {
            for (int i = 0; i < motherButton.Length; i++)
            {
                motherButton[i].GetComponentInChildren<TMP_Text>().text = subjectName[i];
            }
        }
    }

}