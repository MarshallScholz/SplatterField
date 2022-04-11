using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   // public GameObject gameObject;
    public GameObject marker;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 thisPosition = Camera.main.transform.position;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //ray.origin = new Vector3(thisPosition.x, thisPosition.y, thisPosition.z);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<ShieldSettings>())
                {
                    ShieldSettings shieldSettings = hit.collider.GetComponent<ShieldSettings>();
                    shieldSettings.UpdateCenter(hit.point);
                    marker.transform.position = hit.point;
                   // Instantiate(gameObject, marker);
                }
                Debug.DrawLine(ray.origin, hit.point, Color.red, 10);
            }
        }
    }
}
