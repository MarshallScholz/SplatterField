using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   // public GameObject gameObject;
    public GameObject marker;
    public LayerMask ignoreLayer;
    // Update is called once per frame

    private void Start()
    {
        ignoreLayer = LayerMask.GetMask("Ignore Raycast");
    }
    void Update()
    {
        RaycastHit hit;
        Vector3 thisPosition = Camera.main.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //ray.origin = new Vector3(thisPosition.x, thisPosition.y, thisPosition.z);
        if (Physics.Raycast(ray, out hit))
        {
            marker.transform.position = hit.point;
            Debug.DrawRay(ray.origin,  ray.direction * hit.distance, Color.red);
        }
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

    }
}
