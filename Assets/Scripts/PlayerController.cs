using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement")]
    public float normalSpeed = 5f;
    public float boostSpeed = 8f;
    public float slowSpeed = 2f;
    public float jumpForce = 5f;

    float currentSpeed;

    Rigidbody rb;
    Renderer playerRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerRenderer = GetComponentInChildren<Renderer>();

        currentSpeed = normalSpeed;
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RewindServerRpc();
        }
    }

    void HandleMovement()
    {
        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        );

        AuthorityMode mode =
            GameModeManager.Instance.GetMode();

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
    }

    [ServerRpc]
    void MoveServerRpc(Vector3 input)
    {
        Vector3 move =
            input * currentSpeed * Time.deltaTime;

        transform.position += move;

        ClampPosition();
    }

    void MoveClient(Vector3 input)
    {
        Vector3 move =
            input * currentSpeed * Time.deltaTime;

        transform.position += move;

        ClampPosition();
    }

    [ServerRpc]
    void JumpServerRpc()
    {
        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );
    }

    [ServerRpc]
    void RewindServerRpc()
    {
        if (GameModeManager.Instance.GetMode()
            != AuthorityMode.ServerRewind)
            return;

        RewindSystem rewind =
            GetComponent<RewindSystem>();

        transform.position =
            rewind.GetPastPosition(1f);
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -5f, 5f);
        pos.z = Mathf.Clamp(pos.z, -5f, 5f);

        transform.position = pos;
    }

    public void ApplyBoost()
    {
        currentSpeed = boostSpeed;

        playerRenderer.material.color =
            Color.green;

        CancelInvoke(nameof(ResetEffect));

        Invoke(nameof(ResetEffect), 10f);
    }

    public void ApplySlow()
    {
        currentSpeed = slowSpeed;

        playerRenderer.material.color =
            new Color(1f, 0.5f, 0f);

        CancelInvoke(nameof(ResetEffect));

        Invoke(nameof(ResetEffect), 10f);
    }

    void ResetEffect()
    {
        currentSpeed = normalSpeed;

        playerRenderer.material.color =
            Color.white;
    }
}