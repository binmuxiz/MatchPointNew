using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalanceGameManager : MonoBehaviour
{
    public static BalanceGameManager Instance;
    
    public GameObject Content;
    public GameObject TopicPrefab;
    public GameObject BalanceGameButton;
    public GameObject TopicCanvas;

    public GameObject RealBalanceGameCanvas;
    public GameObject BalanceGameUnit;
    public GameObject LeftButton;
    public GameObject RightButton;
    public TMP_Text BalanceGameTopicText; 
    
    public BalanceTopicList balanceTopicList;
    public RealBalanceGameList realBalanceGameList;

    public TMP_Text LeftNameText1;
    public TMP_Text LeftNameText2;
    public TMP_Text RightNameText1;
    public TMP_Text RightNameText2;

    public List<TMP_Text> nameText;
    private bool isShowed = false;
    
    //public int index;

    void Awake()
    {
        if (Instance == null) Instance = this;
        
    }

    
    public async void StartBalanceGameButton()
    {
        BalanceGameButton.SetActive(false);
        
        var response = await GameManager.NetworkController.GetBalanceGameList();
        var s = response.Body;
        
        Debug.Log(s);
        
        balanceTopicList = JsonConvert.DeserializeObject<BalanceTopicList>(s);
        TopicCanvas.SetActive(true);
        
        ShowBalanceTopicListButton();
    }

    public void ShowBalanceTopicListButton()
    {
        for(int i = 0;i < balanceTopicList.triples.Count;i++)
        {
            int index = i;
            
            GameObject button = Instantiate(TopicPrefab, Content.transform,false);
            button.GetComponentInChildren<TMP_Text>().text = $"Topic : {balanceTopicList.triples[i].topic}\nMaker : {balanceTopicList.triples[i].user_id}";
            Debug.Log("index : " + index);
            button.GetComponent<Button>().onClick.AddListener(() => TopicButton(index));
            index++;
        }
    }

    public void TopicButton(int index)
    {
        TopicCanvas.SetActive(false);
         Debug.Log("index : " + index);
        GetRealBalanceList(balanceTopicList.triples[index].topic_id);
    }

    public void GetRealBalanceList(string topic_id)
    {
        SharedData.Instance.GetRealBalanceGameListRpc(topic_id);
    }

    public async void SyncBalanceGameList(string topic_id)
    {
        var response = await GameManager.NetworkController.GetBalanceGame(topic_id);
        string temp = response.Body;
        Debug.Log(temp);
       
        SharedData.Instance.realBalanceGameList = JsonConvert.DeserializeObject<RealBalanceGameList>(temp); 

        
        
        
        Debug.Log("실행됨");
        BalanceGameProcess();
    }

    private async UniTask BalanceGameProcess()
    {
        Debug.Log("시작");
        BalanceGameUnit.SetActive(true);
        RealBalanceGameCanvas.SetActive(true);
        Debug.Log("ㅁㄻㄴㄻㄴㄻㄴㄹ");
        
        for (int i = 0; i < SharedData.Instance.realBalanceGameList.options.Count; i++)
        {
            TextClear();
            SetBalanceGameUnit(i);
            await UniTask.WaitUntil(() => SharedData.isChecked >= 2);
            await UniTask.Delay(2000);
            
            ButtonClear();
            TurnOnResultText();
            
            Debug.Log("서로 것 공개");
            await UniTask.Delay(2000);
            
            TurnOnResultText();
            
            SharedData.Instance.ClearRpc();
            if (i == SharedData.Instance.realBalanceGameList.options.Count - 1)
            {
                BalanceGameUnit.SetActive(false);
                RealBalanceGameCanvas.SetActive(false);
            }
        }
    }

    private void SetBalanceGameUnit(int index)
    {
        BalanceGameTopicText.text = SharedData.Instance.realBalanceGameList.topic;
        LeftButton.GetComponentInChildren<TMP_Text>().text = SharedData.Instance.realBalanceGameList.options[index].left;
        RightButton.GetComponentInChildren<TMP_Text>().text = SharedData.Instance.realBalanceGameList.options[index].right;
    }

    public void ButtonClicked(int index)
    {
        if (index == 0)
        {
            Debug.Log("LeftButton");
            LeftButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            Debug.Log("RightButton");
            RightButton.GetComponent<Image>().color = Color.gray;
        }
        SharedData.Instance.DoneRpc();
        SharedData.Instance.PublicizeProcessRpc(index,PlayerData.Instance.UserId);
    }

    private void ButtonClear()
    {
        LeftButton.GetComponent<Image>().color = Color.white;
        RightButton.GetComponent<Image>().color = Color.white;
    }

    private void TextClear()
    {
        LeftNameText1.text = "";
        LeftNameText2.text = "";
        
        RightNameText1.text = "";
        RightNameText2.text = "";
    }

    private void TurnOnResultText()
    {
        if (!isShowed)
        {
            Debug.Log("텍스트 켜짐");
            isShowed = true;
            foreach (var i in nameText)
            {
                i.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("텍스트 꺼짐");
            isShowed = false;
            foreach (var i in nameText)
            {
                i.gameObject.SetActive(false);
            }
        }
    }

    
    
    
}
