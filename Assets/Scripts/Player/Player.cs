using System.Collections;
using System;
using Managers;
using Managers.Pool;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        [SerializeField]
        private HealthControl _bossHealth;

        private bool _isDead = false;
        private Rigidbody2D _rigidbody2D;
        private PlayerAnimationController _animationController;
        private bool canShoot = false;
        public static int InkCapsuleCount = 0;
        [SerializeField] private ParticleSystem inkParticleSystemPrefab;
        [SerializeField] private Transform ShootPoint;
        [SerializeField] private BossEnemy _boss; // Reference to the boss
        [SerializeField] private HealthControl _playerHealth;
        [SerializeField] private float cooldown = 4f;
        private DateTime _lastAttackTime = DateTime.UtcNow;
        private Coroutine _selfUpdateCoroutine;
        [SerializeField] private Image cooldownImage; 



        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = 0;
            _animationController = GetComponent<PlayerAnimationController>();
            StartCoroutine(SelfUpdateCoroutine());
            CoreManager.instance.PoolManager.InitPool(PoolName.InkAbility, 5);
            CoreManager.instance.PoolManager.InitPool(PoolName.TurtleAbility, 5);
            _playerHealth = GetComponent<HealthControl>();
            _playerHealth.Init(onDeath, onHit);
        }



        private void onDeath()
        {
            Debug.Log("player onDeath entered");
            if (_isDead) return;
            _isDead = true;
            // _animationController.TriggerDeathAnimation();
            UIManager.UIinstance.BossWon();
            AudioManager.instance.PlayLoseSound();
        }

        private void onHit()
        {
            AudioManager.instance.PlayHitOctopusSound();
        }

        private void StartSelfUpdateCoroutine()
        {
            if (_selfUpdateCoroutine != null)
            {
                StopCoroutine(_selfUpdateCoroutine);
            }

            _selfUpdateCoroutine = StartCoroutine(SelfUpdateCoroutine());
        }


        private IEnumerator SelfUpdateCoroutine()
        {
            while (!_isDead)
            {
                TryMove();
                inkCapsuleCount();

                if (canShoot && Input.GetKeyDown(KeyCode.Space))
                {
                    ShootInk();
                    Debug.Log("shooting");
                }

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    Attack();
                }

                if (Input.GetKeyDown(KeyCode.T))
                {
                    tryShootTurtle();
                }

                yield return null;
            }
        }

        private void inkCapsuleCount()
        {
            if (InkCapsuleCount >= 1)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
            }
        }


        private void TryMove()
        {
            // Read the input from the arrow keys
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
            _rigidbody2D.velocity = movement * moveSpeed;

            // Flip the player based on the direction
            if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally for looking left
            }
            else if (movement.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Normal scale for looking right
            }
        }

        private void ShootInk()
        {
            var inkInstance = (InkAbility)CoreManager.instance.PoolManager.GetPoolable(PoolName.InkAbility);
            if (inkInstance != null)
            {
                inkInstance.transform.position = ShootPoint.position;
                inkInstance.Init();
                InkCapsuleCount--;
            }
        }

        public void tryShootTurtle()
        {
            var timePassed = DateTime.UtcNow - _lastAttackTime;
            var isTimePassed = timePassed >= TimeSpan.FromSeconds(cooldown);
            if (isTimePassed)
            {
                ShootTurtle();
                StartCoroutine(CooldownRoutine(cooldown)); 
            }
        }
        

        private void ShootTurtle()
        {
            var turtleInstance = (TurtleAbility)CoreManager.instance.PoolManager.GetPoolable(PoolName.TurtleAbility);
            if (turtleInstance != null)
            {
                turtleInstance.transform.position = ShootPoint.position;
                turtleInstance.Init();
                _lastAttackTime = DateTime.UtcNow;
            }
        }

        private void Attack()
        {
            _animationController.SetTrigger("Attack");
        }

        public void ResetPlayer(Vector3 newPosition)
        {
            _isDead = false;
            InkCapsuleCount = 0;
            canShoot = false;
            transform.position = newPosition;
            _rigidbody2D.velocity = Vector2.zero;
            _playerHealth.Init(onDeath, onHit); // Reset health
            StartSelfUpdateCoroutine();
        }
        
        private IEnumerator CooldownRoutine(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                cooldownImage.fillAmount = Mathf.Lerp(0, 1, time / duration);
                yield return null;
            }
        }

    }
}
