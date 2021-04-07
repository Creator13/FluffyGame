using UnityEngine;

namespace Fluffy
{
    [RequireComponent(typeof(Animator))]
    public class RobinController : MonoBehaviour
    {
        private static readonly int Pick = Animator.StringToHash("Pick");
        private static readonly int Sit = Animator.StringToHash("Sit");
        
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void DoPickup()
        {
            animator.SetTrigger(Pick);
        }

        public void SetSit(bool sit)
        {
            animator.SetBool(Sit, sit);
        }

        public void Move(Vector3 position)
        {
            transform.position = position;
        }
    }
}
