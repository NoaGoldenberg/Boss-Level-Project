using UnityEngine;

namespace Gameplay
{
    public class DiverAnimationController : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        public Transform transformSpawn;
        private Animator animator;
        

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        public void UpdateAnimation(Vector2 direction)
        {
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
            // animator.SetFloat("Speed", direction.sqrMagnitude);
        }
    }
}