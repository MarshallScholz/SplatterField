using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float smooth;

    // public GameObject gameObject;
    public GameObject marker;
    public LayerMask ignoreLayer;
    public ParticleSystem particleSystem;

    void Start()
    {
        ignoreLayer = LayerMask.GetMask("Ignore Raycast");
    }
    void Update()
    {
        float fwd = Input.GetAxis("Vertical");
        animator.SetFloat("Forward", Mathf.Abs(fwd), smooth, Time.deltaTime);
        animator.SetFloat("Sense", Mathf.Sign(fwd), smooth, Time.deltaTime);
        animator.SetFloat("Turn", Input.GetAxis("Horizontal"), smooth, Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve(1.0f, 2.0f);


            RaycastHit hit;
            Vector3 thisPosition = Camera.main.transform.position;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //marker.transform.position = 
            particleSystem.transform.localPosition = Vector3.zero;
            //particleSystem.velocityOverLifetime = particleSystem.transform.position;
            particleSystem.Play();
            //particleSystem.velocityOverLifetime.yMultiplier = 1.0f;
            if (Physics.Raycast(ray, out hit))
            {
                //marker.transform.position = hit.point;
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            }
            else
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        }

    }
}

