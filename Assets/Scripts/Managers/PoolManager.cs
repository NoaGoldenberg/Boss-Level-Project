using System.Collections.Generic;
using UnityEngine;

namespace Managers.Pool
{
    public class PoolManager
    {
        private Dictionary<PoolName, Pool> _pools = new();
        private Transform _rootPools;
        public PoolManager()
        {
            _rootPools = new GameObject("Pools").transform;
            Object.DontDestroyOnLoad(_rootPools);
        }

        public void InitPool(PoolName poolName, int amount)
        {
            var prefab = Resources.Load<Poolable>(poolName.ToString());
            if (prefab == null)
            {
                Debug.LogError($"Prefab for {poolName} not found in Resources.");
                return;
            }
            InitPool(prefab, amount);
        }
        public void InitPool(Poolable prefab, int amount)
        {

            Transform holder = new GameObject($"{prefab.name} Holder").transform;//add
            holder.transform.SetParent(_rootPools);//add
            holder.transform.localPosition = Vector3.zero;//add

            List<Poolable> lst = new();
            for (int i = 0; i < amount; i++)
            {
                var poolPrefab = Object.Instantiate(prefab);
                poolPrefab.transform.SetParent(holder);
                poolPrefab.gameObject.SetActive(false);
                lst.Add(poolPrefab);
            }

            var pool = new Pool
            {
                AvailablePoolables = new Queue<Poolable>(lst)
            };
            _pools.Add(prefab.PoolName, pool);
        }

        public Poolable GetPoolable(PoolName poolName)
        {
            if (_pools.TryGetValue(poolName, out Pool pool))
            {
                if (pool.AvailablePoolables.TryDequeue(out Poolable poolable))
                {
                    poolable.gameObject.SetActive(true);
                    return poolable;
                }
            }

            Debug.Log($"pool - {poolName} not created");
            return null;
        }

        public void ReturnPoolable(Poolable poolable)
        {
            if (_pools.TryGetValue(poolable.PoolName, out Pool pool))
            {
                pool.AvailablePoolables.Enqueue(poolable);
                poolable.gameObject.SetActive(false);
            }
        }

        public class Pool
        {
            public Queue<Poolable> AvailablePoolables = new();
        }
        
        
        
    }

    public enum PoolName
    {
        None = 0,
        JellyfishAttack = 1,
        ElectricityAttack = 2,
        InkAbility = 3,
        Crabs = 4,
        SharkAttack = 5,
        TurtleAbility = 6
        
    }
    
}