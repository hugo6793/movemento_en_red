using System.Collections.Generic;
using UnityEngine;

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
        if (!GameModeManager.Instance || 
            GameModeManager.Instance.GetMode() != AuthorityMode.ServerRewind)
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