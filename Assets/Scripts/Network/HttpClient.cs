using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class Response
    {
        public long Code;
        public string Body;
    }
    
    public class HttpClient
    {
        private readonly string _domainUri;

        public HttpClient(string domainUri)
        {
            _domainUri = domainUri;
        }

        private async UniTask<Response> RequestAsync(UnityWebRequest req)
        {
            Response response = new Response();

            try
            {
                await req.SendWebRequest();
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogWarning(e);   
            }
            response.Code = req.responseCode;
            response.Body = req.downloadHandler.text;
            
            return response;
        }
        
        
        // GET 요청
        public async UniTask<Response> SendGetRequestAsync(string endpoint)
        {
            string uri = _domainUri + endpoint;

            using UnityWebRequest req = UnityWebRequest.Get(uri);

            Response response = await RequestAsync(req);
            
            Debug.Log(response.Body);
            return response;
        }

        // POST 요청
        public async UniTask<Response> SendPostRequestAsync(string endpoint, string body)
        {
            string uri = _domainUri + endpoint;

            using UnityWebRequest req = UnityWebRequest.Post(uri, body, "application/json");

            Response response = await RequestAsync(req);
            
            Debug.Log(response.Body);
            return response;
        }
        
        // POST 요청 - FormData
        public async UniTask<Response> SendPostRequestAsync(string endPoint, WWWForm form)
        {
            string uri = _domainUri + endPoint;

            using UnityWebRequest req = UnityWebRequest.Post(uri, form);

            Response response = await RequestAsync(req);
            
            Debug.Log(response.Body);
            return response;
        }
    }
}