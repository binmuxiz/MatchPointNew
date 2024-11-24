using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile
{
    public class Canvas5 : BaseCanvas2 , IDetailNextButton
{
    public string[] subjects = { "성격 및 특성", "외모", "능력 및 특기" };

    public GameObject content;
    public GameObject linePrefab;
    public GameObject button2Prefab;
    public GameObject SubjectTextPrefab;

    List<string[]> paaInList = new List<string[]>();
    
    List<ButtonData> buttonDataList = new List<ButtonData>();
    
    [SerializeField]List<string> personality = new List<string>();
    [SerializeField]List<string> appearance = new List<string>();
    [SerializeField]List<string> skills = new List<string>();

    private float currentWidth;
    private float viewWidth;

    private void Awake()
    {
        currentWidth = 50;
        ProfileOptions.SetPaaInList(paaInList);
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        viewWidth = content.GetComponent<RectTransform>().rect.width;

        InstantiateSubjectText(SubjectTextPrefab, content, subjects[0]);
        GameObject line = InstantiateLine(linePrefab, content);

        for (int i = 0; i < paaInList.Count; i++)
        {
            if (i != 0)
                InstantiateSubjectText(SubjectTextPrefab, content, subjects[i]);
            for (int j = 0; j < paaInList[i].Length; j++)
            {
                GameObject button = InstantiateButton2(button2Prefab,i);
                button.GetComponentInChildren<TMP_Text>().text = paaInList[i][j];
                Canvas.ForceUpdateCanvases();
                
                if (button.GetComponent<RectTransform>().rect.width + 10 + currentWidth > viewWidth - 50)
                {
                    line = InstantiateLine(linePrefab, content);
                    currentWidth = 50;
                }

                button.transform.SetParent(line.transform, false);
                currentWidth += (button.GetComponent<RectTransform>().rect.width + 10);
            }
        }
    }

    private GameObject InstantiateButton2(GameObject button2Prefab,int index)
    {
        GameObject button2 = Instantiate(button2Prefab);

        ButtonData buttonData = new ButtonData(button2.GetComponent<Button>(),index,false);
        buttonDataList.Add(buttonData);
        
        buttonData.buttonComponent.onClick.AddListener(() => ClickButton(buttonData));
        
        return button2;
    }
    
    private void ClickButton(ButtonData buttonData)
    {
        if (buttonData.selected)
        {
            Debug.Log("해제");
            buttonData.selected = false;
            ChangeColor(buttonData.buttonComponent.gameObject.GetComponent<Image>(),Color.white);
        }
        else
        {
            Debug.Log("등록");
            buttonData.selected = true;
            ChangeColor(buttonData.buttonComponent.gameObject.GetComponent<Image>(),Color.gray);
        }
        ReviseOutList(buttonData);
    }

    private void ReviseOutList(ButtonData buttonData) // 수정
    {
        List<string> temp = new List<string>();
        switch (buttonData.index)
        {
            case 0:
                temp = personality;
                break;
            case 1:
                temp = appearance;
                break;
            case 2:
                temp = skills;
                break;
        }

        if (buttonData.selected)
        {
            Debug.Log("등록");
            temp.Add(buttonData.buttonComponent.gameObject.GetComponentInChildren<TMP_Text>().text);
        }
        else
        {
            Debug.Log("해제");
            temp.Remove(buttonData.buttonComponent.gameObject.GetComponentInChildren<TMP_Text>().text);
        }
    }
    

    class ButtonData
    {
        public ButtonData(Button buttonComponent,int index,bool selected)
        {
            this.buttonComponent = buttonComponent;
            this.index = index;
            this.selected = selected;
        }
        public Button buttonComponent;
        public int index;
        public bool selected;
    }

    public void DetailNextButton()
    {
        Data.Profile request = ProfileManager.Instance.profile;

        if (ProfileManager.Instance._state == ProfileManager.State.Mine)
        {
            request.user.personality = personality.ToArray();
            request.user.appearance = appearance.ToArray();
            request.user.skills = skills.ToArray();
        }
        else
        {
            request.idealType.personality = personality.ToArray();
            request.idealType.appearance = appearance.ToArray();
            request.idealType.skills = skills.ToArray();
        }
                
        Clear();
    }


    protected override void Clear()
    {
        for (int i = 0; i < buttonDataList.Count; i++)
        {
            if (buttonDataList[i].selected)
            {
                buttonDataList[i].selected = false;
                buttonDataList[i].buttonComponent.gameObject.GetComponent<Image>().color = Color.white;
            }
        }
        personality = new List<string>();
        appearance = new List<string>();
        skills = new List<string>();
    }
}

}
