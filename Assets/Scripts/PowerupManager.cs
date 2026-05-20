using Unity.Netcode;
using UnityEngine;

public class PowerupManager : NetworkBehaviour
{
    public float interval = 20f;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        InvokeRepeating(
            nameof(RandomEffect),
            interval,
            interval
        );

        Debug.Log("Powerup system started");
    }

    void RandomEffect()
    {
        PlayerController[] players =
            FindObjectsOfType<PlayerController>();

        if (players.Length == 0)
        {
            Debug.Log("No players found");
            return;
        }

        int randomPlayer =
            Random.Range(0, players.Length);

        int randomEffect =
            Random.Range(0, 2);

        PlayerController target =
            players[randomPlayer];

        if (randomEffect == 0)
        {
            target.ApplyBoost();

            Debug.Log(
                target.name + " received BOOST"
            );
        }
        else
        {
            target.ApplySlow();

            Debug.Log(
                target.name + " received SLOW"
            );
        }
    }
}