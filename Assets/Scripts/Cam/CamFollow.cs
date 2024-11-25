using UnityEngine;

namespace Cam
{
    public class CamFollow: MonoBehaviour
    {
        public Transform target;

        private void Update()
        {
            transform.position = target.position;
        }
    }
}