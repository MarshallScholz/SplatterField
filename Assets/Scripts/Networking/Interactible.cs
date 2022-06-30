using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Multisplat
{
    public class Interactible : MonoBehaviour
    {
        public UnityEvent onUsed;
        public void Use()
        {
            onUsed.Invoke();
        }
    }
}

