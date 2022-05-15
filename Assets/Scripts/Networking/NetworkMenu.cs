using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class NetworkMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public NetworkManager networkManager;
    public GameObject canvas;
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
}