using System;
using System.Collections;
using Gameplay;
using Managers;
using Managers.Pool;
using UnityEngine;

public class TurtleAbility : Poolable
{
    [SerializeField] private float _duration = 4f;
    [SerializeField] private float _speed = 5f; // Speed at which the turtle moves
    private Transform _target;
    private HealthControl _healthControl;// Target for the turtle to move towards
    private Action _onHit;
    private BossEnemy _boss;

    public void Init()
    {
        _boss = CoreManager.instance.boss;
        _healthControl = _boss.GetComponent<HealthControl>();
        _target = _boss.transform;
        StartCoroutine(AbilityCoroutine());
    }

    private IEnumerator AbilityCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _duration)
        {
            if (_target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        CoreManager.instance.PoolManager.ReturnPoolable(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            _healthControl.TakeDamage(10);
            Debug.Log("Turtle hit the boss!");
            CoreManager.instance.PoolManager.ReturnPoolable(this);
        }
    }
}