using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LobbyManager : NetworkLobbyManager
{
    public TextMeshPro playerName;

    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
        

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
