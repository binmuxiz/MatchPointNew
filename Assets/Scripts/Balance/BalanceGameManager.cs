using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Newtonsoft.Json;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BalanceGameManager : MonoBehaviour
{
    public static BalanceGameManager Instance;
    
    public GameObject Content;
    public GameObject TopicPrefab;
    public GameObject BalanceGameButton;
    public GameObject BalanceGameListCanvas;
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

    private Color color;
    public List<TMP_Text> nameText;
    private bool isShowed = false;
    
    //public int index;

    void Awake()
    {
        if (Instance == null) Instance = this;
        color = LeftButton.GetComponentInChildren<Image>().color;
        
        BalanceGameListCanvas.SetActive(false);
    }

    
    public async void StartBalanceGameButton()
    {
        BalanceGameButton.SetActive(false);
        
        var response = await GameManager.NetworkController.GetBalanceGameList();
        var s = response.Body;
        
        Debug.Log(s);
        
        balanceTopicList = JsonConvert.DeserializeObject<BalanceTopicList>(s);
        BalanceGameListCanvas.SetActive(true);
        
        ShowBalanceTopicListButton();
    }

    public void ShowBalanceTopicListButton()
    {
        int idx = 1;
        foreach (var balanceGame in balanceTopicList.triples)
        {
            GameObject go = Instantiate(TopicPrefab, Content.transform,false);
            var itemUI = go.GetComponent<BalanceGameItem>();

            itemUI.no.text = $"{idx++}";
            itemUI.title.text = $"{balanceGame.topic}";
            itemUI.creator.text = $"{balanceGame.user_id}";
            itemUI.playCount.text = $"{idx}";

            Button button = itemUI.button;
            
            int index = idx - 1;
            button.onClick.AddListener(() => OnClickedTopicItemButton(index));
        }
    }

    private void OnClickedTopicItemButton(int index)
    {
        Debug.Log("OnClickedTopicItemButton");
        
        BalanceGameListCanvas.SetActive(false);
        
        GetRealBalanceList(balanceTopicList.triples[index].topic_id);
    }

    private void GetRealBalanceList(string topic_id)
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
            Debug.Log(1111111);
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
            LeftButton.GetComponentInChildren<Image>().color = Color.gray;
        }
        else
        {
            Debug.Log("RightButton");
            RightButton.GetComponentInChildren<Image>().color = Color.gray;
        }
        SharedData.Instance.DoneRpc();
        SharedData.Instance.PublicizeProcessRpc(index,PlayerData.Instance.UserId);
    }

    private void ButtonClear()
    {
        LeftButton.GetComponentInChildren<Image>().color = color;
        RightButton.GetComponentInChildren<Image>().color = color;
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
