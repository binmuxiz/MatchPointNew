using System;
using Network;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BalanceGameSettingManager : MonoBehaviour
{
   public GameObject BalanceGameSettingButton;
   public GameObject InputPrefferredCanvas;
   public GameObject RBalanceButtonCanvas;
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
      sendMyPrefer.user_id = PlayerData.Instance.UserId;
      //sendMyPrefer.user_id = "1";
      sendMyPrefer.topic = InputPrefferredText.text;
      string temp = JsonConvert.SerializeObject(sendMyPrefer);
      //Debug.Log(GameManager.NetworkController);
      
      Response response = await GameManager.NetworkController.GenerateBalanceTopic(temp);
      string s = response.Body;
      Debug.Log(s);
      rBalanceList = JsonConvert.DeserializeObject<RecommandedBalanceList>(s);
      InputPrefferredCanvas.SetActive(false);
      RBalanceButtonCanvas.SetActive(true);
      SpawnrBalanceListButton();
   }

   public void SpawnrBalanceListButton()
   {
      for (int i = 0; i < rBalanceList.options.Count; i++)
      {
         bool isPressed = false;
         GameObject temp = Instantiate(rBalanceButtonPrefab, Content.transform, false);
         temp.GetComponentInChildren<TMP_Text>().text = rBalanceList.options[i].left + " vs " + rBalanceList.options[i].right;
         temp.GetComponent<Button>().onClick.AddListener(() => rBalanceButton(ref isPressed,temp.GetComponent<Image>()));
      }
   }

   public void rBalanceButton(ref bool isPressed,Image buttonImage)
   {
      if (!isPressed)
      {
         isPressed = true;
         Debug.Log("선택됨");
         buttonImage.color = Color.gray;
      }
      else
      {
         Debug.Log("선택 해제");
         isPressed = false;
         buttonImage.color = Color.white;
      }

   }

   public void SettingEndButton()
   {
      RBalanceButtonCanvas.SetActive(false);
   }
}
