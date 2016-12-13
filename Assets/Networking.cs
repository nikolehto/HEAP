using UnityEngine;

using UnityEngine.Networking;


public class Networking : NetworkManager
{

    public NetworkDiscovery discovery;

    public override void OnStartHost()
    {
        //string ipAdd = Network.player.ipAddress;
        //Debug.Log(ipAdd);

        discovery.Initialize();
        discovery.StartAsServer();
    }

    public override void OnStartClient(NetworkClient client)
    {
        discovery.showGUI = false;
    }

    public override void OnStopClient()
    {
        discovery.StopBroadcast();
        discovery.showGUI = true;
    }
}