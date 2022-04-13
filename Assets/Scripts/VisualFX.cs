using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualFXSystem
{
    [CreateAssetMenu(fileName = "VisualFX", menuName = "VisualFX/VisualFX", order = 1)]
    public class VisualFX : ScriptableObject
    {
        public GameObject prefab;
        public float duration;
        public bool autoStop;
        public bool detach;
        public float fade;
        //public Color color = Color.white;
        public Color[] colors = new Color[] { Color.white };
        public VisualFXInstance Begin(Transform t)
        {
            GameObject obj = Instantiate(prefab, detach ? null : t);
            if (detach)
            {
                obj.transform.position = t.position;
            }
            VisualFXInstance instance = obj.GetComponent<VisualFXInstance>();
            if (instance == null)
                instance = obj.AddComponent<VisualFXInstance>();
            instance.Init(this, autoStop);
            return instance;
        }

        public VisualFXInstance Begin(Transform t, Transform t2)
        {
            GameObject obj = Instantiate(prefab, detach ? null : t);
            if (detach)
            {
                obj.transform.position = t.position;
                Vector3 temp = t2.rotation.eulerAngles;
                temp.x = 90.0f;
                obj.transform.rotation = Quaternion.Euler(temp);
            }

            VisualFXInstance instance = obj.GetComponent<VisualFXInstance>();

            if (instance == null)
                instance = obj.AddComponent<VisualFXInstance>();

            instance.Init(this, autoStop);
            return instance;
        }
    }


}