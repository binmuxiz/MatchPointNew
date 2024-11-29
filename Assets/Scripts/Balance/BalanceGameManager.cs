using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BalanceGameManager : MonoBehaviour
{
    public static BalanceGameManager Instance;
    
    public GameObject Content;
    public GameObject TopicPrefab;
    public GameObject BalanceGameListPanel;
    public GameObject BalanceGamePlayPanel;
    
    
    public GameObject LeftButton;
    public GameObject RightButton;
    public TMP_Text BalanceGameTopicText; 
    
    public BalanceTopicList balanceTopicList;

    // public TMP_Text LeftNameText1;
    // public TMP_Text LeftNameText2;
    // public TMP_Text RightNameText1;
    // public TMP_Text RightNameText2;

    public TMP_Text leftText;
    public TMP_Text rightText;

    private Color color;
    // public List<TMP_Text> nameText;
    private bool isShowed = false;


    [Header("Selection")] 
    public GameObject[] leftSelections;
    public GameObject[] rightSelections;

    void Awake()
    {
        if (Instance == null) Instance = this;
        color = LeftButton.GetComponentInChildren<Image>().color;
        
        
    }

    private void OnEnable()
    {
        BalanceGameListPanel.SetActive(false);
        /**/BalanceGamePlayPanel.SetActive(false);
    }


    public void CloseBalanceGameListPanel()
    {
        BalanceGameListPanel.SetActive(false);
    }
    
    
    public async void ShowBalanceGameListButton()  // 밸런스 게임 목록 버튼 클릭
    {
        var response = await GameManager.NetworkController.GetBalanceGameList();
        var s = response.Body;
        
        Debug.Log(s);
        
        balanceTopicList = JsonConvert.DeserializeObject<BalanceTopicList>(s);
        BalanceGameListPanel.SetActive(true);  // UI 켜기 
        
        SetBalanceGameListData();  // 데이터 받아오기 
    }
    

    private void SetBalanceGameListData()
    {
        int idx = 1;
        foreach (var balanceGame in balanceTopicList.triples)
        {
            GameObject go = Instantiate(TopicPrefab, Content.transform,false);
            var itemUI = go.GetComponent<BalanceGameItem>();

            int index = idx - 1;
            itemUI.no.text = $"{idx++}";
            itemUI.title.text = $"{balanceGame.topic}";
            itemUI.creator.text = $"{balanceGame.user_id}";
            itemUI.playCount.text = $"{idx}";

            Button button = itemUI.button;
            
            button.onClick.AddListener(() => OnClickedTopicItemButton(index));
        }
    }
    
    

    private void OnClickedTopicItemButton(int index)   // 밸런스게임 아이템 선택 
    {
        Debug.Log("OnClickedTopicItemButton");
        
        BalanceGameListPanel.SetActive(false);      // 밸런스 게임 목록 UI 끄기
        
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

    
    
    
    private async UniTask BalanceGameProcess()   // 밸런스 게임 프로세스 
    {
        Debug.Log("시작");
        SelectionClear();           
        ButtonClear();             
        BalanceGamePlayPanel.SetActive(true);

        int count = 1;  // 1개만 할거임 
        
        // for (int i = 0; i < SharedData.Instance.realBalanceGameList.options.Count; i++)
        for (int i = 0; i < count; i++)   
        {
            // TextClear();
            SelectionClear();           // 선택한 오브젝트 다 끄기 
            ButtonClear();              
            SetBalanceGameUnit(4);  // 시연 할 두 밸런스게임 둘다 4번째꺼 항목만 시연함  
            
            await UniTask.WaitUntil(() => SharedData.isChecked >= 2);
            await UniTask.Delay(1000);
            
            // TurnOnResultText();
            
            // Debug.Log("서로 것 공개");
            // await UniTask.Delay(2000);
            
            // TurnOnResultText();
            
            SharedData.Instance.ClearRpc();
        }
        BalanceGamePlayPanel.SetActive(false);  // 밸런스게임 플레이 UI 끄기
        
        DoubleRoom.Instance.ShowDefaultPanel();
    }


    private void SelectionClear()
    {
        leftSelections[0].SetActive(false);
        leftSelections[1].SetActive(false);
        rightSelections[0].SetActive(false);
        rightSelections[1].SetActive(false);
    }

    private void SetBalanceGameUnit(int index)
    {
        BalanceGameTopicText.text = SharedData.Instance.realBalanceGameList.topic;
        leftText.text = SharedData.Instance.realBalanceGameList.options[index].left;
        rightText.text = SharedData.Instance.realBalanceGameList.options[index].right;
    }

    
    public void ButtonClicked(int index)  // 0이면 left 선택, 1이면 right 선택 
    {
        if (index == 0)
        {
            Debug.Log("LeftButton");
            LeftButton.GetComponent<Image>().color = Color.gray;
            // leftSelections[0].SetActive(true);
            
        }
        else
        {
            Debug.Log("RightButton");
            RightButton.GetComponent<Image>().color = Color.gray; 
            // rightSelections[0].SetActive(true);
        }
        
        SharedData.Instance.DoneRpc(SharedData.Instance.UserName, index);
        //SharedData.Instance.PublicizeProcessRpc(index,PlayerData.Instance.UserId);
    }

    private void ButtonClear()
    {
        LeftButton.GetComponent<Image>().color = color;
        RightButton.GetComponent<Image>().color = color;
    }

    public void OnClicked(string userName, int index)
    {
        if (index == 0)
        {
            if (userName.Contains("은빈"))
            {
                leftSelections[0].SetActive(true);
            }
            else if (userName.Contains("민준"))
            {
                leftSelections[1].SetActive(true);
            }
        }   
        else
        {
            if (userName.Contains("은빈"))
            {
                rightSelections[0].SetActive(true);
            }
            else if (userName.Contains("민준"))
            {
                rightSelections[1].SetActive(true);
            }
        }
    }

    
    
    
    
    
    
    // private void TextClear()
    // {
    //     LeftNameText1.text = "";
    //     LeftNameText2.text = "";
    //     
    //     RightNameText1.text = "";
    //     RightNameText2.text = "";
    // }
    //
    // private void TurnOnResultText()
    // {
    //     if (!isShowed)
    //     {
    //         Debug.Log("텍스트 켜짐");
    //         isShowed = true;
    //         foreach (var i in nameText)
    //         {
    //             i.gameObject.SetActive(true);
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("텍스트 꺼짐");
    //         isShowed = false;
    //         foreach (var i in nameText)
    //         {
    //             i.gameObject.SetActive(false);
    //         }
    //     }
    // }
}

