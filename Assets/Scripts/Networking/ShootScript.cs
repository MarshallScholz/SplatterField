using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using VisualFXSystem;

namespace Multisplat
{
    public class ShootScript : NetworkBehaviour
    {
        public LineRenderer lineRenderer;
        public float coolDown = 1;
        public ParticleSystem fireFX;
        public Transform gunTransform;
        public float bulletSpeed = 5;
        public Vector3 bulletOffset;
        int index = 1;

        public GameObject bulletPrefab;

        public float coolDownLength = 1;


        // Use this for initialization
        void Start()
        {
            // turn off the linerenderer
            //ShowLaser(false);
            //==================CHANGE THIS TO PLAYERCONTROLS
            PlayerControls cm = GetComponent<PlayerControls>();
            if (cm)
                index = cm.index;
        }

        // Update is called once per frame
        void Update()
        {
            // count down, and hide the laser after half a second
            if (coolDown > 0)
            {
                coolDown -= Time.deltaTime;
                //if (coolDown < 0.5f)
                //    ShowLaser(false);
            }

            // only check controls if we're the local player
            if (!isLocalPlayer)
                return;
            if (Input.GetButton("Fire" + index) && coolDown <= 0)
            {
                //CmdFire(this.gameObject);
                CmdFire(this.gameObject);
                coolDown = coolDownLength;
            }

        }

        [Command(requiresAuthority = false)]
        void CmdFire(GameObject playerObject)
        {

            ///ONLY THE PLAYER CAN HAVE A NETWORK IDENTITY, SO I NEED TO GRAB THE PLAYER, AND GET THE REFERENCE OF THE GUNOBJECT FROM THEM
            //tells all clients to do it
            SpawnBullet(playerObject);
        }

        //[ClientRpc]
        //void RpcFire(GameObject playerObject)
        //{
        //    SpawnBullet(playerObject);
        //}

        void DoLaser()
        {
            // trigger the visuals - this should happen on all machines individually
            //ShowLaser(true);
            coolDown = 1.0f;

            // more visual fx, a burst around the firing nozzle
            if (fireFX)
                fireFX.Play();

            // do a raycast to subtract health. We only want to do this on the server
            // rather than each client doing their own raycast
            if (!isServer)
                return;

            // do a raycast against anything which may have a health on it
            RaycastHit hit;
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 10.0f))
            {
                Health health = hit.transform.GetComponent<Health>();
                if (health)
                {
                    // subtract health from the other player
                    health.ApplyDamage(20);
                }
            }
        }

        //void ShowLaser(bool show)
        //{
        //    if(lineRenderer)
        //        lineRenderer.enabled = show;
        //}
        [Server]
        public void SpawnBullet(GameObject playerObject)
        {
            // this gets called in response to animation events
            // DoLaser();
            Transform gunTransform = playerObject.GetComponent<CharacterFX>().rightGun;
            GameObject go = Instantiate(bulletPrefab.gameObject, gunTransform.position + bulletOffset, Quaternion.LookRotation(gunTransform.forward));
            Bullet bullet = go.GetComponent<Bullet>();

            bullet.velocity = gunTransform.right;
            Debug.Log("Player " + playerObject.GetComponent<PlayerControls>().playerIndex + ": " + gunTransform.right);

            bullet.colour = GetComponent<PlayerControls>().paintColour;
            bullet.player = this.gameObject;

            NetworkServer.Spawn(go);
            //IF I HAVE TROUBLE UPDATING THE BULLET'S VELOCITY 
            //StartCoroutine(setBulletVelocity(bullet.velocity, bullet.gameObject)); 

        }

        IEnumerator setBulletVelocity(Vector3 velocity, GameObject bullet)
        {
            yield return new WaitForSeconds(0.01f);
            if (bullet != null)
                RpcSetBulletVelocity(velocity, bullet);

        }

        [Command(requiresAuthority = false)]
        public void RpcSetBulletVelocity(Vector3 velocity, GameObject bullet)
        {
            bullet.GetComponent<Rigidbody>().velocity = velocity * bulletSpeed;
        }
    }
}