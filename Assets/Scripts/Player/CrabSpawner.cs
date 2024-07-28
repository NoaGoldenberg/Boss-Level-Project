using UnityEngine;
using System.Collections;
using Managers;
using Managers.Pool;

namespace Gameplay
{
    public class CrabsSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 5f; // Time interval between spawns
        [SerializeField] private Vector2 spawnAreaMin; // Minimum bounds for random spawn area
        [SerializeField] private Vector2 spawnAreaMax;
        [SerializeField] private float capsuleLifetime = 4f;

        private void Awake()
        {
            CoreManager.instance.PoolManager.InitPool(PoolName.Crabs, 5);
            StartCoroutine(SpawnInkCapsules());
        }

        private IEnumerator SpawnInkCapsules()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                SpawnInkCapsule();
            }
        }

        private void SpawnInkCapsule()
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            var inkCapsuleInstance = CoreManager.instance.PoolManager.GetPoolable(PoolName.Crabs) as Crabs;
            if (inkCapsuleInstance != null)
            {
                inkCapsuleInstance.transform.position = spawnPosition;
                StartCoroutine(DestroyAfterTime(inkCapsuleInstance, capsuleLifetime));
            }
        }

        private IEnumerator DestroyAfterTime(Poolable poolable, float time)
        {
            yield return new WaitForSeconds(time);
            CoreManager.instance.PoolManager.ReturnPoolable(poolable);
        }
    }
}