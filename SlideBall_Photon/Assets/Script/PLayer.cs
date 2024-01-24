using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player : MonoBehaviourPun, IPunObservable
{
    public float baseSpeed;
    public float accelerationRate;
    public float decelerationRate;
    public float maxSpeed;

    private bool isSliding;
    private Vector2 slideDirection;

    public float dampingFactor;

    private float Slide;

    public float originalSlideValue;

    private Vector3 networkPosition;
    private float currentSpeed;

    PhotonView view;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();

        if (!view.IsMine)
        {
            // Disable script for remote players
            enabled = false;
        }

        currentSpeed = baseSpeed;
    }

    public void ResetPosition(Vector3 position)
    {
        rb.position = position;
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            // Get input for movement
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Calculate acceleration based on input
            Vector2 acceleration = new Vector2(horizontalInput, verticalInput).normalized * accelerationRate;

            // Update current speed
            if (acceleration.magnitude > 0)
            {
                currentSpeed = Mathf.Clamp(currentSpeed + accelerationRate * Time.fixedDeltaTime, 0f, maxSpeed);
                isSliding = false;
            }
            else
            {
                // Decelerate when no input is detected
                currentSpeed = Mathf.Clamp(currentSpeed - decelerationRate * Time.fixedDeltaTime, 0f, maxSpeed);

                // Start sliding when decelerating
                if (currentSpeed > 0 && !isSliding)
                {
                    isSliding = true;
                    slideDirection = rb.velocity.normalized; // Calculate input direction for sliding
                    Slide = originalSlideValue;
                
                }

            }

            // Apply sliding force continuously when sliding
            if (isSliding)
            {
                rb.AddForce(slideDirection * originalSlideValue, ForceMode2D.Force); // Adjust the force factor as needed
           
                Slide *= dampingFactor;

                if (Slide < 0.01f)
            {
                isSliding = false;
                
            }
            }

            // Move the player using Rigidbody and forces
            rb.velocity = new Vector2(horizontalInput, verticalInput).normalized * currentSpeed;

            // Set sprite direction
            spriteRenderer.flipX = Mathf.Approximately(horizontalInput, 0f) ? spriteRenderer.flipX : horizontalInput > 0;

            // Update network position
            networkPosition = transform.position;
        }
        else
        {
            // Smoothly interpolate to the network position
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.fixedDeltaTime * 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(currentSpeed);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            currentSpeed = (float)stream.ReceiveNext();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (view != null && view.IsMine && collision.gameObject.CompareTag("SoccerBall"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                // Get the player's input
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float verticalInput = Input.GetAxisRaw("Vertical");

                // Use the input to determine the force direction
                Vector2 forceDirection = new Vector2(horizontalInput, verticalInput).normalized;

                // Adjust the force magnitude based on the desired strength
                float forceMagnitude = 0.01f;

                // Apply force to the ball
                ballRb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Force);
            }
        }

    }

    
        
    }


