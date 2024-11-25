using UnityEngine;

namespace Player
{
    public class AvatarSetter: Singleton<AvatarSetter>
    {
        [SerializeField] private GameObject[] avatarPrefabList;
        
        public void SetAvatar(Transform parent, string id)
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