using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;

    [Header("Vertical movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.36f;
    public Vector3 colliderOffset;

    // Update is called once per frame
    void Update() 
    {
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if (!wasOnGround && onGround) 
        {
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));    
        }

        if(Input.GetButtonDown("Jump")) 
        {
            jumpTimer = Time.time + jumpDelay;
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate() 
    {
        MoveCharacter(direction.x);
        if(jumpTimer > Time.time && onGround) 
        {
            Jump();
        }
        ModifyPhysics();
    }

    void MoveCharacter(float horizontal) 
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        // animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) 
        {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) 
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }

    void ModifyPhysics() {
        // When OR when player moves faster than 0.4 units per sec 
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        { // If player is touching ground then ->
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections) 
            { 
                rb.drag = linearDrag;
            }
            else 
            { 
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else 
        { // If player is NOT touching ground then ->
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;

            if(rb.velocity.y < 0) 
            {
                rb.gravityScale = gravity * fallMultiplier;
            }

            else if (rb.velocity.y > 0 && !Input.GetButtonDown("Jump")) 
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds) 
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0) 
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
