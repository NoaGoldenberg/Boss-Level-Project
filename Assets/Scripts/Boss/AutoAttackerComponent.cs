using System;
using Managers;
using Managers.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class AutoAttackerComponent : MonoBehaviour
    {
        [SerializeField]
        private PoolName[] _abilitiesPoolNamesOrder;
        [SerializeField]
        private PoolName[] _abilitiesPoolNames;
        private int _currentIndex;

        [field: SerializeField]
        public Transform RespawnPoint { get; set; }

        [SerializeField]
        private float cooldown = 1f;


        private DateTime _lastAttackTime = DateTime.UtcNow;

        
       

        private void Awake()
        {
            foreach (var poolName in _abilitiesPoolNames)
            {
                CoreManager.instance.PoolManager.InitPool(poolName, 5);
            }

            _currentIndex = 0;
        }

        public void TryAttack(HealthControl target)
        {
            var timePassed = DateTime.UtcNow - _lastAttackTime;
            var isTimePassed = timePassed >= TimeSpan.FromSeconds(cooldown);
            if (isTimePassed)
            {
                _currentIndex++;
                _currentIndex %= _abilitiesPoolNamesOrder.Length; 
                PoolName poolName = _abilitiesPoolNamesOrder [_currentIndex];
                Attack(target, poolName);
            }
        }

        public void Attack(HealthControl target, PoolName poolName)
        {
            _lastAttackTime = DateTime.UtcNow;
            var prefab = (BaseBossAbilityPrefab) CoreManager.instance.PoolManager.GetPoolable(poolName);
            if (prefab != null)
            {
                prefab.transform.position = RespawnPoint.position;
                prefab.Init(target,this.transform );
               
            }
        }
        
        
    }
}