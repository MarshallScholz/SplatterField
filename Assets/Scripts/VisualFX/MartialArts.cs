using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartialArts : MonoBehaviour
{
    public Animator animator;
    public GameObject punchFX;
    public GameObject kickFX;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetBool("Punch", true);
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                animator.SetBool("PunchLower", true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
            animator.SetBool("Kick", true);

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("Shoot", true);
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                animator.SetBool("ShootLower", true);
        }
    }

    void EndAnimations()
    {
        animator.SetBool("Punch", false);
        animator.SetBool("PunchLower", false);

        animator.SetBool("Kick", false);

        animator.SetBool("Shoot", false);
        animator.SetBool("ShootLower", false);
    }
}
