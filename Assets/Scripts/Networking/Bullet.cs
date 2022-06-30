using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;

namespace Multisplat
{
    using Mirror;
    public class Bullet : NetworkBehaviour
    {
        [SyncVar]
        public Vector3 velocity = new Vector3(10, 0, 0);

        public float speed = 10;
        public float lifeTime;
        private Rigidbody rb;
        public Color colour;
        public GameObject player;

        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), lifeTime);
        }
        // destroy for everyone on the server

        void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = this.velocity * speed;
            Debug.Log(this.velocity);
        }


        //TRY DOING ONCOLLISIONSTAY TO PAINT WHILE IT ROLLS
        [Server]
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<Health>())
                other.transform.root.GetComponent<Health>().ApplyDamage(10);


            Vector3 collisionPoint = other.collider.ClosestPoint(this.transform.position);
            //other.gameObject.GetComponentInParent<SplatterMap>().CmdUpdatePaint(collisionPoint);
            FindObjectOfType<SplatterMap>().CmdUpdatePaint(collisionPoint, colour, player);


            NetworkServer.Destroy(this.gameObject);

            //CmdDestroyBullet(this.gameObject);
        }

        [Command(requiresAuthority = false)]
        public void CmdDestroyBullet(GameObject bullet)
        {
            RpcDestroyBullet(bullet);
        }

        [ClientRpc]
        public void RpcDestroyBullet(GameObject bullet)
        {
            DestroyBullet(bullet);
        }

        public void DestroyBullet(GameObject bullet)
        {
            Destroy(bullet);
        }

        private void Update()
        {
            Debug.Log("Bullet Velocity" + rb.velocity);
            Debug.Log("Velocity" + velocity);
        }



    }
}
