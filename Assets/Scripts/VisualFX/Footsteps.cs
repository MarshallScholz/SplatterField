using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualFXSystem
{
    public class Footsteps : MonoBehaviour
    {
        public Transform[] feet;
        public VisualFX fx;
        public void Step(int foot)
        {
            // foot has to be >0 to register in the animation event,
            // so values of 1 and 2 correspond to
            // the first and second foot, hence the -1 here
            fx.Begin(feet[foot - 1], this.transform);

        }
    }
}
