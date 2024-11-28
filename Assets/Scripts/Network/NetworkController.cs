using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public class NetworkController
    {
        private readonly HttpClient _httpClient;

        public NetworkController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 1. 회원가입
        public async UniTask<Response> SignUp(string data)
        {
            return await _httpClient.SendPostRequestAsync("/register", data);
        }

        
        // 2. 로그인
        public async UniTask<Response> SignIn(string data)
        {
            return await _httpClient.SendPostRequestAsync("/login", data);
        }

        
        // 3. 프로필 등록
        public async UniTask<Response> PostProfile(string userId, string data)
        {
            return await _httpClient.SendPostRequestAsync($"/user_info/{userId}", data);
        }

                
        // 4. 프로필 조회
        public async UniTask<Response> GetProfile(string userId)
        {
            return await _httpClient.SendGetRequestAsync($"/user_info/{userId}");
        }
        
        
                
        // 5. 간단 프로필 조회
        public async UniTask<Response> GetSimpleProfile(string userId, string otherId)
        {
            return await _httpClient.SendGetRequestAsync($"/simple_profile/{userId}/{otherId}");
        }
        
        
        // 6. 오디오 데이터 전송
        public async UniTask<Response> PostVoiceChatData(WWWForm form)
        {
            // WWWForm form = new WWWForm();
            //
            // form.AddBinaryData("audioFile", data.audioData, data.fileName, "audio/wav");
            // form.AddField("timestamp", data.timestamp);
            // form.AddField("sessionId", data.sessionId);
            // form.AddField("userId", data.userId);

            return await _httpClient.SendPostRequestAsync($"/stt", form);
        }

        

        // 7. 토픽 조회
        public async UniTask<Response> GetTopic(string roomId)
        {
            return await _httpClient.SendGetRequestAsync($"/generate-topic/{roomId}");
        }
        
        
        // 8. 더블룸 대화 요약 조회
        public async UniTask<Response> GetConversationResult(string roomId)
        {
            return await _httpClient.SendGetRequestAsync($"/process-conversation/{roomId}");
        }
        
        
        // 9. 밸런스 게임 주제 생성
        public async UniTask<Response> GenerateBalanceTopic(string data)
        {
            return await _httpClient.SendPostRequestAsync($"/generate_balance_topics", data);
        }
        
        // 10. 밸런스 게임 목록 조회
        public async UniTask<Response> GetBalanceGameList()
        {
            return await _httpClient.SendGetRequestAsync($"/get-balance-games");
        }
        
        // 11. 밸런스 게임 조회
        public async UniTask<Response> GetBalanceGame(string gameId)
        {
            return await _httpClient.SendGetRequestAsync($"/get-balance-game/{gameId}");
        }
        
    }
}






















