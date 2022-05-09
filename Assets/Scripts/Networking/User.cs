using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MirrorNetwork {
    using Mirror;

    public class User : NetworkBehaviour
    {


        void Update()
        {
            if (!isLocalPlayer)
                return;

            // check for clicking on nearby objects
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // make sure we're within 2m
                    //if (Vector3.Distance(hit.point, transform.position) < 2)
                   //// {
                        GameObject target = hit.transform.gameObject;
                        
                        if (target.GetComponent<Interactible>())
                            CmdUse(target);
                        //this happens on the local client
                    //}
                }
            }
        }

        
        [Command]
        void CmdUse(GameObject targetObject)
        {
            //called on the server
            RpcUse(targetObject);
        }
        [ClientRpc]
        void RpcUse(GameObject targetObject)
        {
            //called on each client
            Interactible interactible = targetObject.GetComponent<Interactible>();
            if(interactible)
            {
                interactible.Use();
            }
        }
    }
}
