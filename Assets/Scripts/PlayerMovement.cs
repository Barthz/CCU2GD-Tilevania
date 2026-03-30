using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;

        myAnimator.SetBool("isRunning", hasHorizontalSpeed());
    }

    void FlipSprite()
    {
        if (hasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    
    }

    bool hasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
    }
}
