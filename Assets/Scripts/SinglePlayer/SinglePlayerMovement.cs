using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerMovement : MonoBehaviour
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

        CameraController.instance.target = CM_target;
    }
    void Update()
    {
        float forward = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");
        animator.SetFloat("Forward", Mathf.Abs(forward), smooth, Time.deltaTime);
        animator.SetFloat("Sense", Mathf.Sign(forward), smooth, Time.deltaTime);

        // Vector3 playerMovement = (transform.forward * forward * playerSpeed + Physics.gravity) * Time.deltaTime;
        Vector3 playerMovement = (transform.forward * forward * playerSpeed + transform.right * turn * playerSpeed) * Time.deltaTime;

        //transform.eulerAngles += rotation;
        cc.Move(playerMovement);


        RaycastHit hit;
        Vector3 thisPosition = Camera.main.transform.position;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.3f))
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

        marker.transform.position = transform.position + (ray.direction * 5) + new Vector3(0, 1.8f, 0);

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
}


