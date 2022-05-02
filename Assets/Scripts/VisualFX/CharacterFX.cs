using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualFXSystem;


public class CharacterFX : MonoBehaviour
{
    public AnimatedAction currentAction;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public enum BodyPart
    {
        //transfrom for the character midway between their feet
        Root,
        LeftHand,
        RightHand,
        LeftFoot,
        RightFoot,
        Head,
        Chest
    }
    //Dictionary used to map enum values to body parts (eg LeftFoot = 3)
    Dictionary<BodyPart, Transform> parts;
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform head;
    public Transform chest;

    public Transform GetBodyPart(BodyPart part)
    {
        // lazy init of the dictionary when first needed
        if (parts == null)
        {
            parts = new Dictionary<BodyPart, Transform>();
            parts[BodyPart.LeftHand] = leftHand;
            parts[BodyPart.RightHand] = rightHand;
            parts[BodyPart.LeftFoot] = leftFoot;
            parts[BodyPart.RightFoot] = rightFoot;
            parts[BodyPart.Head] = head;
            parts[BodyPart.Chest] = chest;
        }
        if (parts.ContainsKey(part))
            return parts[part];
        // default - return our own transform, eg if Root is passed in
        return transform;
    }

    public void DoAction(AnimatedAction action)
    {
        currentAction = action;
        animator.SetTrigger(action.animTrigger);
        if (action.beginFX)
            action.beginFX.Begin(GetBodyPart(action.beginPart));
    }

    // called from an animation event
    public void Activate()
    {
        if (currentAction)
        {
            currentAction.Activate(this);
            currentAction = null; // free to do something else now
        }
    }

}
