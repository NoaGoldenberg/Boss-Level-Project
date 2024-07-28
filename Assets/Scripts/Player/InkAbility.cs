using System;
using System.Collections;
using Managers;
using Managers.Pool;
using UnityEngine;

namespace Gameplay
{
    public class InkAbility : Poolable
    {
        private CircleCollider2D _collider;
        [SerializeField] private float _duration = 1.5f;
        [SerializeField] private float _initRadius = 1.9f;
        [SerializeField] private float _endRadius = 2.4f;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Init()
        {
            _collider.radius = _initRadius;
            StartCoroutine(AbilityCoroutine());
        }

        private IEnumerator AbilityCoroutine()
        {
            StartCoroutine(RadiusCoroutine());
            _collider.enabled = true;
            yield return new WaitForSeconds(_duration);
            _collider.enabled = false;
            yield return new WaitForSeconds(0.5f);
            CoreManager.instance.PoolManager.ReturnPoolable(this);
        }

        private IEnumerator RadiusCoroutine()
        {
            float elapsedTime = 0;
            while (elapsedTime < _duration)
            {
                _collider.radius = Mathf.Lerp(_initRadius, _endRadius, elapsedTime /_duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}