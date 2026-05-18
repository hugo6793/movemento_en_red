using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public void ServerMode()
    {
        GameModeManager.Instance.SetMode(AuthorityMode.Server);
        Debug.Log("SERVER BUTTON PRESSED");
    }

    public void RewindMode()
    {
        GameModeManager.Instance.SetMode(AuthorityMode.ServerRewind);
        Debug.Log("SERVER REWIND BUTTON PRESSED");
    }

    public void ClientMode()
    {
        GameModeManager.Instance.SetMode(AuthorityMode.Client);
        Debug.Log("CLIENT BUTTON PRESSED");
    }
}