using UnityEngine;
using Managers;

namespace Gameplay
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField]
        protected float moveSpeed = 5f;
        [SerializeField]
        protected float minDistToPlayer = 1f;
        [SerializeField] public HealthControl _healthControl;
        protected Player _player;
        
        public virtual void Init()
        {
            _player = CoreManager.instance.player;
            _healthControl = GetComponent<HealthControl>();
            _healthControl.Init(OnDeath, OnHit);
        }
        
        private void Update()
        {
            TryMove();
        }
        

        public virtual void TryMove()
        {
            
        }

        public virtual void OnDeath()
        {
            UIManager.UIinstance.OctopusWon();
            AudioManager.instance.PlayWinOctopusSound();
            
        }
        
        public virtual void OnHit()
        {
            AudioManager.instance.PlayHitJellyfishSound();
        }
        
        public virtual bool IsPlayerInRange()
        {
            return Vector3.Distance(transform.position, _player.transform.position) < minDistToPlayer;
        }
    }
}