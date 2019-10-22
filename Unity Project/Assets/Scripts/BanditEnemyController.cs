using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditEnemyController : MonoBehaviour {

    private Rigidbody2D rb;
    private bool facingLeft = true;
    private Transform banditTransform;
    public Transform sightStart, sightEnd;
    public bool playerSpotted = false;
    float banditSpriteWidth;
    public float speed;
    public bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private Vector2 knockbackVector = new Vector2(20, 10);

    public int health = 50;

    // Use this for initialization
    void Start () {
        InvokeRepeating("wander", 0f, Random.Range(2,6));
        rb = GetComponent<Rigidbody2D>();
        banditSpriteWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        banditTransform = this.transform;
    }
	
	// Update is called once per frame
	void Update () {
        Raycasting();
        
    }

    private void FixedUpdate()
    {
        
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Raycasting()
    {
        playerSpotted = Physics2D.Linecast(sightStart.position, sightEnd.position, 1<<LayerMask.NameToLayer("Player"));
    }

    private void banditMovement()
    {
        //moves Bandit on path towards player when playerSpotted is true
    }

    private void wander()
    {
        //Checks to see if bandit will walk off ledge
        Vector2 lineCastPosition = banditTransform.position - banditTransform.right * banditSpriteWidth;
        isGrounded = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down, 1<<LayerMask.NameToLayer("Ground"));

        if(!isGrounded)
        {
            Flip();
        }

        //Moves Bandit forward
        Vector2 banditVelocity = rb.velocity;
        banditVelocity.x = -banditTransform.right.x * speed;
        rb.velocity = banditVelocity;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(25);
        }
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;
        knockback();
        if (health <= 0)
        {
            Destroy(rb.gameObject);
        }
    }

    private void knockback()
    {
        GameObject player = GameObject.Find("Player");
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        knockbackVector.x += -rb.velocity.x;

        if (playerRB.position.x > rb.position.x)
            knockbackVector.x *= -1;

        rb.AddForce(knockbackVector, ForceMode2D.Impulse);
    }
}
