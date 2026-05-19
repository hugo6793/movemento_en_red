using Unity.Netcode;
using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void Server()
    {
        NetworkManager.Singleton.StartServer();
    }
}
