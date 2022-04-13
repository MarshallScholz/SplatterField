using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float smooth;
    void Update()
    {
        float fwd = Input.GetAxis("Vertical");
        animator.SetFloat("Forward", Mathf.Abs(fwd), smooth, Time.deltaTime);
        animator.SetFloat("Sense", Mathf.Sign(fwd), smooth, Time.deltaTime);
        animator.SetFloat("Turn", Input.GetAxis("Horizontal"), smooth, Time.deltaTime);

    }

}
