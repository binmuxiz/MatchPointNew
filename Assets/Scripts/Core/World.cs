using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class World: Singleton<World>
{

    [Header("UI")] 
    public GameObject canvas;
    [SerializeField] private RectTransform topBar;  //상단바 

    [Header("My Info")] 
    [SerializeField] private Image profileImage;
    [SerializeField] private TMP_Text myNameText;

    [Header("ProfileSprites")] 
    [SerializeField] private Sprite[] profileSprites;  //은빈, 민주, 민준, 동섭 순 

    
    private void Awake()
    {
        if (profileSprites.Length != 4)
        {
            Debug.LogError("프로필 스프라이트 초기화 안함");
        }
    }
        
        
    private void Start()
    {
        HideWorldUI();
    }


    

    public void Enter()
    {
        canvas.SetActive(true);

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
        Sprite profileSprite = profileSprites[0];
        string userId = PlayerData.Instance.UserId;
        switch (userId)
        {
            case "1":
                profileSprite = profileSprites[0];
                break;
            case "2":
                profileSprite = profileSprites[1];
                break;
            case "3":
                profileSprite = profileSprites[2];
                break;
            case "4":
                profileSprite = profileSprites[3];
                break;
            default:
                profileSprite = profileSprites[0];
                break;
        }

        profileImage.sprite = profileSprite;
        myNameText.text = PlayerData.Instance.Profile.user.name;
    }

    private void ShowWorldUI()
    {
        canvas.SetActive(true);
        
        // 목표 위치로 이동 (예: y축 0)
        topBar.DOAnchorPos(new Vector2(0, 0), 0.5f)
            .SetEase(Ease.OutBounce); // 애니메이션 이징 설정
    }

    private void HideWorldUI()
    {
        canvas.SetActive(false);
        topBar.anchoredPosition = new Vector2(0, 115);
    }
}
