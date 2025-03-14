using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    private Animator animator;
    public float moveSpeed = 2.0f;
    public float moveDistance = 5.0f;
    private Vector3 startPosition;
    private bool movingForward = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            if (movingForward)
            {
                //animator.SetBool("isMoving", true);
                transform.position = Vector3.MoveTowards(transform.position, startPosition + transform.right * moveDistance, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startPosition + transform.right * moveDistance) < 0.1f)
                {
                    movingForward = false;
                }
            }
            else
            {
                //animator.SetBool("isMoving", true);
                transform.position = Vector3.MoveTowards(transform.position, startPosition - transform.right * moveDistance, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startPosition - transform.right * moveDistance) < 0.1f)
                {
                    movingForward = true;
                }
            }

            yield return null;
        }
    }
}
