using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualFXSystem
{


    public class AnimationFXBehaviour : StateMachineBehaviour
    {
        public VisualFX fx;
        public CharacterFX.BodyPart bodyPart;

        VisualFXInstance fxInstance;
        // OnStateEnter is called when a transition starts and the state machine
        //starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterFX characterFX = animator.GetComponent<CharacterFX>();
            //fxInstance = fx.Begin(characterFX.GetBodyPart(bodyPart), false);
            fxInstance = fx.Begin(characterFX.GetBodyPart(bodyPart));

        }
        // OnStateExit is called when a transition ends and the state machine
        //finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            fxInstance.End();
        }
    }
}
