using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintParticle : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    public GameObject marker;
    public LayerMask ignoreLayer;


    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();

        ignoreLayer = LayerMask.GetMask("Ignore Raycast");
    }
    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;



        //collisionEvents[0].
        while (i < numCollisionEvents)
        {       
            if (other.GetComponentInParent<Transform>().GetComponentInParent<SplatterMap>())
            {
                Vector3 pos = collisionEvents[i].intersection;
                marker.transform.position = pos;
                this.transform.localPosition = Vector3.zero;
                other.GetComponentInParent<Transform>().GetComponentInParent<SplatterMap>().UpdatePaint(pos);
            }
            else
            {
                RaycastHit hit;
                Vector3 thisPosition = Camera.main.transform.position;
                Ray ray = Camera.main.WorldToScreenPoint(collisionEvents[i].intersection);
                //ray.origin = new Vector3(thisPosition.x, thisPosition.y, thisPosition.z);
                if (Physics.Raycast(ray, out hit))
                {
                    marker.transform.position = hit.point;
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                    //Debug.Log(hit.lightmapCoord);
                    foreach (PaintSpot paintSpot in FindObjectsOfType<PaintSpot>())
                    {
                        paintSpot.UpdateTexture(hit.lightmapCoord);
                    }
                }

                Vector3 pos = collisionEvents[i].intersection;
                marker.transform.position = pos;
                this.transform.localPosition = Vector3.zero;
                other.ligh
                other.GetComponent<PaintSpot>().UpdateTexture
            }
            Debug.Log(pos);
            //Destroy(part);
            i++;
        }
    }
}
