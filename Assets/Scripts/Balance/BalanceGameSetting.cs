using System;
using Network;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class BalanceGameSettingManager : MonoBehaviour
{
   public GameObject BalanceGameSettingButton;
   public GameObject InputPrefferredCanvas;
   public TMP_InputField InputPrefferredText;
   public GameObject Content;
   public  GameObject rBalanceButtonPrefab;
   
   private SendMyPrefer sendMyPrefer;
   public RecommandedBalanceList rBalanceList;
   

   private void Start()
   {
      sendMyPrefer = new SendMyPrefer();
      
   }

   public void BalanceGameButton()
   {
      BalanceGameSettingButton.SetActive(false);
      InputPrefferredCanvas.SetActive(true);
   }

   public async void SendMyPreferButton()
   {
      //sendMyPrefer.user_id = PlayerData.Instance.UserId;
      sendMyPrefer.user_id = "1";
      sendMyPrefer.topic = InputPrefferredText.text;
      string temp = JsonConvert.SerializeObject(sendMyPrefer);
      //Debug.Log(GameManager.NetworkController);
      
      Response response = await GameManager.NetworkController.GenerateBalanceTopic(temp);
      string s = response.Body;
      Debug.Log(s);
      rBalanceList = JsonConvert.DeserializeObject<RecommandedBalanceList>(s);
   }

   public void SpawnrBalanceListButton()
   {
      for (int i = 0; i < rBalanceList.options.Count; i++)
      {
         GameObject temp = Instantiate(rBalanceButtonPrefab, Content.transform, false);
         temp.GetComponent<TMP_Text>().text = rBalanceList.options[i].left + " vs " + rBalanceList.options[i].right;
      }
   }
}
