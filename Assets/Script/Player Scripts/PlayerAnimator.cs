using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //References
    Animator animator;
    PlayerMovement pm;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.moveDir != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);

            SpriteDirectionChecker();
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void SpriteDirectionChecker()
    {
        if (pm.lastHorizontalVector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
