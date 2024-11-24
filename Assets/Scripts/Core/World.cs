using Data;
using DG.Tweening;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class World: Singleton<World>
{
    private const string WorldSessionName = "World";
    private const int WorldMaxPlayerCount = 20;

    public Transform WorldSpawnPosition;
    [SerializeField] private GameObject runnerPrefab;

    [SerializeField] private MyRoom myRoom;

    
    [Header("UI")] 
    public GameObject worldUIGO;
    [SerializeField] private RectTransform topBar;  //상단바 

    [Header("My Info")] 
    [SerializeField] private Image profileImage;
    [SerializeField] private TMP_Text myNameText;

    [Header("ProfileSprites")] 
    [SerializeField] private Sprite[] userSprites;  //은빈, 민주, 민준, 동섭 순 

    [Header("Buttons")] 
    public Button profileButton; 
    public Button chatButton;
    
    
    private void Awake()
    {
        //버튼 리스너 연결 
    }
        
        
    private void Start()
    {
        worldUIGO.SetActive(false);
        topBar.anchoredPosition = new Vector2(0, 115);
    }


    

    public async void Enter()
    {
        var args = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = WorldSessionName,
            PlayerCount = WorldMaxPlayerCount,
        };

        // session 생성/접속 및 플레이어 스폰
        await SessionManager.Instance.StartSessionAsync(args, runnerPrefab);
        Debug.Log("접속 및 플레이어 스폰 완료");

        worldUIGO.SetActive(true);

        SetProfile();
        ShowWorldUI();
        Debug.Log("월드 UI 세팅 완료");
    }
    
    
    private void SetProfile()
    {
        if (PlayerData.Instance.UserId == null)
        {
            Debug.LogError("프로필 정보가 없습니다.");
            return;
        }
        
        // TODO ID별로 서로 다른 이미지 삽입
        
        myNameText.text = PlayerData.Instance.Profile.user.name;
    }

    private void ShowWorldUI()
    {
        worldUIGO.SetActive(true);
        
        // 목표 위치로 이동 (예: y축 0)
        topBar.DOAnchorPos(new Vector2(0, 0), 0.5f)
            .SetEase(Ease.OutBounce); // 애니메이션 이징 설정
    }
}
