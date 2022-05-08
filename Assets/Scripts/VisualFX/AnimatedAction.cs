using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VisualFXSystem
{

    [CreateAssetMenu(fileName = "AnimatedAction", menuName = "VisualFX/AnimatedAction", order = 1)]
    //abrstract can have classes that inherit its methods, and it cannot be attatched to a gameobject
    public abstract class AnimatedAction : ScriptableObject
    {
        public string animTrigger;
        public VisualFX beginFX;
        public CharacterFX.BodyPart beginPart;
        public VisualFX activateFX;
        public CharacterFX.BodyPart activatePart;

        public void Activate(CharacterFX character)
        {
            if (activateFX)
                activateFX.Begin(character.GetBodyPart(activatePart));
            OnActivate(character);
        }
        public abstract void OnActivate(CharacterFX character);
    }
}

