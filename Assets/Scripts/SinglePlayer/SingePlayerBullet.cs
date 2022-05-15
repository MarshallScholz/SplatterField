using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingePlayerBullet : MonoBehaviour
{
    public Vector3 velocity = new Vector3(10, 0, 0);
    public float speed = 10;
    public float lifeTime;
    private Rigidbody rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = this.velocity * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Health>())
            other.gameObject.GetComponent<Health>().ApplyDamage(10);

        if (other.gameObject.GetComponentInParent<Transform>().GetComponentInParent<SingePlayerSplatterMap>())
        {
            //collisionEvents[i].colliderComponent.transform.position;
            Vector3 collisionPoint = other.collider.ClosestPoint(this.transform.position);
            //Vector3 pos = collisionEvents[i].colliderComponent.transform.position;
            other.gameObject.GetComponentInParent<SingePlayerSplatterMap>().UpdatePaint(collisionPoint);



        }

        Destroy(this.gameObject);
    }
}
