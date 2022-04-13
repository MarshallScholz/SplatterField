using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace VisualFXSystem
{
    public class VisualFXInstance : MonoBehaviour
    {
        float countdown;
        public bool countingDown;
        private ParticleSystem[] particles;
        private DecalProjector[] decals;
        private AudioSource audiosource;

        public void Init(VisualFX fx, bool autoStop)
        {
            countingDown = autoStop;
            countdown = fx.duration;

            int index = 0;
            particles = GetComponentsInChildren<ParticleSystem>();
            decals = GetComponentsInChildren<DecalProjector>();
            audiosource = GetComponentInChildren<AudioSource>();

            audiosource.Stop();

            audiosource.Play();
            //change each type colour. Eg a light ring particle effect, and then a smoke particle effect
            foreach (ParticleSystem ps in particles)
            {
                ParticleSystem.MainModule main = ps.main;
                main.startColor = fx.colors[index];
                ParticleSystem.ColorOverLifetimeModule col = ps.colorOverLifetime;
                col.color = TintGradient(col.color, fx.colors[index]);
                index++;
                index %= fx.colors.Length;
            }
            foreach (DecalProjector decal in decals)
            {
                //DecalProjector. main = ps.main;
                //main.startColor = fx.colors[index];
                //ParticleSystem.ColorOverLifetimeModule col = ps.colorOverLifetime;
                //col.color = TintGradient(col.color, fx.colors[index]);
                //index++;
                //index %= fx.colors.Length;
            }
        }
        public void Update()
        {
            if (!countingDown)
                //return false;
                return;
            countdown -= Time.deltaTime;
            if (countdown < 0)
            {
                GameObject.Destroy(gameObject);
            }
        }
        public bool isFinished() { return countdown <= 0; }

        public static ParticleSystem.MinMaxGradient
        TintGradient(ParticleSystem.MinMaxGradient gradient, Color color)
        {
            switch (gradient.mode)
            {
                case ParticleSystemGradientMode.Color:
                    gradient.color = color;
                    break;
                case ParticleSystemGradientMode.Gradient:
                    {
                        Gradient g = gradient.gradient;
                        GradientColorKey[] colorKeys = g.colorKeys;
                        for (int i = 0; i < colorKeys.Length; i++)
                            colorKeys[i].color = color;
                        g.SetKeys(colorKeys, g.alphaKeys);
                        gradient.gradient = g;
                    }
                    break;
            }
            return gradient;
        }

        public void End()
        {
            GameObject.Destroy(gameObject);
        }
    }
}