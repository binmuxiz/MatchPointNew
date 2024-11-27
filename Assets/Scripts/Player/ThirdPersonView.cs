using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;

namespace Player
{
    public class ThirdPersonView : NetworkBehaviour
    {
        public float speed = 140;        // 이동 속도
        public float gravity = -9.81f;   // 중력 값 설정
        public float verticalVelocity = 0;  // 현재 캐릭터의 수직 속도
        public float rotationSpeed = 4; // 회전 속도 
        
        public Vector3 direction;

        public SimpleKCC SimpleKcc;
        
        private Vector3 inputDirection;
        private float inputMouseX;
        
        public Animator animator;


        private void Awake()
        {
            Camera.main.gameObject.SetActive(true);
            SimpleKcc = GetComponent<SimpleKCC>();
        }

        void Update()
        {
            if (!HasStateAuthority) return;
        
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            // inputDirection = Camera.main.transform.TransformDirection(inputDirection);
            // inputMouseX = Input.GetAxis("Mouse X");

            // 애니메이션 
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
            if (SimpleKcc.IsGrounded)
            {
                verticalVelocity = 0;  // 지면에 붙어있으면 수직 속도를 0으로 설정
            }
            else
            {
                verticalVelocity += gravity * RunnerController.Runner.DeltaTime;  // 중력 적용
            }

            Vector3 move = direction * speed * RunnerController.Runner.DeltaTime;
            move.y = verticalVelocity;  // 수직 속도를 이동 벡터에 추가
            SimpleKcc.Move(move);  // 이동

            // // 회전 (움직임이 있을 때만 회전)
            if (direction != Vector3.zero)
            {
                SimpleKcc.SetLookRotation(new Vector2(0f, 90f + Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg));
            }
            
            // 회전
            //SimpleKcc.AddLookRotation(0f, inputMouseX * rotationSpeed);
        }

        public override void Spawned()
        {
            if (HasStateAuthority)
            { 
                CameraController.Instance.SetWorldCamera(transform); 
            }
        }
    }
}