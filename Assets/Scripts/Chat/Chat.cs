// using ExitGames.Client.Photon;
// using Photon.Chat;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;
//
// public class Chat : MonoBehaviour, IChatClientListener
// {
//     [SerializeField] private bool isChat;
//     
//     [SerializeField] private string chatAppId = "0f96bfde-317c-4069-a6a7-a5522a67f87f";
//     [SerializeField] private string appVersion = "disgustringPhoton";
//     [SerializeField] private ChatClient chatClient;
//     [SerializeField] private string userName;
//     [SerializeField] private string currentChannelName;
//     
//     // 메세지 보관 리스트
//     private Stack<GameObject> messageAreas = new Stack<GameObject>();
//     
//     //채팅 창
//     public GameObject ScrollChatView;
//     public GameObject MyTextArea;
//     public GameObject OtherTextArea;
//     public  RectTransform Content;
//     public TMP_InputField InputField;
//
//     
//     //채팅 연결
//     public void Connect(string userName)
//     {
//         
//         UserName = userName;
//         currentChannelName = "World Channel";
//         chatClient = new ChatClient(this);
//         chatClient.Connect(chatAppId, "1.0", new AuthenticationValues(UserName));
//         chatClient.ChatRegion = "ASIA";
//     }
//
//     private string UserName
//     {
//         get
//         {
//             return userName;
//         }
//          set
//         {
//             if (!string.IsNullOrEmpty(userName)) return;
//             userName = value.Trim();
//         }
//     }
//
//     public void OnApplicationQuit()
//     {
//         if (chatClient != null)
//         {
//             chatClient.Disconnect();
//         }
//     }
//     
//     
//     private void InstantiateMessage(string senderName,string message)
//     {
//         Debug.Log($"보내는 이 : {senderName}");
//         messageAreas.Push(Instantiate(senderName.Equals(UserName) ? MyTextArea : OtherTextArea));
//         
//         GameObject temp = messageAreas.Peek();
//         var area = temp.GetComponent<Area>();
//         
//         if(!senderName.Equals(UserName))
//             area.UserText.text = senderName;
//         
//         //메세지 UI 스크롤 뷰에 등록
//         temp.transform.SetParent(Content.transform,false);
//         
//         area.BoxRect.sizeDelta = new Vector2(200, area.BoxRect.sizeDelta.y);
//         area.TextRect.GetComponent<TMP_Text>().text = message;
//         
//         
//         //UI 생성 후 업데이트
//         LayoutRebuilder.ForceRebuildLayoutImmediate(area.TextRect);
//         LayoutRebuilder.ForceRebuildLayoutImmediate(area.BoxRect);
//         LayoutRebuilder.ForceRebuildLayoutImmediate(area.AreaRect);
//         Canvas.ForceUpdateCanvases();
//     }
//     
//     //엔터키 눌렀을 때
//     public void Input_OnEndEdit(string text)
//     {
//         Debug.Log(currentChannelName);
//         if (chatClient.State == ChatState.ConnectedToFrontEnd)
//         {
//             //outputText.text = InputField.text;
//             chatClient.PublishMessage(currentChannelName, InputField.text);
//             InputField.text = "";
//         }
//     }
//     
//     
//     public void Update()
//     {
//         if (this.chatClient != null)
//         {
//             this.chatClient.Service(); 
//         }
//     }
//     
//     
//     public void DebugReturn(DebugLevel level, string message)
//     {
//         if(level == ExitGames.Client.Photon.DebugLevel.ERROR)
//             Debug.LogError(message);
//         else if (level == DebugLevel.WARNING)
//         {
//             Debug.LogWarning(message);
//         }
//         else
//         {
//             Debug.Log(message);
//         }
//     }
//     
//     public void OnDisconnected()
//     {
//         Debug.Log("연결 끊음");
//     }
//
//     public void OnConnected ()
//     {
//         Debug.Log("서버에 연결되었습니다");
//
//         chatClient.Subscribe(new string[]{currentChannelName}, 10);
//     }
//
//     public void OnChatStateChange(ChatState state)
//     {
//         Debug.Log("상태 변경 : " + state);
//     }
//
//     public void OnGetMessages(string channelName, string[] senders, object[] messages)
//     {
//         Debug.Log(channelName);
//         for (int i = 0; i < messages.Length; i++)
//         {
//             Debug.Log(senders[i]);
//             Debug.Log(messages[i].ToString());
//
//             InstantiateMessage(senders[i], messages[i].ToString());
//         }
//     }
//     
//     public void OnPrivateMessage(string sender, object message, string channelName)
//     {
//         Debug.Log("OnPrivateMessage : " + message);
//     }
//
//     public void OnSubscribed(string[] channels, bool[] results)
//     {
//         Debug.Log("채널 입장");
//         foreach(var i in channels) Debug.Log(i);
//     }
//
//     public void OnUnsubscribed(string[] channels)
//     {
//         Debug.Log("채널 퇴장");
//         foreach(var i in channels) Debug.Log(i);
//     }
//
//     public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
//     {
//         Debug.Log("상태 변경 " + user + status +  gotMessage + message);
//     }
//
//     public void OnUserSubscribed(string channel, string user)
//     {
//         throw new System.NotImplementedException();
//     }
//
//     public void OnUserUnsubscribed(string channel, string user)
//     {
//         throw new System.NotImplementedException();
//     }
//
//     
// }
