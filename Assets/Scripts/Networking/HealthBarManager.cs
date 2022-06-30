using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {

    public GameObject prefab;

    public static HealthBarManager instance;

    // Use this for initialization
    void Awake () {
        instance = this;	
	}

    public HealthBar AddHealthBar(Health health)
    {
        GameObject go = Instantiate(prefab);
        go.transform.parent = transform;
        HealthBar hb = go.GetComponent<HealthBar>();
        hb.health = health;

        return hb;
    }
}
