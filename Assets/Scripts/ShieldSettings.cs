using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSettings : MonoBehaviour
{
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;

        //material.SetTexture("_Texture", GetComponent<Texture>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCenter(Vector3 hitPoint)
    {

        material.SetVector("_SphereCenter", new Vector4(hitPoint.x, hitPoint.y, hitPoint.z, 0));
    }
}
