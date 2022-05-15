using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float distance;
    Transform cam;

    float currentDistance;
    public float relaxSpeed;

    float distanceBack;
    public float zoomSpeed;
    public float minDistance;
    public float maxDistance;

    public Vector3 offset;


    //singleston pattern - this is the one and only CameraController
    public static CameraController instance;


    public CinemachineVirtualCamera cm;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        cam = this.GetComponent<Transform>();
    }

    public void UpdateTarget()
    {
        cm.Follow = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        UpdateTarget();

        RaycastHit hit;
        if (Physics.Raycast(target.position + offset, -transform.forward, out hit, currentDistance, 9))
        {
            // snap the camera right in to where the collision happened
            currentDistance = hit.distance;
        }
        else
        {
            distanceBack = Mathf.Clamp(distanceBack - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minDistance, maxDistance);

            // relax the camera back to the desired distance
            currentDistance = Mathf.MoveTowards(currentDistance, distanceBack,
            Time.deltaTime * relaxSpeed);
        }


        // look at the target point
        transform.position = (target.position + offset)
        - currentDistance * transform.forward;
    }

}
