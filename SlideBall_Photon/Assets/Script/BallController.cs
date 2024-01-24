using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f;
    public float slideFriction = 0.2f;



    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        // Example: Apply sliding friction to the ball
        if (rb.velocity.magnitude > 0)
        {
            // Apply sliding friction only when the ball is moving
            rb.velocity = rb.velocity * (1f - slideFriction * Time.deltaTime);
        }
    }
    



   
    
}
