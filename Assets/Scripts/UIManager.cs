using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multisplat
{
    public class UIManager : MonoBehaviour
    {
        public Texture2D[] paintColours;
        public Texture2D currentPaintColour;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void PaintColour(int colour)
        {
            currentPaintColour = paintColours[colour];
            FindObjectOfType<SplatterMap>().UpdateColour(currentPaintColour);
        }
    }

}