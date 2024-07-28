using System.Collections;
using Managers;
using Managers.Pool;
using UnityEngine;

namespace Gameplay
{
    public class BaseBossAbilityPrefab : Poolable
    {
        protected Vector3 _initialDirection;
        [SerializeField] protected int damage;
        protected HealthControl _target; 
        protected Player _player;
        [SerializeField] private float _duration;
        [SerializeField] protected int _speed = 20;
        protected Transform _boss;

        
        private IEnumerator ReturnAfterTime()
        {
            yield return new WaitForSeconds(_duration);
            CoreManager.instance.PoolManager.ReturnPoolable(this);
        }
        
        public void Init(HealthControl target,Transform boss)
        {
            _boss = boss;
            _target = target;
            _player = CoreManager.instance.player;
            _initialDirection = (target.transform.position - transform.position).normalized;
            StartCoroutine(ReturnAfterTime());
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }
        
    }
}