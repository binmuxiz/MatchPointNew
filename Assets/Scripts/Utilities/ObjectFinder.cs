using UnityEngine;

namespace Utilities
{
    public static class ObjectFinder
    {
        public static Transform FindChildByName(Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                {
                    return child;
                }
                Transform found = FindChildByName(child, childName); // 재귀 호출
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
    }
}