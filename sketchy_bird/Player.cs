using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Idle")]
    [SerializeField] float idleSpeed;

    [Header("Movement")]
    [SerializeField] float jumpHeight;
    [SerializeField] float rotation;

    [Header("Points")]
    [SerializeField] AudioClip pointAudio;
    int points;

    [Header("Game Over")]
    [SerializeField] Sprite deadPlayer;
    float gravityOnStart = 1.17f;
    [SerializeField] float gravityOnCollision;
    CameraShake cameraShake;

    Rigidbody2D playerRb;
    Collider2D playerCollider;
    Animator playerAnimator;
    SpriteRenderer playerRenderer;

    bool play = false;
    bool cantMove = false;
    bool isAlive;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerAnimator = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();
        cameraShake = FindObjectOfType<CameraShake>();
    }

    private void Start()
    {
        points = 0;
        playerRb.gravityScale = gravityOnStart;
        isAlive = true;
    }

    private void Update()
    {
        
        isPlaying();
        PlayerRotation();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        var alreadyJump = context.started;
        play = true;

        if (alreadyJump == true && cantMove == false)
        {
            playerRb.velocity = playerRb.velocity + new Vector2(0f, jumpHeight);

        }
    }

    private void PlayerRotation()
    {
        var dirZ = playerRb.velocity;
        float zValue = dirZ.y * rotation;

        if (dirZ != Vector2.zero && play == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zValue));
        }
    }

    public int GetPoints()
    {
        return points;
    }

    private void isPlaying()
    {
        if (play == false)
        {
            playerRb.isKinematic = true;
            float y = Mathf.PingPong(Time.time * idleSpeed, 1) * 2 - 1;
            transform.position = new Vector3(0f, y, 0f);
        }
        else
        {
            playerRb.isKinematic = false;
        }
    }

    private void Die()
    {
        cantMove = true;
        isAlive = false;
        playerRenderer.sprite = deadPlayer;
        playerRb.gravityScale = gravityOnCollision;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Pipes")
        {
            Die();
            cameraShake.Play();
        }
    }

    public bool isPlayerAlive()
    {
        return isAlive;
    }

    public bool isPlayerReady()
    {
        return play;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isAlive == true && collision.tag != "Boundary")
        {
            points++;
            AudioSource.PlayClipAtPoint(pointAudio, transform.position, 0.15f);
        }

        if(collision.tag == "Boundary")
        {
            Die();
        }
    }


}
