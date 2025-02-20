using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement characterMovement;
    private Rigidbody rb;
    public void Start()
    {
        animator = GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        animator.SetFloat("CharacterSpeed", rb.linearVelocity.magnitude);
        animator.SetBool("IsGrounded", characterMovement.IsGrounded);
        animator.SetBool("IsDoubleJump", characterMovement.doubleJump);
    }

    //public void LateUpdate()
    //{
    //   UpdateAnimator();
    //}

    //// TODO Fill this in with your animator calls
    //void UpdateAnimator()
    //{
    //    animator.SetFloat("CharacterSpeed", rb.linearVelocity.magnitude);
    //    animator.SetBool("IsGrounded", characterMovement.IsGrounded);
    //}
}
