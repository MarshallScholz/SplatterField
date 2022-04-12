using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        cam = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // right drag rotates the camera
        if (Input.GetMouseButton(1))
        {
            Vector3 angles = transform.eulerAngles;
            float dx = -Input.GetAxis("Mouse Y");
            float dy = Input.GetAxis("Mouse X");
            // look up and down by rotating around X-axis
            angles.x = Mathf.Clamp(angles.x + dx * speed * Time.deltaTime, 0, 70);
            // spin the camera round
            angles.y += dy * speed * Time.deltaTime;
            transform.eulerAngles = angles;
        }

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
