using Managers;
using UnityEngine;

namespace Gameplay
{
    public class BossEnemy : BaseEnemy
    {
        [SerializeField]
        private float shootInterval = 1f;
        private float shootTimer;
        private Player player;
        [SerializeField]
        private AutoAttackerComponent autoAttacker;
        private DiverAnimationController animationController;
        private HealthControl playerHealth;
        protected Vector3 _moveDirection;
        protected float _changeDirectionInterval = 3f; // Time interval to change direction
        protected float _timeSinceLastDirectionChange = 0f;
        [SerializeField]
        private Vector2 minBounds; // Minimum x and y boundaries
        [SerializeField]
        private Vector2 maxBounds;

        private void Start()
        {
            player = CoreManager.instance.player;
            autoAttacker = GetComponent<AutoAttackerComponent>();
            animationController = GetComponent<DiverAnimationController>();
            playerHealth = player.GetComponent<HealthControl>();
            
        }
        
        public override void TryMove()
        {
            Vector3 newPosition = transform.position + _moveDirection * moveSpeed * Time.deltaTime;

            // Check if the new position is within the bounds
            if (newPosition.x < minBounds.x || newPosition.x > maxBounds.x)
            {
                _moveDirection.x = -_moveDirection.x; // Reverse direction on the x-axis
                newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            }
            if (newPosition.y < minBounds.y || newPosition.y > maxBounds.y)
            {
                _moveDirection.y = -_moveDirection.y; // Reverse direction on the y-axis
                newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
            }

            transform.position = newPosition;
        }

        private void ShootAtPlayer()
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                if (playerHealth != null)
                {
                    autoAttacker.TryAttack(playerHealth);
                }
            }
        }

        private void Update()
        {
            TryMove();
            ShootAtPlayer();
            _timeSinceLastDirectionChange += Time.deltaTime;
            if (_timeSinceLastDirectionChange >= _changeDirectionInterval)
            {
                ChangeDirection();
                _timeSinceLastDirectionChange = 0f;
            }
        }
        
        protected virtual void ChangeDirection()
        {
            _moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }

        public override void OnHit()
        {
            base.OnHit();
        }
        
        public void ResetBoss(Vector3 newPosition)
        {
            transform.position = newPosition;
            _healthControl.Init(OnDeath, OnHit); // Reset health
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("hit the player");
                playerHealth.TakeDamage(5);
            }
        }
    }
}