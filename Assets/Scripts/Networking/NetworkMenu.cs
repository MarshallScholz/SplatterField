using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

namespace Multisplat
{
    public class NetworkMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        public NetworkRoomManager networkManager;
        public GameObject canvas;

        public TMP_Text gamertag;

        //[SyncVar(hook = "PlayerName")] public string playerName;
        public void Host()
        {
            networkManager.StartHost();
            canvas.SetActive(false);
        }
        public void SetIP(string ip)
        {
            networkManager.networkAddress = ip;
        }
        public void Join()
        {
            networkManager.StartClient();
            canvas.SetActive(false);
        }

        private void Start()
        {
            canvas.SetActive(true);
        }

        public void UpdatePlayerName(string newName)
        {
            //playerName = newName;
            //gamertag.text = playerName;
        }

        private void OnApplicationQuit()
        {
            networkManager.StopServer();
        }

        //private void OnApplicationQuit()
        //{
        //    if(networkManager.is)
        //    networkManager.StopHost();
        //    networkManager.StopServer();
        //}
    }
}
