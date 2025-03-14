using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Animator animator;
    private TopDownCarController car;
    private SlimeMovement slimeMovement;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        car = FindObjectOfType<TopDownCarController>();
        slimeMovement = GetComponent<SlimeMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isPlayer = other.gameObject.CompareTag("Player");
        bool isNPC    = other.gameObject.CompareTag("NPC");
        if (isPlayer || isNPC)
        {
            Debug.Log("Slime hit! Slowing down...");
            StartCoroutine(SlowDownOnSlime());
            Die();
        }
    }

    // Call this method to trigger the death animation and then back to idle
    public void Die()
    {
        Debug.Log("Die() called");
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        Debug.Log("DieCoroutine started");
        // Stop the slime movement
        if (slimeMovement != null)
        {
            Debug.Log("Disabling SlimeMovement");
            //slimeMovement.enabled = false;
            animator.SetBool("isMoving", false); // Stop movement animation
        }

        // Trigger the death animation
        animator.SetBool("isDead", true);
        Debug.Log("isDead set to true");

        // Wait for the death animation to finish
        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deathAnimationLength);
        Debug.Log($"Death animation length: {deathAnimationLength}");
        // Additional wait time before going back to idle
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Additional wait time completed");
        Debug.Log("isDead set to false");

        // Set back to idle
        animator.SetBool("isDead", false);

        // Restart the slime movement
        if (slimeMovement != null)
        {
            Debug.Log("Enabling SlimeMovement");
            //slimeMovement.enabled = true;
            animator.SetBool("isMoving", true); // Resume movement animation
        }
        Debug.Log("DieCoroutine ended");
    }

    private IEnumerator SlowDownOnSlime()
    {
        float originalMaxSpeed = car.maxSpeed;
        float originalAccelerationFactor = car.accelerationFactor;

        car.maxSpeed = car.slimeMaxSpeed;
        car.accelerationFactor = car.slimeAccelerationFactor;

        yield return new WaitForSeconds(car.slimeSlowDuration);

        car.maxSpeed = originalMaxSpeed;
        car.accelerationFactor = originalAccelerationFactor;
    }
}
