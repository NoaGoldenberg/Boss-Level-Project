using System;
using UnityEngine;

namespace Gameplay
{
    public class HealthControl : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;
        [SerializeField] private HealthBar healthBar;

       public int _currentHealth;
        private Action _onDeath;
        private Action _onHit;
        

        public void Init(Action onDeath = null, Action onHit = null)
        {
            _currentHealth = maxHealth;
            healthBar.SetHealth(_currentHealth);
            _onDeath = onDeath;
            _onHit = onHit;
        
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            healthBar.SetHealth(_currentHealth);
            _onHit?.Invoke();
            Debug.Log($"Health: {_currentHealth}");
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Dead!");
            if (_onDeath == null)
            {
                Debug.Log("no ondeath func");
            }
            _onDeath?.Invoke();
        }
    }
}