using System.Collections;
using Managers;
using UnityEngine;

namespace Gameplay
{
    public class ElectricityAttack : BaseBossAbilityPrefab
    {
        [SerializeField] private Transform point1;
        [SerializeField] private Transform point2;
        [SerializeField] private Transform point3;
        [SerializeField] private Transform point4;
        [SerializeField] private float speed = 5.0f;
        private Vector3 startPoint; 
        [SerializeField] private float randomRange = 15.0f;  // Range for random target positions
        private Vector3 randomPoint;

        protected override void OnInit()
        {
            startPoint = _boss.transform.position;
            point1.position = startPoint;
            
            // Generate a random target position for point4
            randomPoint = GetRandomPosition(_boss.position, randomRange);

            // Set initial positions for point2 and point3 based on randomPoint
            point2.position = startPoint;
            point3.position = startPoint;
                
            point4.position = startPoint;

            StartCoroutine(MoveElectric());
        }

        private IEnumerator MoveElectric()
        {
            while (Vector3.Distance(point4.position, randomPoint) > 0.1f)
            {
                if (Vector3.Distance(point4.position, _player.transform.position) < 10)
                {
                    randomPoint = _player.transform.position;
                }
                // Move points towards the random target
                point1.position = _boss.transform.position;
                point2.position = Vector3.Lerp(point3.position, point1.position ,0.5f);
                point3.position = Vector3.Lerp(point3.position, point4.position ,0.5f);
                point4.position = Vector3.MoveTowards(point4.position, randomPoint, speed * Time.deltaTime);

                yield return null;
            }

            // Return to the pool once the points reach the target
            CoreManager.instance.PoolManager.ReturnPoolable(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("hit the player");
                HitPlayer();
            }
        }

        public void HitPlayer()
        {
            _target.TakeDamage(damage);
            CoreManager.instance.PoolManager.ReturnPoolable(this);
        }

        public static Vector3 GetRandomPosition(Vector3 center, float radius)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI); // Random angle in radians
            float distance = radius;    // Random distance within the radius
            float xOffset = Mathf.Cos(angle) * distance;
            float yOffset = Mathf.Sin(angle) * distance;

            return new Vector3(center.x + xOffset, center.y + yOffset, center.z);
        }
    }
}
