using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

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

    public float jumpForce = 10;
    public Vector3 playerVelocity = Vector3.zero;
    public Transform CM_target;

    public float verticalRotMin = -80;
    public float verticalRotMax = 80;

    //public Transform feetHitPosition;

    void Start()
    {
        ignoreLayer = LayerMask.GetMask("Ignore Raycast");
        audioSource = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        //attatch player to camera if local player
        if (CameraController.instance != null && isLocalPlayer)
        {
            CameraController.instance.target = CM_target;
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
        //animator.SetFloat("Turn", turn, smooth, Time.deltaTime);

        //Vector3 rotation = (Vector3.up * turn) * (rotateSpeed * Time.deltaTime);

        // Vector3 playerMovement = (transform.forward * forward * playerSpeed + Physics.gravity) * Time.deltaTime;
        Vector3 playerMovement = (transform.forward * forward * playerSpeed + transform.right * turn * playerSpeed) * Time.deltaTime;

        //transform.eulerAngles += rotation;
        cc.Move(playerMovement);


        RaycastHit hit;
        Vector3 thisPosition = Camera.main.transform.position;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.15f))
        {
            Debug.Log("can jump");
            Debug.DrawRay(transform.position, -transform.up * hit.distance, Color.yellow);
            //feetHitPosition.position = hit.point;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * Physics.gravity.y);
            }
            else
            {
                playerVelocity.y = 0;
            }

        }

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit))
        //{
        //    marker.transform.position = hit.point;
        //    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        //}
        //else
        //{
        //    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        //    marker.transform.position = ray.direction * 100;
        //}
                                                                 //players height
        marker.transform.position = transform.position + (ray.direction * 5) + new Vector3(0, 1.8f, 0);
        //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, marker.transform.position);

        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.time);
        //transform.LookAt(new Vector3(marker.transform.position.x, 0, marker.transform.position.z));
        CmdLook(marker.transform.position);


        //gets the mouse input to rotate the character
        var rotInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var rot = transform.eulerAngles;
        rot.y += rotInput.x * rotateSpeed;
        transform.rotation = Quaternion.Euler(rot);

        //rotates the CM target so that the camera rotates correctly as well
        if (CM_target != null)
        {
            rot = CM_target.localRotation.eulerAngles;
            rot.x -= rotInput.y * rotateSpeed;
            if (rot.x > 180)
                rot.x -= 360;
            //clamps the up and down camera rotation
            rot.x = Mathf.Clamp(rot.x, verticalRotMin, verticalRotMax);
            CM_target.localRotation = Quaternion.Euler(rot);
        }

    }

    private void FixedUpdate()
    {
        //Vector3 rawtoRotation = new Vector3(marker.transform.position.x, 0, marker.transform.position.z);
        //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, rawtoRotation);
        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.5f * Time.time);
        //transform.LookAt(new Vector3(marker.transform.position.x, 0, marker.transform.position.z));
        //transform.Rotate(0f, Input.mousePosition.x, 0f, Space.World);

        //float yawCamera = Camera.main.transform.rotation.eulerAngles.y;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), rotateSpeed * Time.deltaTime);
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

