using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;

namespace MirrorNetwork
{
    using Mirror;
    public class Bullet : NetworkBehaviour
    {
        public Vector3 velocity = new Vector3(10, 0, 0);
        public float speed = 10;
        public float lifeTime;
        private Rigidbody rb;

        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), lifeTime);
        }
        // destroy for everyone on the server
        [Server]
        void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = this.velocity * speed;
        }

        // Update is called once per frame on the server
        [Server]
        void Update()
        {
            //transform.position += velocity * Time.deltaTime;
            //velocity -= new Vector3(1, -Physics.gravity.y, 1);
        }


        //TRY DOING ONCOLLISIONSTAY TO PAINT WHILE IT ROLLS
        [Server]
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.GetComponent<Health>())
                other.gameObject.GetComponent<Health>().ApplyDamage(10);

            if (other.gameObject.GetComponentInParent<Transform>().GetComponentInParent<SplatterMap>())
            {
                //collisionEvents[i].colliderComponent.transform.position;
                Vector3 collisionPoint = other.collider.ClosestPoint(this.transform.position);
                //Vector3 pos = collisionEvents[i].colliderComponent.transform.position;
                other.gameObject.GetComponentInParent<SplatterMap>().CmdUpdatePaint(collisionPoint);


                //RaycastHit hit;
                //if (!Physics.Raycast(Camera.main.ScreenPointToRay(other.collider.ClosestPoint(this.transform.position)), out hit))
                //    return;

                //Renderer rend = hit.transform.GetComponent<Renderer>();
                //MeshCollider meshCollider = hit.collider as MeshCollider;

                //Vector2 collisionPointRaw = hit.textureCoord;

                //if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                //    return;

                //other.gameObject.GetComponentInParent<SplatterMap>().CmdUpdatePaint(collisionPoint);



            }

            NetworkServer.Destroy(this.gameObject);
        }



    }
}
