using Unity.Netcode;
using UnityEngine;


public enum AuthorityMode : int
{
    Server = 0,
    ServerRewind = 1,
    Client = 2
}

public class GameModeManager : NetworkBehaviour
{
    public static GameModeManager Instance;

    public NetworkVariable<int> Mode = new NetworkVariable<int>(0);

    void Awake()
    {
        Instance = this;
        Debug.Log("GameModeManager INIT");
    }

    public void SetMode(AuthorityMode mode)
    {
        if (!IsServer) return;
        Mode.Value = (int)mode;
    }

    public AuthorityMode GetMode()
    {
        return (AuthorityMode)Mode.Value;
    }
}