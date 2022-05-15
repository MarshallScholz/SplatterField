using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerShoot : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float coolDown = 1;
    public ParticleSystem fireFX;
    public Transform gunTransform;
    public Vector3 bulletOffset;
    int index = 1;

    public GameObject bulletPrefab;


    // Use this for initialization
    void Start()
    {
        // turn off the linerenderer
        //ShowLaser(false);
        CharacterMovement cm = GetComponent<CharacterMovement>();
        if (cm)
            index = cm.index;
    }

    // Update is called once per frame
    void Update()
    {
        // count down, and hide the laser after half a second

        coolDown -= Time.deltaTime;
        if (coolDown < 0f)
            if (Input.GetButtonDown("Fire" + index))
                SpawnBullet();

    }


    void DoLaser()
    {
        // trigger the visuals - this should happen on all machines individually
        //ShowLaser(true);
        coolDown = 1;
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

    public void SpawnBullet()
    {
        // this gets called in response to animation events
        DoLaser();



        // this gets called in response to animation events
        // DoLaser();
        GameObject go = Instantiate(bulletPrefab.gameObject,
        gunTransform.position + bulletOffset,
        Quaternion.LookRotation(gunTransform.forward));
        SingePlayerBullet bullet = go.GetComponent<SingePlayerBullet>();
        bullet.velocity = gunTransform.right;
    }
}
