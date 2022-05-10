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

        //attatch player to camera if local player
        if (CameraController.instance != null && isLocalPlayer)
        {
            CameraController.instance.target = transform;
        }
    }
    void Update()
    {
        if (!isLocalPlayer)
            return;

        float forward = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");
        animator.SetFloat("Forward", Mathf.Abs(forward), smooth, Time.deltaTime);
        animator.SetFloat("Sense", Mathf.Sign(forward), smooth, Time.deltaTime);
        animator.SetFloat("Turn", turn, smooth, Time.deltaTime);

        Vector3 rotation = (Vector3.up * turn) * (rotateSpeed * Time.deltaTime);
        Vector3 movement = (transform.forward * forward * playerSpeed + Physics.gravity) * Time.deltaTime;

        transform.eulerAngles += rotation;
        cc.Move(movement);



        RaycastHit hit;
        Vector3 thisPosition = Camera.main.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            marker.transform.position = hit.point;
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            marker.transform.position = ray.direction * 100;
        }

        CmdLook(marker.transform.position);


    }

    [Command]
    void CmdLook(Vector3 position)
    {
        //tells all clients to do it
        RpcLook(position);
    }

    [ClientRpc]
    void RpcLook(Vector3 position)
    {
        LookAt(position);
    }

    void LookAt(Vector3 position)
    {
        marker.transform.position = position;
    }
}
