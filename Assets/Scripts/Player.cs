using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player controller
/// </summary>
public class Player : MonoBehaviour
{
    public float m_MovementSpeed;
    public PlayerSkin m_Skin;
    public PlayerInventory m_Inventory;
    public bool m_Interacting;

    private bool m_CanInteract;
    private bool m_Fliped;
    private Rigidbody2D m_Rigidbody;
    private Vector2 m_MovementVector;
    private Animator m_Animator;
    private InteractionEvents m_InteractableObject;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_Skin = GetComponent<PlayerSkin>();
        m_Inventory = GetComponent<PlayerInventory>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponentInChildren<Animator>();
        m_AudioSource = GetComponentInChildren<AudioSource>();
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = m_MovementVector * m_MovementSpeed;
    }

    /// <summary>
    /// Change amount of coins that player owns
    /// </summary>
    /// <param name="a_Amount"></param>
    public void ChangeCoins(int a_Amount)
    {
        m_Inventory.SetCoins(m_Inventory.m_Coins + a_Amount);
    }
    /// <summary>
    /// Interaction with NPC
    /// </summary>
    /// <param name="context"></param>
    public void Interact(InputAction.CallbackContext context)
    {
        if (!m_CanInteract) return;

        if (!m_Interacting)
        {
            m_Interacting = true;
            m_Animator.SetBool("Walking", false);
            m_InteractableObject?.m_EventsToInvoke?.Invoke();
        }
    }
    /// <summary>
    /// Player movement
    /// </summary>
    /// <param name="context"></param>
    public void Movement(InputAction.CallbackContext context)
    {
        if (m_Interacting) return;

        m_MovementVector = context.ReadValue<Vector2>();

        if (context.performed)
        {
            m_AudioSource.Play();
            m_Animator.SetBool("Walking", true);
            if (m_MovementVector.x < 0 && !m_Fliped)
            {
                m_Fliped = true;
                FlipCharacter();
            }
            if(m_MovementVector.x > 0 && m_Fliped)
            {
                m_Fliped = false;
                FlipCharacter();
            }
        }
        if (context.canceled)
        {
            m_AudioSource.Stop();
            m_Animator.SetBool("Walking", false);
        }
    }
    /// <summary>
    /// Flip palyer sprite on X
    /// </summary>
    private void FlipCharacter()
    {
        Vector3 _flipScale = transform.localScale;
        _flipScale.x *= -1;
        transform.localScale = _flipScale;
    }
    /// <summary>
    /// Stop player movement and restart animator
    /// </summary>
    public void StopPlayer()
    {
        m_Animator.Rebind();
        m_Rigidbody.velocity = Vector3.zero;
        m_MovementVector = Vector3.zero;
        m_AudioSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("NPC"))
        {
            m_CanInteract = true;
            m_InteractableObject = collision.GetComponent<InteractionEvents>();
            collision.GetComponent<NPC>()?.EnableInteractionIcon(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            m_CanInteract = false;
            m_InteractableObject = null;
            collision.GetComponent<NPC>()?.EnableInteractionIcon(false);
        }
    }
}
