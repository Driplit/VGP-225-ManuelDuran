using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent agent; // Used for movement detection
    private bool isAttacking = false;

    void Update()
    {
        HandleMovementAnimation();
    }

    // Handle movement animation
    private void HandleMovementAnimation()
    {
        if (isAttacking) return; // Don't change animation while attacking

        float speed = agent.velocity.magnitude;

        if (speed > 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    // Call this function when the enemy attacks
    public void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        StartCoroutine(ResetAttack()); // Ensure animation resets
    }

    // Call this function when the enemy gets hit
    public void TakeHit()
    {
        anim.SetTrigger("Hit");
    }

    // Reset attack state after animation plays
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f); // Adjust based on attack animation length
        isAttacking = false;
    }
}