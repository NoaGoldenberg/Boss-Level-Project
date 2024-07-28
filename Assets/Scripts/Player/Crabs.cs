using System.Collections;
using Managers;
using Managers.Pool;
using UnityEngine;

public class Crabs : Poolable
{

    public void Init()
    {
        
        
    }

    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            CoreManager.instance.PoolManager.ReturnPoolable(this);
            Gameplay.Player.InkCapsuleCount++;
            AudioManager.instance.PlayCrabSound();
        }
    }
}