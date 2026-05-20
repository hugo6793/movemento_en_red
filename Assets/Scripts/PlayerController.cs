using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float speed = 6f;
    public float jumpForce = 5f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleInput();
    }

    void HandleInput()
    {
        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        AuthorityMode mode = GameModeManager.Instance.GetMode();

        // 🔥 MOVIMIENTO
        if (mode == AuthorityMode.Server || mode == AuthorityMode.ServerRewind)
        {
            MoveServerRpc(input);
        }
        else if (mode == AuthorityMode.Client)
        {
            MoveClient(input);
        }

        // 🔥 SALTO
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpServerRpc();
        }

        // 🔥 REWIND (SOLO PEDIDO AL SERVIDOR)
        if (Input.GetKeyDown(KeyCode.R))
        {
            RewindServerRpc();
        }
    }

    // =========================
    // MOVIMIENTO
    // =========================

    [ServerRpc]
    void MoveServerRpc(Vector3 input)
    {
        transform.position += input * speed * Time.deltaTime;
        ClampToBounds();
    }

    void MoveClient(Vector3 input)
    {
        transform.position += input * speed * Time.deltaTime;
    }

    // =========================
    // SALTO
    // =========================

    [ServerRpc]
    void JumpServerRpc()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // =========================
    // REWIND (SERVER AUTHORITY)
    // =========================

    [ServerRpc]
    void RewindServerRpc()
    {
        if (GameModeManager.Instance.GetMode() != AuthorityMode.ServerRewind)
            return;

        RewindSystem rewind = GetComponent<RewindSystem>();
        if (rewind == null) return;

        transform.position = rewind.GetPastPosition(1f);
    }

    // =========================
    // LIMITES
    // =========================

    void ClampToBounds()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -5f, 5f);
        pos.z = Mathf.Clamp(pos.z, -5f, 5f);

        transform.position = pos;
    }
}