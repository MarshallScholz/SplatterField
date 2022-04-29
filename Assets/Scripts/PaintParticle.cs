using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintParticle : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        other.GetComponentInParent<Transform>().GetComponentInParent<SplatterMap>().UpdatePaint(this.transform.position);
        Debug.Log(other.name);
    }
}
