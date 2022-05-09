using UnityEngine;
using System.Collections;
using Mirror;

// a basic character movement script, for moving a character around in multiplayer with a Network transform
public class CharacterMovement : NetworkBehaviour {

    public float MoveSpeed = 10.0f;
    public float RotateSpeed = 180.0f;
    CharacterController cc;
    public int index = 1;

	// Use this for initialization
	void Start () 
    {
        cc = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        // rotate the character according to left/right key presses
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal" + index) * RotateSpeed * Time.deltaTime);
        // move forward/backward according to up/down key presses
        cc.Move((transform.forward * Input.GetAxis("Vertical" + index) * MoveSpeed + Physics.gravity) * Time.deltaTime);
    }
}
