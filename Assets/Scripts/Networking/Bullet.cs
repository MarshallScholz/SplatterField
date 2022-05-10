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
        public float lifeTime;

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

        // Update is called once per frame on the server
        [Server]
        void Update()
        {
            transform.position += velocity * Time.deltaTime;
        }

        [Server]
        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Health>())
                other.GetComponent<Health>().ApplyDamage(10);

            if (other.GetComponentInParent<Transform>().GetComponentInParent<SplatterMap>())
            {
                //collisionEvents[i].colliderComponent.transform.position;
                Vector3 collisionPoint = other.ClosestPoint(this.transform.position);
                //Vector3 pos = collisionEvents[i].colliderComponent.transform.position;
                SplatterMap splatterMap = other.GetComponentInParent<SplatterMap>();
                NetworkCommands.instance.UpdateSplatterMap(splatterMap,collisionPoint);
            }

            NetworkServer.Destroy(this.gameObject);
        }



    }
}
