using System;
using Cysharp.Threading.Tasks;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DoubleRoomUI: MonoBehaviour
    {
        public CanvasGroup doubleRoomCanvasGroup;
        
        public CanvasGroup topicCanvasGroup;
        public TMP_Text topicText;
        
        public CanvasGroup quitCanvasGroup;

        [Header("Topic")]
        public Button topicButton;

        public CanvasGroup waitTopicCanvasGroup;
        
        public CanvasGroup endingCanvasGroup;
        public Button nextButton;
        public TMP_Text smallTitle;
        public TMP_Text summary;
        public int step;

        public ConversationResult conversationResult;
        public string[] sentimentArr;

        private void Awake()
        {
            Fader.FadeOut(topicCanvasGroup, 0f).Forget();
            Fader.FadeOut(quitCanvasGroup, 0f).Forget();
            Fader.FadeOut(doubleRoomCanvasGroup, 0f).Forget();
            Fader.FadeOut(waitTopicCanvasGroup, 0f).Forget();
            Fader.FadeOut(endingCanvasGroup, 0f).Forget();
        }

        private void Start()
        {
            topicButton.onClick.AddListener(OnClickedTopicButton);
            nextButton.onClick.AddListener(OnClickedNextButton);
        }

        public void ShowResult()
        {
            step = 0;
            Fader.FadeIn(endingCanvasGroup, 0f).Forget();
            Fader.FadeOut(doubleRoomCanvasGroup, 0f).Forget();

            GetResult();
        }

        private async void GetResult()
        {
            // string sessionId = "session11";
            // var conversationResult = await NetworkManager.Instance.MeetingController.GetConversationResult(sessionId);
            //
            //
            // sentimentArr = new string[3];
            // sentimentArr[0] = "[공통점]\n이재윤과 김민준은 서로 설렘이라는 감정을 공유하고,\n두 사람 다 첫 소개팅이라는 경험을 통해 긴장하고 있다는 공통적인 감정을 갖고 있습니다.\n또한, 두 사람 모두 과거에서 가장 잘했다고 생각하는 일이 소개팅에 나오는 것이라고 언급했습니다.";
            // sentimentArr[1] = "[차이점]\n이재윤은 J 유형의 MBTI 성격을 가지고 있으며,\n계획성 있는 하루를 완벽하게 보내는 것을 이상적인 날로 꼽는 반면, \n김민준은 계획 없이 휴식을 취하며 시간을 보내는 것을 선호합니다. \n이는 두 사람의 성격과 가치관에서 상당한 차이를 나타냅니다.";
            // sentimentArr[2] = "[미래에 대한 기대감]\n두 사람은 서로에 대해 매력을 느끼고 있으며, \n특히 이재윤은 김민준의 대화 방식이 편안하다고 느끼고 있습니다. \n대화가 순조롭게 흘러가고 서로의 긍정적인 면을 발견하면서 둘 사이의 관계가 앞으로 더 발전할 것을 기대하고 있습니다.";
            //
            // // sentimentArr = conversationResult.data.sentiment.Split("\n", StringSplitOptions.None);
            //
            // // 분리된 부분 출력
            // foreach (string part in sentimentArr)
            // {
            //     Console.WriteLine(part);
            //     Console.WriteLine("-----"); // 구분선
            // }
            //
            // step = 1;
            // smallTitle.text = "대화 요약";
            // summary.text = conversationResult.data.summary;
        }
        
        private void OnClickedNextButton()
        {
            step++;

            if (step == 2)
            {
                smallTitle.text = "감정 분석";
                summary.text = sentimentArr[0];
            }
            else if (step == 3)
            {
                summary.text = sentimentArr[1];
            }
            else if (step == 4)
            {
                summary.text = sentimentArr[2];
            }

        }

        private void OnClickedTopicButton()
        {
            Fader.FadeIn(waitTopicCanvasGroup, 0.5f);
            GetTopic();
        }

        private async void GetTopic()
        {
            // string sessionId = DoubleRoom.SessionId;
            string sessionId = "session11"; // 테스트용 
            
            Debug.Log($"sessionId => {sessionId}");
            
            // Topic topic = await NetworkManager.Instance.MeetingController.GetTopic(sessionId);
            //
            // Debug.Log($"topic => {topic.topic}");
            //
            // if (topic == null || string.IsNullOrEmpty(topic.topic))
            // { 
            //     topic = new Topic("당신에게 완벽한 날은 어떤 날인가요?");
            // }
            //
            // Fader.FadeOut(waitTopicCanvasGroup, 0.5f);
            //
            // ShowTopicUI(topic);
        }
        
        
        

        private async void ShowTopicUI(Topic topic)
        {
            topicText.text = topic.topic;
            Fader.FadeIn(topicCanvasGroup, 0.5f).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(10f));            
            Fader.FadeOut(topicCanvasGroup, 0.5f).Forget();
        }
        

        public async UniTask ShowAndHideQuitUI(int time)
        {
            Fader.FadeIn(quitCanvasGroup, 1f).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(time)); 
            Fader.FadeOut(quitCanvasGroup, 1f).Forget();
            
            // 캔버스 다 끄기 
            Fader.FadeOut(topicCanvasGroup, 0f).Forget();
            Fader.FadeOut(doubleRoomCanvasGroup, 0f).Forget();
            Fader.FadeOut(waitTopicCanvasGroup, 0f).Forget();
        }
    }
}