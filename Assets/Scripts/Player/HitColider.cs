using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using Managers.Pool;
using Managers;

namespace Gameplay
{
    public class HitCollider : MonoBehaviour
    {
        private JellyfishAttackPrefab _bossAbility;
        
        public void OnInit( JellyfishAttackPrefab bossAbility)
        {
            _bossAbility = bossAbility;
        }

        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("hit the player");
                _bossAbility.HitPlayer();
                
            }
            else if (other.CompareTag("Ink"))
            {
                Debug.Log("hit the Ink");
                _bossAbility.HitInk();
                
            }
        }
    }
}
