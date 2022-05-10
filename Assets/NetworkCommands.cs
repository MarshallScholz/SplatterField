using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkCommands : NetworkBehaviour
{
    public static NetworkCommands instance;
    private void Start()
    {
        if (isLocalPlayer)
        {
            instance = this;
        }
    }

    public void UpdateSplatterMap(SplatterMap splatterMap, Vector3 position)
    {
        CmdUpdateSplatterMap(splatterMap.gameObject, position);
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateSplatterMap(GameObject splatterMap, Vector3 position)
    {
        RpcUpdateSplatterMap(splatterMap, position);
    }

    [ClientRpc]
    void RpcUpdateSplatterMap(GameObject splatterMap, Vector3 position)
    {
        splatterMap.GetComponent<SplatterMap>().UpdatePaint(position);

    }
        

}
