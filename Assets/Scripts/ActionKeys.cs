using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterFX))]
public class ActionKeys : MonoBehaviour
{
    public AnimatedAction[] actions;
    CharacterFX characterFX;
    // Start is called before the first frame update
    void Start()
    {
        characterFX = GetComponent<CharacterFX>();
    }
    void Update()
    {
        if (characterFX.currentAction == null)
        {
            // start with the 1 key
            // we'll need to rethink this if we have > 10 actions
            KeyCode key = KeyCode.Alpha1;
            foreach (AnimatedAction action in actions)
            {
                if (Input.GetKeyDown(key))
                    characterFX.DoAction(action);
                key++;
            }
        }
    }
}