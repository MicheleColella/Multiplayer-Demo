using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Aggiungi il Photon Transform View programmatically se non già presente
        if (!gameObject.GetComponent<PhotonTransformView>())
        {
            PhotonTransformView photonTransformView = gameObject.AddComponent<PhotonTransformView>();
            photonTransformView.m_SynchronizePosition = true;
            photonTransformView.m_SynchronizeRotation = true;
            photonTransformView.m_SynchronizeScale = false;
        }
    }

    void Update()
    {
        if (photonView.IsMine) // Controlla se il GameObject è controllato dal giocatore locale
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine) // Assicura che solo il giocatore locale possa muovere il GameObject
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
