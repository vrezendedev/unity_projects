using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Movement")]
    bool heldUp = false;
    bool heldLeft = false;
    bool heldRight = false;
    bool isAbducting = false;
    bool isOnGround = false;
    [SerializeField]float fuel = 100f;
    bool usingFuel = false;
    [SerializeField] float impulseForce = 0.25f;
    [SerializeField] float ImpulseSpeed = 2f;

    [Header("Abducting")]
    bool canAbduct = true;
    float initialGravity;
    [SerializeField]float abductGravity = 0.15f;
    float initalDrag;
    [SerializeField]float abductDrag = 3;
    [SerializeField] float abductSpeed = 1.4f;
    [SerializeField] GameObject particle;

    [Header("Health")]
    [SerializeField] float health = 100;

    Rigidbody2D playerRb;
    PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.MoveUp.performed += MoveUpPerformed;
        playerInputActions.Player.MoveLeft.performed += MoveLeftPerformed;
        playerInputActions.Player.MoveRight.performed += MoveRightPerformed;
        playerInputActions.Player.Abduct.performed += AbductPerfomed;
        playerInputActions.Player.Enable();
    }

    private void Start()
    {
        initialGravity = playerRb.gravityScale;
        initalDrag = playerRb.drag;
    }
    
    private void Update()
    {
        StartCoroutine(WaitAndUseFuel());
    }

    private void FixedUpdate()
    {
        if (heldUp && isAbducting == false)
        {
            Movement(0f, impulseForce * ImpulseSpeed);
        }

        if ((heldLeft && heldRight == false) && isOnGround == false)
        {
            Movement(-impulseForce * ImpulseSpeed, 0f);
        }

        if ((heldRight && heldLeft == false) && isOnGround == false)
        {
            Movement(impulseForce * ImpulseSpeed, 0f);
        }

        if(isAbducting == true)
        {
            Abduct();
        }
        else
        {
            playerRb.gravityScale = initialGravity;
            playerRb.drag = initalDrag;
        }
    }

    private void MoveUpPerformed(InputAction.CallbackContext context)
    {
        heldUp = !heldUp;
    }

    private void MoveLeftPerformed(InputAction.CallbackContext context)
    {
        heldLeft = !heldLeft;

    }

    private void MoveRightPerformed(InputAction.CallbackContext context)
    {
        heldRight =! heldRight;

    }

    private void AbductPerfomed(InputAction.CallbackContext context)
    {
        isAbducting = !isAbducting;
    }

    private void Abduct()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        if ((fuel > 0 || health > 0) && canAbduct == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 5f, layerMask);

            GameObject particleInstance = particle;
            Instantiate(particleInstance, gameObject.transform);

            playerRb.gravityScale = abductGravity;
            playerRb.drag = abductDrag;

            if (hit && hit.collider.gameObject.tag == "Animal")
            {
                GameObject animal = hit.collider.gameObject;
                animal.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, abductSpeed, 0f);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * hit.distance, Color.red);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Animal" && isAbducting == true)
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Animal" && isAbducting == false)
        {
            TakeDamage(5);
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(collision.gameObject, 0.15f);
        }

        if(collision.gameObject.tag == "Environment")
        {
            if (playerRb.velocity.x > 1 || playerRb.velocity.y > 1) 
            {
                TakeDamage(20);
            }
            else if(isAbducting == false)
            {
                TakeDamage(2.5f);
            }

            canAbduct = false;
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            canAbduct = true;
            isOnGround = false;
        }
    }

    private void Movement(float dirX, float dirY)
    {
        if (fuel > 0 && health > 0)
        {
            playerRb.AddForce(new Vector2(dirX, dirY), ForceMode2D.Impulse);
        }
    }

    IEnumerator WaitAndUseFuel()
    {
        if ((heldUp || (heldRight && isOnGround == false) || (heldLeft && isOnGround == false)) && usingFuel == false)
        {
            usingFuel = true;
            yield return new WaitForSeconds(0.75f);
            if(heldUp == true && heldRight == true || heldUp == true && heldLeft == true)
            {
                fuel = fuel - 5f;
            }
            else
            {
                fuel = fuel - 2.50f;
            }
            Debug.Log("Current fuel: " + fuel);
            if(fuel > 0)
            {
                usingFuel = false;
            }
        }  
    }

    public void TakeDamage(float dmg)
    {
        health = health - dmg;
        Debug.Log("Current health: " + health);
    }
    
    public float GetCurrentFuel()
    {
        return fuel;
    }

    public float GetCurrentHealth()
    {
        return health;
    }
}


