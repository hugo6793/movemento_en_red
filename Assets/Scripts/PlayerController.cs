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

        switch (mode)
        {
            case AuthorityMode.Server:
                MoveServerRpc(input);
                break;

            case AuthorityMode.Client:
                MoveClient(input);
                break;

            case AuthorityMode.ServerRewind:
                MoveServerRpc(input);
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RewindSystem rewind = GetComponent<RewindSystem>();

            transform.position = rewind.GetPastPosition(1f);
        }
    }

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

    [ServerRpc]
    void JumpServerRpc()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void ClampToBounds()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -5f, 5f);
        pos.z = Mathf.Clamp(pos.z, -5f, 5f);

        transform.position = pos;
    }
}