using Cysharp.Threading.Tasks;
using Data;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Profile;

public class ProfileInput : MonoBehaviour
{
    public enum State
    {
        Mine,Ideal
    }

    public static ProfileInput Instance;
    
    public State _state = State.Mine;
    public bool isDone;
    private int _index = 0;

    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject[] Canvases;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private GameObject profileInputDoneCanvas;
     
    delegate void DetailNextButton();
    private DetailNextButton detailNextButton;
    private IDetailNextButton[] detailNextButtons = new IDetailNextButton[5];
    
    //통신 관련 데이터
    public Profile profile = new Profile();

    private NetworkController _networkController;
     
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        rootCanvas.enabled = false;
    }

    private void Start()
    {
        _networkController = GameManager.NetworkController;
        
        profileInputDoneCanvas.SetActive(false);

        detailNextButtons[0] = GetComponent<Canvas1>();
        detailNextButtons[1] = GetComponent<Canvas2>();
        detailNextButtons[2] = GetComponent<Canvas3>();
        detailNextButtons[3] = GetComponent<Canvas4>();
        detailNextButtons[4] = GetComponent<Canvas5>();
        nextButton.onClick.AddListener(ClickNextButton);
    }

    public void Show()
    {
        rootCanvas.enabled = true;
        isDone = false;
    }

    public void Hide()
    {
        rootCanvas.enabled = false;
    }
    
    async void ClickNextButton()
    {
        detailNextButton += detailNextButtons[_index].DetailNextButton;
        detailNextButton();
        detailNextButton -= detailNextButtons[_index].DetailNextButton;
        
        if (_index < Canvases.Length - 1)
        {
            Canvases[_index].SetActive(false);
            _index++;
            Canvases[_index].SetActive(true);
        }
        else
        {
            if (_state == State.Mine)
            {
                Canvases[_index].SetActive(false);
                _index = 3;
                Canvases[_index].SetActive(true);
            }
            else
            {
                ProfileInputDone().Forget();
            }
            
            _state = State.Ideal;
            titleText.text = "내 이상형";
        }
    }
    
    private async UniTaskVoid ProfileInputDone()
    {
        // 프로필 데이터 전송 
        string userId = PlayerData.Instance.UserId;
        string data = JsonConvert.SerializeObject(profile);
        
        var response = await _networkController.PostProfile(userId, data);
        if (response.Code == 200)
        {
            var registerUserResponse = JsonConvert.DeserializeObject<RegisterUserResponse>(response.Body);
            
            // Canvases[_index].SetActive(false);
            // nextButton.gameObject.SetActive(false);
        
            profileInputDoneCanvas.SetActive(true);
            isDone = true;
        }
        else
        {
            Debug.LogWarning("프로필 등록 실패!");
        }
    }
}
