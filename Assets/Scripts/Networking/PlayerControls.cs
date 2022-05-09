using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControls : NetworkBehaviour
{
    public Animator animator;
    public float smooth;
    public CharacterController cc;

    // public GameObject gameObject;
    public GameObject marker;
    public LayerMask ignoreLayer;
    public ParticleSystem particleSystem;
    public AudioSource audioSource;
    public float playerSpeed = 0.5f;
    public float rotateSpeed = 1.0f;

    void Start()
    {
        ignoreLayer = LayerMask.GetMask("Ignore Raycast");
        audioSource = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (!isLocalPlayer)
            return;

        float fwd = Input.GetAxis("Vertical");
        animator.SetFloat("Forward", Mathf.Abs(fwd), smooth, Time.deltaTime);
        animator.SetFloat("Sense", Mathf.Sign(fwd), smooth, Time.deltaTime);
        animator.SetFloat("Turn", Input.GetAxis("Horizontal"), smooth, Time.deltaTime);

        Vector3 rotation = Vector3.up * Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        Vector3 movement = (transform.forward * Input.GetAxis("Vertical") * playerSpeed + Physics.gravity) * Time.deltaTime;

        transform.eulerAngles += rotation;
        cc.Move(movement);

        //RaycastHit hit;
        //Vector3 thisPosition = Camera.main.transform.position;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit))
        //{
        //    //marker.transform.position = hit.point;
        //    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        //}
        //else
        //{
        //    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        //    //marker.transform.position = ray.direction * 100;
        //}

        ////re-enable for shooting
        //if (Input.GetMouseButtonDown(0))
        //{

        //    //marker.transform.position = 
        //    //particleSystem.transform.localPosition = Vector3.zero;
        //    ////particleSystem.velocityOverLifetime = particleSystem.transform.position;
        //    //particleSystem.Play();
        //    //audioSource.Play();

        //    //particleSystem.velocityOverLifetime.yMultiplier = 1.0f;
        //}



    }
}

