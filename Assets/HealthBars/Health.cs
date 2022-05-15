using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

// Basic Hitpoint class demonstrating a SyncVar with a hook
public class Health : NetworkBehaviour 
{


    //detects if health changes, if it does, it calls "onHealthChanged"
    [SyncVar(hook = "onHealthChanged")]
    public float health = 100;

    public float maxHealth = 100;
    

    HealthBar healthBar;

    public ParticleSystem hitFX;

    void Start()
    {
        // add a healthbar to the to the canvas
        healthBar = HealthBarManager.instance.AddHealthBar(this);
    }

    public void ApplyDamage(float damage)
    {
        // subtract the damage
        health -= damage;
    }
                              //Value before health was changed, value after
    public void onHealthChanged(System.Single oldValue, System.Single newValue)
    {
        // play the blood FX
        if (newValue < oldValue)
            hitFX.Play();

        // update our health bar
        if (healthBar)
            healthBar.UpdateMeter();

        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

