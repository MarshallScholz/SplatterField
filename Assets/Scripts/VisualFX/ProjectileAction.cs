using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VisualFXSystem
{

    [CreateAssetMenu(fileName = "ProjectileAction",
     menuName = "VisualFX/ProjectileAction", order = 2)]
    public class ProjectileAction : AnimatedAction
    {
        public VisualFX projectileFX;
        public VisualFX impactFX;

        public Projectile projectilePrefab;
        public float projectileSpeed = 10;
        public override void OnActivate(CharacterFX character)
        {
            // spawn a projctile
            Projectile projectile = Instantiate(projectilePrefab);
            Transform spawnPoint = character.GetBodyPart(activatePart);
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = character.transform.rotation;
            projectile.velocity = projectile.transform.forward * projectileSpeed;
            projectile.action = this;

            projectileFX.Begin(projectile.transform);
        }


    }
}
