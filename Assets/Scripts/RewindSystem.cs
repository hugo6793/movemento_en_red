using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RewindSystem : MonoBehaviour
{
    struct Snapshot
    {
        public Vector3 pos;
        public float time;
    }

    Queue<Snapshot> history = new Queue<Snapshot>();

    void FixedUpdate()
    {
        // 🔥 SOLO EL SERVER guarda historial
        if (!NetworkManager.Singleton.IsServer)
            return;

        history.Enqueue(new Snapshot
        {
            pos = transform.position,
            time = Time.time
        });

        if (history.Count > 60)
            history.Dequeue();
    }

    public Vector3 GetPastPosition(float secondsBack)
    {
        foreach (var snap in history)
        {
            if (Time.time - snap.time >= secondsBack)
                return snap.pos;
        }

        return transform.position;
    }
}