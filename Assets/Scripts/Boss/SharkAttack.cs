using System.Collections;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class SharkAttack3 : BaseBossAbilityPrefab
    {
        private Coroutine _moveCoroutine;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        [SerializeField] private Sprite leftSprite;
        [SerializeField] private Sprite rightSprite;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnInit()
        {
            // Choose a random spawn point
            Vector2 chosenSpawnPoint = GetRandomSpawnPoint();
            Debug.Log($"Chosen Spawn Point: {chosenSpawnPoint}");
            transform.position = new Vector3(chosenSpawnPoint.x, chosenSpawnPoint.y, transform.position.z);

            _initialDirection = (_target.transform.position - transform.position).normalized;

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }
        
        private Vector2 GetRandomSpawnPoint()
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            return new Vector2(x, y);
        }

        private IEnumerator MoveCoroutine()
        {
            while (true)
            {
                Move();
                yield return new WaitForFixedUpdate();
            }
        }

        private void Move()
        {
            var playerPos = _player.transform.position;
            var direction = (playerPos - transform.position).normalized;
            transform.position += direction * (_speed * Time.deltaTime);
            
            UpdateSpriteDirection(direction);
        }

        private void UpdateSpriteDirection(Vector2 direction)
        {
            if (direction.x < 0)
            {
                spriteRenderer.sprite = leftSprite;
            }
            else
            {
                spriteRenderer.sprite = rightSprite;
            }
        }

        public void HitPlayer()
        {
            _target.TakeDamage(damage);
            CoreManager.instance.PoolManager.ReturnPoolable(this);
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
        }

        public void HitInk()
        {
            AudioManager.instance.PlayHitAttackSound();
            CoreManager.instance.PoolManager.ReturnPoolable(this);
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("hit the player");
                HitPlayer();
            }
            else if (other.CompareTag("Ink"))
            {
                Debug.Log("hit the Ink");
                HitInk();
            }
        }
    }
}
