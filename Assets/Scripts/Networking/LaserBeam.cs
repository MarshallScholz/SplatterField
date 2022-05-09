using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorNetwork;

public class LaserBeam : NetworkBehaviour {

    public LineRenderer lineRenderer;
    public float coolDown = 1;
    public ParticleSystem fireFX;
    int index = 1;

    public GameObject bulletPrefab;

    // Use this for initialization
    void Start()
    {
        // turn off the linerenderer
        ShowLaser(false);
        CharacterMovement cm = GetComponent<CharacterMovement>();
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
            if (coolDown < 0.5f)
                ShowLaser(false);
        }

        // only check controls if we're the local player
        if (!isLocalPlayer)
            return;
        if (Input.GetButtonDown("Fire" + index) && coolDown <= 0)
            CmdFire();

    }

    [Command]
    void CmdFire()
    {
        //tells all clients to do it
        RpcFire();
    }

    [ClientRpc]
    void RpcFire()
    {
        Hit();
    }

    void DoLaser()
    {
        // trigger the visuals - this should happen on all machines individually
        ShowLaser(true);
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

    void ShowLaser(bool show)
    {
        lineRenderer.enabled = show;
    }

    [Server]
    public void Hit()
    {
        // this gets called in response to animation events
        // DoLaser();
        GameObject go = Instantiate(bulletPrefab.gameObject,
        transform.position + Vector3.up + transform.forward,
        Quaternion.LookRotation(transform.forward));
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.velocity = transform.forward * 5;

        NetworkServer.Spawn(bullet.gameObject);

    }
}
