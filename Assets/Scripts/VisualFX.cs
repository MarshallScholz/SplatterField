using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualFXSystem
{
    [CreateAssetMenu(fileName = "VisualFX", menuName = "VisualFX/VisualFXs", order = 1)]
    public class VisualFX : ScriptableObject
    {
        public GameObject prefab;
        public float duration;
        public bool autoStop;
        public bool detach;
        public float fade;

        public AudioClip clip;
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
            CheckClip(instance);

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
            CheckClip(instance);

            return instance;
        }

        void CheckClip(VisualFXInstance instance)
        {
            if (clip)
            {
                AudioSource source = instance.GetComponent<AudioSource>();
                if (source == null)
                    source = instance.gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.Play();
                source.spatialBlend = 0.8f;
            }
        }
    }


}