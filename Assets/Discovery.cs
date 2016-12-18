using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Discovery : NetworkDiscovery
{
    bool connected = false;

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if (!connected)
        {
            base.OnReceivedBroadcast(fromAddress, data);
            NetworkManager.singleton.networkAddress = fromAddress;
            NetworkManager.singleton.StartClient();
            connected = true;
        }
    }


    public void setAsClient()
    {
        Initialize();
        StartAsClient();
    }

    public void setAsServer()
    {
        Initialize();
        StartAsServer();
    }

}