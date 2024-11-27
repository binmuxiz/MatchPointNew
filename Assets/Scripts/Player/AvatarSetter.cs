using System;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Player
{
    public class AvatarSetter: Singleton<AvatarSetter>
    {
        
        [SerializeField] private GameObject[] avatarPrefabList;
        public GameObject NameTag;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public async void SetAvatar(Transform parent, string id, string name)
        {
            GameObject avatarPrefab = null;

            switch (id)
            {
                case "1":
                    avatarPrefab = avatarPrefabList[0];
                    break;
                case "2":
                    avatarPrefab = avatarPrefabList[1];
                    break;
                case "3":
                    avatarPrefab = avatarPrefabList[2];
                    break;
                case "4":
                    avatarPrefab = avatarPrefabList[3];
                    break;
                default:
                    avatarPrefab = avatarPrefabList[0];
                    break;
            }
        
        
            if (avatarPrefab != null)
            {
                GameObject avatar = Instantiate(avatarPrefab, parent);

                var tpv = avatar.GetComponentInParent<ThirdPersonView>();
                tpv.animator = avatar.GetComponentInChildren<Animator>();
                
                NetworkObject nameTag = await RunnerController.Runner.SpawnAsync(NameTag, parent.position + 2*Vector3.up);
                // 이름표를 생성한 후 에디터 전용 플래그를 해제

                nameTag.transform.SetParent(avatar.transform);
                await UniTask.WaitUntil(() => SharedData.Instance);
                SharedData.Instance.SyncNameTagRpc(nameTag, name);

                // parent.position = new Vector3(parent.position.x, 1, parent.position.z);


                // 예: Animator 초기화
                // animator = avatar.GetComponent<Animator>();
                // if (animator != null)
                // {
                //     animator.Rebind();  // Animator 상태 초기화
                // }
            }
            else
            {
                Debug.LogWarning("아바타 프리팹이 설정되지 않았습니다.");
            }
        }
    }
}