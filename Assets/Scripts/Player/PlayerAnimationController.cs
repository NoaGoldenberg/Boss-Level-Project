using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void TriggerDamageAnimation()
    {
        animator.SetBool("isDamaged", true);
        StartCoroutine(ResetDamageAnimation());
    }

    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Adjust time as needed
        animator.SetTrigger("isDamaged");
    }

    public void TriggerDeathAnimation()
    {
        animator.SetTrigger("isDead");
    }
    
    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}