using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Gameplay
{
    public class JellyfishAttackPrefab : BaseBossAbilityPrefab
    {
        private Coroutine _moveCoroutine;
        [SerializeField] private HitCollider hitCollider;
        protected override void OnInit()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveCoroutine());
            // hitCollider.OnInit( this);
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