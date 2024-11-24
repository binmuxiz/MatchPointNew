using System.Text;
using Cysharp.Threading.Tasks;
using Network;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyRoom: MonoBehaviour
{
    public World world;
    
    public GameObject avatar;
    
    [SerializeField] private CanvasGroup myRoomCanvasGroup;
    [SerializeField] private Button closeButton;

    public Data.Profile _profile;


    public TMP_Text nameGenderMbti;
    public TMP_Text birthDate;
    public TMP_Text address;

    public TMP_Text info1;
    public TMP_Text info2;
    
    public TMP_Text personality;
    public TMP_Text appearance;
    public TMP_Text skills;
    public TMP_Text hobbies;
    
    // 이상형
    public TMP_Text idealPersonality;
    public TMP_Text idealAppearance;
    public TMP_Text idealSkills;
    public TMP_Text idealHobbies;


    private NetworkController _networkController;
            
    private void SetProfileToUI()
    {
        Debug.Log("마이룸 데이터 바인딩");

        nameGenderMbti.text = $"{_profile.user.name} ({_profile.user.gender})  {_profile.user.mbti}";
        birthDate.text = _profile.user.birthDate;
        address.text = _profile.user.address;

        string edu = _profile.user.education;
        string div = _profile.user.divorced;
        string rel = _profile.user.religion;
        string smo = _profile.user.smoking;
        string dri = _profile.user.drinking;
        
        string height =  _profile.user.height.ToString();
        string bodytype = _profile.user.bodyType;
        
        info1.text = $"{edu}, {div}, {rel}, [흡연]{smo}, [음주]{dri}";
        info2.text = $"[키]{height}cm, [체형]{bodytype}";
     
        StringBuilder sb = new StringBuilder();
        personality.text = GetStr(sb, _profile.user.personality);
        appearance.text = GetStr(sb, _profile.user.appearance);
        skills.text = GetStr(sb, _profile.user.skills);
        hobbies.text = GetStr(sb, _profile.user.hobbies);
        
        //이상형 
        idealPersonality.text = GetStr(sb, _profile.idealType.personality);
        idealAppearance.text = GetStr(sb, _profile.idealType.appearance);
        idealSkills.text = GetStr(sb, _profile.idealType.skills);
        idealHobbies.text = GetStr(sb, _profile.idealType.hobbies);
        sb.Clear();
    }

    private string GetStr(StringBuilder sb, string[] strArr)
    {
        sb.Clear();
        foreach (var str in strArr)
        {
            sb.Append($"#{str}  ");
        }
        return sb.ToString();
    }

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseMyRoomUI);
    }

    

    private void Start()
    {
        _networkController = GameManager.NetworkController;
        
        Fader.FadeOut(myRoomCanvasGroup);
    }

    public async void ShowMyRoomUI()
    {
        await SetProfile();
        avatar.SetActive(true);
    }

    private async UniTask SetProfile()
    {
        string userId = PlayerData.Instance.UserId;
        
        var response = await _networkController.GetProfile(userId);

        if (response.Code == 200)
        {
            _profile = JsonConvert.DeserializeObject<Data.Profile>(response.Body);
            if (_profile != null) SetProfileToUI();
        
            Fader.FadeIn(myRoomCanvasGroup);
        }
        else
        {
            Debug.LogWarning(response.Code);
            Debug.LogWarning(response.Body);
        }
    }
    
    
    private void CloseMyRoomUI()
    {
        Fader.FadeOut(myRoomCanvasGroup);
        avatar.SetActive(false);
        
        // world.ShowMainUI();
    }
}
