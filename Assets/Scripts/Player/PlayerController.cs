using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 40;
    [SerializeField] private float gravity = -9.81f;  // 중력 값 설정
    [SerializeField] private float verticalVelocity = 0;  // 현재 캐릭터의 수직 속도
    
    public Vector3 direction;
    public CharacterController cc;
    public Animator animator;
    
    
    [SerializeField] private GameObject manCharacterPrefab;
    [SerializeField] private GameObject womanCharacterPrefab;
    

    private Vector3 inputDirection;


    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    

    void Update()
    {
        if (!HasStateAuthority) return;
    
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (animator != null)
        {
            bool isMoving = inputDirection != Vector3.zero;
            animator.SetBool("IsWalking", isMoving);
        }
    }
    
    
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        // 이동 방향 설정
        direction = inputDirection;

        // 중력 적용
        if (cc.isGrounded)
        {
            verticalVelocity = 0;  // 지면에 붙어있으면 수직 속도를 0으로 설정
        }
        else
        {
            verticalVelocity += gravity * RunnerController.Runner.DeltaTime;  // 중력 적용
        }

        Vector3 move = direction * speed * RunnerController.Runner.DeltaTime;
        move.y = verticalVelocity;  // 수직 속도를 이동 벡터에 추가
        cc.Move(move);  // 이동

        // 회전 (움직임이 있을 때만 회전)
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RunnerController.Runner.DeltaTime * 10f);  // 부드러운 회전
        }
    }
    

    public override void Spawned()
    {
        GetComponent<NetworkTransform>().enabled = true;
        
        if (HasStateAuthority) {
            switch (GameManager.Instance.GameState)
            {
                case GameState.World:
                    StartCoroutine(CameraController.Instance.SetCinemachineCameraTarget(transform));
                    break;
                case GameState.Group:
                    CameraController.Instance.SetGroupMeetingRoomCamera(); 
                    break;
            }
        }
        
        SetAvatar().Forget(); // 아바타 설정 
    }
    
    
    
    
    private async UniTask SetAvatar()
    {
        var sh = GetComponent<SharedData>();

        if (string.IsNullOrEmpty(sh.Gender))
        {
            await UniTask.WaitUntil(() => !string.IsNullOrEmpty(sh.Gender));
        }

        
        GameObject avatarPrefab = sh.Gender == "여자" ? womanCharacterPrefab : manCharacterPrefab;
        
        if (avatarPrefab != null)
        {
            GameObject avatar = Instantiate(avatarPrefab, gameObject.transform);

            Debug.Log($"{sh.Gender} 아바타 생성 완료");
            
            // 예: Animator 초기화
            animator = avatar.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Rebind();  // Animator 상태 초기화
            }
        }
        else
        {
            Debug.LogWarning("아바타 프리팹이 설정되지 않았습니다.");
        }
    }


}
