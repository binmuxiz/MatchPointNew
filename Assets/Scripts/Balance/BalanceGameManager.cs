using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalanceGameManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject TopicPrefab;
    public GameObject BalanceGameButton;
    public GameObject TopicCanvas;
    
    public BalanceTopicList balanceTopicList;
    public RealBalanceGameList realBalanceGameList;

    
    public async void StartBalanceGameButton()
    {
        BalanceGameButton.SetActive(false);
        
        var response = await GameManager.NetworkController.GetBalanceGameList();
        var s = response.Body;
        
        Debug.Log(s);
        
        balanceTopicList = JsonConvert.DeserializeObject<BalanceTopicList>(s);
        
        ShowBalanceTopicListButton();
    }

    public void ShowBalanceTopicListButton()
    {
        TopicCanvas.SetActive(true);
        
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
         Debug.Log("index : " + index);
        GetRealBalanceList(balanceTopicList.triples[index].topic_id);
    }

    public async void GetRealBalanceList(string topic_id)
    {
        var response = await GameManager.NetworkController.GetBalanceGame(topic_id);
        string temp = response.Body;
        Debug.Log(temp);
        realBalanceGameList = JsonConvert.DeserializeObject<RealBalanceGameList>(temp);
    }
    
    
}
