using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [HideInInspector]
    public new Rigidbody2D rigidbody {get; private set;}
    private Vector2 direction = Vector2.down;
    [SerializeField]
    private float speed = 5.0f;

    //inputs
    [SerializeField]
    private KeyCode inputUp = KeyCode.W;
    [SerializeField]
    private KeyCode inputDown = KeyCode.S;
    [SerializeField]
    private KeyCode inputLeft = KeyCode.A;
    [SerializeField]
    private KeyCode inputRight = KeyCode.D;

    [SerializeField]
    private AnimatedSpriteRenderer spriteRendererUp;
    [SerializeField]
    private AnimatedSpriteRenderer spriteRendererDown;
    [SerializeField]
    private AnimatedSpriteRenderer spriteRendererLeft;
    [SerializeField]
    private AnimatedSpriteRenderer spriteRendererRight;

    [SerializeField]
    private AnimatedSpriteRenderer spriteRendererDeath;

    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update() 
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate() 
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    public void AddSpeed()
    {
        speed++;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererUp.enabled = false;

        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }
}
